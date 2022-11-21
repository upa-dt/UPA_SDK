using System.Net.Http.Headers;
using UPA_EXTERNAL_MODELS;
using UPA_EXTERNAL_MODELS.Models.Common;
using UPA_EXTERNAL_MODELS.Models.Customer;
using UPA_EXTERNAL_MODELS.Models.Vendor;
using UPAExternalAPI.Models.Customer;
using UPAExternalAPI.Models.Inventory;

namespace UPA_SDK
{
    public class Customer : UPA_BASE
    {
        public Customer(
          string API_ROOT_URL,
          string API_KEY,
          string API_USER,
          string API_PASSWORD,
          string API_OTP_SECRET,
          string API_SECRET,
           int MIN_TIME_BETWEEN_API_CALLS,
            int SERVER_OTP_VALIDATION_WINDOW_SECONDS = 60
          ) : base(API_ROOT_URL,
            API_KEY,
            API_USER,
            API_PASSWORD,
            API_OTP_SECRET,
            API_SECRET,
            MIN_TIME_BETWEEN_API_CALLS,
            SERVER_OTP_VALIDATION_WINDOW_SECONDS
            )
        {
        }
        /// <summary>
        /// Search For Purchase requests for a customer
        /// </summary>
        /// <param name="PrSearchObject">
        /// Search parameters
        /// </param>
        /// <returns></returns>
        public ResponseContainer<List<PrData>> PrSearch(PrSearchRequest PrSearchObject)
        {
            var result = base.JSON_POST<List<PrData>>("/Customer/prSearch", PrSearchObject);
            //result.SerializeObject().ToFile("PoSearch.json");
            return result;
        }

        /// <summary>
        /// Download Purchase Request ton the specific Path and return File Stream
        /// </summary>
        /// <param name="pr">
        /// Pr Number
        /// </param>
        /// <param name="OutputFile">
        /// Output File Path
        /// </param>
        /// <returns></returns>
        public Stream PrDownload(int pr, string OutputFile)
        {

            var result = base.JSON_Download($"/Customer/prGetPdf/{pr}");
            if (OutputFile != null)
                using (var fileStream = File.Create(OutputFile))
                {
                    result.CopyTo(fileStream);
                    fileStream.Flush();
                }
            return result;
        }

        /// <summary>
        /// Download Purchase Request ton the specific Path and return File Stream
        /// </summary>
        /// <param name="pr">
        /// Purchase Request Number 
        /// </param>
        /// <param name="OutputFile">
        /// Output File Path
        /// </param>
        /// <returns></returns>
        public ResponseContainer<PrMetaData> PrGetMetaData(int pr)
        {
            var result = base.JSON_Get<PrMetaData>($"/Customer/prGetMetaData/{pr}");
            //result.SerializeObject().ToFile("prGetMetaData.json");
            return result;
        }
        /// <summary>
        /// Get List of Items for a specific Purchase Request
        /// </summary>
        /// <param name="pr"></param>
        /// <returns></returns>
        public ResponseContainer<List<PrItemData>> PrGetItems(int pr)
        {
            var result = base.JSON_Get<List<PrItemData>>($"/Customer/prGetItems/{pr}");
            //result.SerializeObject().ToFile("PrGetItems.json");
            return result;
        }


        /// <summary>
        /// Get List of Purchase Orders For Specific Purchase Requests
        /// </summary>
        /// <param name="pr">
        /// Purchase Request Number 
        /// </param>
        /// <returns>
        /// List of Purchase Orders for a specific Purchase Request
        /// </returns>
        public ResponseContainer<List<PrPoData>> PrGetPoList(int pr)
        {
            var result = base.JSON_Get<List<PrPoData>>($"/Customer/prGetPoList/{pr}");
            //result.SerializeObject().ToFile("PrGetPoList.json");
            return result;
        }

        /// <summary>
        /// Download Purchase Order for a Purchase Request
        /// </summary>
        /// <param name="pr">
        /// Purchase Request Number 
        /// </param>
        /// <param name="po">
        /// Purchase Order Number
        /// </param>
        /// <param name="OutputFile">
        /// Output File Path
        /// </param>
        /// <returns>
        /// 
        /// </returns>
        public Stream Pr_Po_Download(int pr, int po, string OutputFile)
        {

            var result = base.JSON_Download($"/Customer/prGetPoPdf/{pr}/{po}");
            if (OutputFile != null)
                using (var fileStream = File.Create(OutputFile))
                {
                    result.CopyTo(fileStream);
                    fileStream.Flush();
                }
            return result;
        }


        /// <summary>
        /// Get List of Purchase Orders Items Detail For Specific Purchase Requests
        /// </summary>
        /// <param name="pr">
        /// Purchase Request Number 
        /// </param>
        /// <param name="po">
        /// Purchase Order Number
        /// </param>
        /// <returns>
        /// List of Purchase Orders for a specific Purchase Request
        /// </returns>
        public ResponseContainer<List<PrDetailPoDetailData>> PrGetPoDet(int pr, int po)
        {
            var result = base.JSON_Get<List<PrDetailPoDetailData>>($"/Customer/prGetPoDet/{pr}/{po}");
            //result.SerializeObject().ToFile("prGetPoDet.json");
            return result;
        }


        /// <summary>
        /// Get Distribution of Purchase Request over all Purchase orders
        /// </summary>
        /// <param name="pr">
        /// Purchase Request Number 
        /// </param>
        /// <param name="det_id">
        /// Purchase request Detail Number
        /// </param>
        /// <returns>
        /// List of Purchase Orders for a specific Purchase Request
        /// </returns>
        public ResponseContainer<List<PrDetailPoDetailData>> PrGetPrDetDistribution(int pr, int det_id)
        {
            var result = base.JSON_Get<List<PrDetailPoDetailData>>($"/Customer/prGetPrDetDistribution/{pr}/{det_id}");
            //result.SerializeObject().ToFile("PrGetPrDetDistribution.json");
            return result;
        }
        /// <summary>
        /// Get List of Reception Invoices (أذون تسليم) For Specific Purchase Requests
        /// </summary>
        /// <param name="pr">
        /// Purchase Request Number 
        /// </param>
        /// <returns>
        ///  List of Reception Invoices (أذون تسليم) 
        /// </returns>
        public ResponseContainer<List<ReceptionInvoiceHeader>> PrGetRiList(int pr)
        {
            var result = base.JSON_Get<List<ReceptionInvoiceHeader>>($"/Customer/prGetRiList/{pr}");
            //result.SerializeObject().ToFile("prGetRiList.json");
            return result;
        }


        /// <summary>
        /// Get List of 
        /// </summary>
        /// <param name="Ri">
        /// Reception Invoice  (أذن تسليم) Number 
        /// </param>
        /// <returns>
        ///  List of Reception Invoices (أذن تسليم) 
        /// </returns>
        public ResponseContainer<List<RI_DetailData>> RiGetItems(int Ri)
        {
            var result = base.JSON_Get<List<RI_DetailData>>($"/Customer/riGetItems/{Ri}");
            //result.SerializeObject().ToFile("RiGetItems.json");
            return result;
        }
        /// <summary>
        /// Receive Reception Invoice
        /// This function Can Be Done using Mobile Application
        /// </summary>
        /// <param name="ri">
        /// Reception Invoice Number
        /// </param>
        /// <param name="refusedItems">
        /// List of Item ID for Refused Items
        /// Other Items in reception invoice will be considered Acepted
        /// </param>
        /// <param name="InvoiceImagePaths">
        /// Stamped Vendor Invoice Images List
        /// </param>
        /// <param name="RiImagePaths">
        /// Stamped Reception Invoice Image List
        /// </param>
        /// <returns></returns>
        public ResponseContainer<RiReceiveResponse> RiReceive(
            int ri,
            List<int> refusedItems,
            List<string> InvoiceImagePaths,
            List<string> RiImagePaths)
        {
            if (InvoiceImagePaths.IsEmptyList() || RiImagePaths.IsEmptyList()
                || InvoiceImagePaths.InvalidFileExtension() || RiImagePaths.InvalidFileExtension())
                return null;
            using (var multipartFormContent = new MultipartFormDataContent())
            {
                multipartFormContent.Add(new StringContent(ri.ToString()), "InvoiceID");
                if (refusedItems != null)
                    foreach (var refused in refusedItems)
                        multipartFormContent.Add(new StringContent(refused.ToString()), "RefusedItems");
                AddImageFiles(multipartFormContent, InvoiceImagePaths, "inv_");
                AddImageFiles(multipartFormContent, RiImagePaths, "doc_");
                var result = base.Form_POST<RiReceiveResponse>($"/Customer/riReceive", multipartFormContent);
                result.SerializeObject().ToFile("RiReceive.json");
                return result;
            }
        }
        /// <summary>
        /// Receive Reception Invoice
        /// This function Can Be Done using Mobile Application
        /// </summary>
        /// <param name="ri">
        /// Reception Invoice Number
        /// </param>
        /// <param name="refusedItems">
        /// List of Item ID for Refused Items
        /// Other Items in reception invoice will be considered Acepted
        /// </param>
        /// <param name="InvoiceImagePaths">
        /// Stamped Vendor Invoice Images List
        /// </param>
        /// <param name="RiImagePaths">
        /// Stamped Reception Invoice Image List
        /// </param>
        /// <returns></returns>
        public ResponseContainer<RiReceiveResponse> RiAccept(
            int ri,
            List<int> refusedItems,
            List<string> InvoiceImagePaths)
        {
            if (InvoiceImagePaths.IsEmptyList() || InvoiceImagePaths.InvalidFileExtension())
                return null;
            using (var multipartFormContent = new MultipartFormDataContent())
            {
                multipartFormContent.Add(new StringContent(ri.ToString()), "InvoiceID");
                if (refusedItems != null)
                    foreach (var refused in refusedItems)
                        multipartFormContent.Add(new StringContent(refused.ToString()), "RefusedItems");
                AddImageFiles(multipartFormContent, InvoiceImagePaths, "com_");
                var result = base.Form_POST<RiReceiveResponse>($"/Customer/riAccept", multipartFormContent);
                result.SerializeObject().ToFile("RiReceive.json");
                return result;
            }
        }
        private static void AddImageFiles(MultipartFormDataContent multipartFormContent, List<string> ImageList, string postfixFileName)
        {
            for (int i = 0; i < ImageList.Count; i++)
            {
                var filePath = ImageList[i];
                var fileStreamContent = new StreamContent(File.OpenRead(filePath));
                fileStreamContent.Headers.ContentType = new MediaTypeHeaderValue(filePath.GetImageExtension());
                multipartFormContent.Add(fileStreamContent, name: "Files", fileName: $"{postfixFileName}{i}{Path.GetExtension(filePath)}");
            }
        }

        /// <summary>
        /// Search Inventory
        /// </summary>
        /// <param name="PrSearchObject">
        /// Search parameters
        /// </param>
        /// <returns></returns>
        public ResponseContainer<List<InventoryContentItem>> InvSearch(InventorySearchRequest invSearchParameters)
        {
            var result = base.JSON_POST<List<InventoryContentItem>>("/Customer/invSearch", invSearchParameters);
            //result.SerializeObject().ToFile("invSearch.json");
            return result;
        }


        /// <summary>
        /// Add New Transaction to inventory
        /// </summary>
        /// <param name="transactionData">
        /// Search parameters
        /// </param>
        /// <returns></returns>
        public ResponseContainer<string> InventoryAddTransaction(InventoryTransactionRequest transactionData)
        {
            var result = base.JSON_POST<string>("/Customer/inventoryAddTransaction", transactionData);
            //result.SerializeObject().ToFile("transactionData.json");
            return result;
        }

        /// <summary>
        /// Inspect Inventory
        /// </summary>
        /// <param name="InsepectionData">
        /// Search parameters
        /// </param>
        /// <returns></returns>
        public ResponseContainer<string> InventoryInspect(InventoryInspectionRequest InsepectionData)
        {
            var result = base.JSON_POST<string>("/Customer/inventoryInspect", InsepectionData);
            //result.SerializeObject().ToFile("transactionData.json");
            return result;
        }
    }
}