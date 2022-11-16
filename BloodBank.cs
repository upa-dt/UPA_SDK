using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UPA_EXTERNAL_MODELS.Models.BloodBanks;
using UPA_EXTERNAL_MODELS.Models.Lookup;

namespace UPA_SDK
{
    public class BloodBank : UPA_BASE
    {
        public BloodBank(
           string API_ROOT_URL,
          string API_KEY,
          string API_USER,
          string API_PASSWORD,
          string API_OTP_SECRET,
          string API_SECRET,
           int MIN_TIME_BETWEEN_API_CALLS_SECONDS,
            int SERVER_OTP_VALIDATION_WINDOW_SECONDS = 60
          ) : base(API_ROOT_URL,
            API_KEY,
            API_USER,
            API_PASSWORD,
            API_OTP_SECRET,
            API_SECRET,
            MIN_TIME_BETWEEN_API_CALLS_SECONDS,
            SERVER_OTP_VALIDATION_WINDOW_SECONDS
            )
        { }

        public ResponseContainer<List<LookupBase>> GetBloodGroups()
        {
            return base.JSON_Get<List<LookupBase>>("/BloodBank/getBloodGroups");
        }
        public ResponseContainer<List<LookupBase>> GetBloodRh()
        {
            return base.JSON_Get<List<LookupBase>>("/BloodBank/getBloodRh");
        }
        public ResponseContainer<List<LookupBase>> GetProfessions()
        {
            return base.JSON_Get<List<LookupBase>>("/BloodBank/getProfessions");
        }
        public ResponseContainer<List<LookupBase>> GetBLoodFormType()
        {
            return base.JSON_Get<List<LookupBase>>("/BloodBank/getBLoodFormType");
        }
        public ResponseContainer<List<BloodDeferTypes>> GetDeferTypes()
        {
            return base.JSON_Get<List<BloodDeferTypes>>("/BloodBank/getDeferTypes");
        }
        public ResponseContainer<List<LookupBase>> GetCountries()
        {
            return base.JSON_Get<List<LookupBase>>("/BloodBank/getCountries");
        }
        public ResponseContainer<List<CityLookup>> GetCities()
        {
            return base.JSON_Get<List<CityLookup>>("/BloodBank/getCities");
        }
        public ResponseContainer<List<DistrictLookup>> GetDestricts()
        {
            return base.JSON_Get<List<DistrictLookup>>("/BloodBank/getDestricts");
        }
        /// <summary>
        /// Add New Donor Data
        /// If Donor Already Exists, His Info Will Be Updated
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResponseContainer<int> RegisterDonor(RegisterDonorModel model)
        {
            return base.JSON_POST<int>("/BloodBank/registerDonor", model);
        }
        public ResponseContainer<DonorSearchResult> GetDonor(DonorSearchModel model)
        {
            return base.JSON_POST<DonorSearchResult>("/BloodBank/getDonor", model);
        }

        public ResponseContainer<int> RegisterDefer(DeferralModel model)
        {
            return base.JSON_POST<int>("/BloodBank/registerDefer", model);
        }
        public ResponseContainer<int> AddInventoryTransaction(InventoryTransactionModel transactionData)
        {
            return base.JSON_POST<int>("/BloodBank/addInventoryTransaction", transactionData);
        }
        public ResponseContainer<List<InventoryRecord>> SearchInventory(InventorySearchModel model)
        {
            return base.JSON_POST<List<InventoryRecord>>("/BloodBank/searchInventory", model);
        }
    }

}
