using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        { }
    }
}
