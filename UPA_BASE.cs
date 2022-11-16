using Google.Authenticator;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UPA_EXTERNAL_MODELS;
using UPAExternalAPI.Models;

namespace UPA_SDK
{
    public abstract class UPA_BASE
    {
        public string API_SECRET { get; }
        public int SERVER_OTP_VALIDATION_WINDOW { get; }
        public int MIN_TIME_BETWEEN_API_CALLS { get; }
        public string API_ROOT_URL { get; }
        public string API_KEY { get; }
        public string API_USER { get; }
        public string API_PASSWORD { get; }
        public string API_OTP_SECRET { get; }
        public string JWT_TOKEN { get; private set; }
        public DateTime LastAuthTime { get; private set; }

        /// <summary>
        /// This Constructor Used by 
        /// </summary>
        /// <param name="API_ROOT_URL"></param>
        /// <param name="API_KEY"></param>
        /// <param name="API_USER"></param>
        /// <param name="API_PASSWORD"></param>
        /// <param name="API_OTP_SECRET"></param>
        /// <param name="API_SECRET"></param>
        /// <param name="SERVER_OTP_VALIDATION_WINDOW_SECONDS">
        /// Time Difference To Validate Server Inoming OTP While Login
        /// Default 60 (Too Long, But Never Mind its nice)
        /// </param>
        public UPA_BASE
            (
            string API_ROOT_URL,
            string API_KEY,
            string API_USER,
            string API_PASSWORD,
            string API_OTP_SECRET,
            string API_SECRET,
            int MIN_TIME_BETWEEN_API_CALLS,
            int SERVER_OTP_VALIDATION_WINDOW_SECONDS = 60
            )
        {
            this.API_ROOT_URL = API_ROOT_URL.Trim().TrimEnd('/');
            this.API_KEY = API_KEY;
            this.API_USER = API_USER;
            this.API_PASSWORD = API_PASSWORD;
            this.API_OTP_SECRET = API_OTP_SECRET;
            this.API_SECRET = API_SECRET;
            this.SERVER_OTP_VALIDATION_WINDOW = SERVER_OTP_VALIDATION_WINDOW_SECONDS;
            this.MIN_TIME_BETWEEN_API_CALLS = MIN_TIME_BETWEEN_API_CALLS;
        }


        public ResponseContainer<T> Form_POST<T>(string ServiceURL, MultipartFormDataContent outPayload)
        {
            HttpClient client = GetClientPrepared();
            var p = client.PostAsync(
                $"{this.API_ROOT_URL}{ServiceURL}", outPayload);
            p.Wait();
            return Introduce_Result<T>(p);
        }

        public ResponseContainer<T> JSON_POST<T>(string ServiceURL, object outPayload)
        {
            HttpClient client = GetClientPrepared();
            var p = client.PostAsJsonAsync(
                $"{this.API_ROOT_URL}{ServiceURL}", outPayload);
            p.Wait();
            return Introduce_Result<T>(p);
        }
        /// <summary>
        /// Get URL Of The Service Formated with any Parameters
        /// </summary>
        /// <param name="ServiceGetURL"></param>
        public ResponseContainer<T> JSON_Get<T>(string ServiceGetURL)
        {
            HttpClient client = GetClientPrepared();
            var p = client.GetAsync($"{this.API_ROOT_URL}{ServiceGetURL}",
                HttpCompletionOption.ResponseContentRead);
            p.Wait();
            //Console.WriteLine(p.Result);
            return Introduce_Result<T>(p);
        }
        /// <summary>
        /// Download PDF From Server
        /// </summary>
        /// <param name="ServiceGetURL"></param>
        public Stream JSON_Download(string ServiceGetURL)
        {
            try
            {
                HttpClient client = GetClientPrepared();
                var p = client.GetStreamAsync($"{this.API_ROOT_URL}{ServiceGetURL}");
                p.Wait();
                return p.Result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }
        private static ResponseContainer<T> Introduce_Result<T>(Task<HttpResponseMessage> p)
        {
            if (!p.Result.IsSuccessStatusCode)
            {
                ResponseContainer<T> result = new();
                var errorResult = p.Result.Content.ReadAsStringAsync();
                errorResult.Wait();
                result.ErrorCode = 1;
                result.ErrorMessage = errorResult.Result;
                result.Result = default;
                return result;
            }
            var pString = p.Result.Content.ReadAsStringAsync();
            pString.Wait();
            return JsonConvert.DeserializeObject<ResponseContainer<T>>(pString.Result);
        }

        private HttpClient GetClientPrepared()
        {
            var client = new HttpClient();
            PrepareHttpClient(client);
            client.BaseAddress = new Uri(API_ROOT_URL);
            client.DefaultRequestHeaders.Accept.Clear();
            return client;
        }

        public void PrepareHttpClient(HttpClient client)
        {
            string CurrentOTP = this.CurrentOTP;
            var Digest = HashHMACString($"{API_SECRET}{CurrentOTP}", this.JWT_TOKEN);
            client.DefaultRequestHeaders.Add("Authorization", this.JWT_TOKEN);
            client.DefaultRequestHeaders.Add("OTP", CurrentOTP);
            client.DefaultRequestHeaders.Add("UPAD", Digest);
        }
        public static string HashHMACString(string key_string, string message_string)
        {
            return ByteArrayToString(HashHMAC(key_string, message_string));
        }
        public static byte[] HashHMAC(string key_string, string message_string)
        {
            byte[] key = Encoding.ASCII.GetBytes(key_string);
            byte[] message = Encoding.ASCII.GetBytes(message_string);
            var hash = new HMACSHA256(key);
            return hash.ComputeHash(message);
        }
        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        public bool Login()
        {
            var reso = InternalLogin();
            if (reso.ErrorCode != 0)
                return false;
            if (ValidateOTP(reso.Result.OTP))// Server Authentication You Can Hash This Part
            {
                this.JWT_TOKEN = reso.Result.Token;
                this.LastAuthTime = DateTime.Now;
                return true;
            }
            return false;
        }
        private ResponseContainer<AuthenticateResponse> InternalLogin()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(API_ROOT_URL);
            client.DefaultRequestHeaders.Accept.Clear();
            var outPayload = new AuthenticateRequest
            {
                API_KEY = this.API_KEY,
                API_USER = this.API_USER,
                API_PASSWORD = this.API_PASSWORD,
            };
            var p = client.PostAsJsonAsync(
                $"{this.API_ROOT_URL}/Users/authenticate", outPayload);
            p.Wait();
            var pStringX = p.Result.Content.ReadAsStringAsync();
            pStringX.Wait();
            if (!p.Result.IsSuccessStatusCode)
            {
                ResponseContainer<AuthenticateResponse> result = new();
                result.ErrorCode = 1;
                result.ErrorMessage = "LOGIN ERROR";
                result.Result = null;
                return result;
            }
            var pString = p.Result.Content.ReadAsStringAsync();
            pString.Wait();
            return JsonConvert.DeserializeObject<ResponseContainer<AuthenticateResponse>>(pString.Result);
        }
        public bool ValidateOTP(string incomingOTP)
        {
            TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();
            return tfa.ValidateTwoFactorPIN
                (
                this.API_OTP_SECRET,
                incomingOTP,
                TimeSpan.FromSeconds(SERVER_OTP_VALIDATION_WINDOW)
                );
        }
        public string CurrentOTP
        {
            get
            {
                TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();
                return tfa.GetCurrentPIN(this.API_OTP_SECRET, false);
            }
        }
    }
}
