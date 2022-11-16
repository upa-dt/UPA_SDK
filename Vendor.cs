using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using UPA_EXTERNAL_MODELS;
using UPA_EXTERNAL_MODELS.Models.Common;
using UPA_EXTERNAL_MODELS.Models.Vendor;
using UPAExternalAPI.Models.Vendor;

namespace UPA_SDK
{
    public class Vendor : UPA_BASE
    {
        public Vendor(
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
        public ResponseContainer<List<PoSearchResponse>> PoSearch(PoSearchRequest PoSearch)
        {
            var result = base.JSON_POST<List<PoSearchResponse>>("/Vendor/poSearch", PoSearch);
            //result.SerializeObject().ToFile("PoSearch131.json");
            return result;
        }
        public ResponseContainer<List<PoDetailResponse>> PoGetPoDet(int po)
        {
            var result = base.JSON_Get<List<PoDetailResponse>>($"/Vendor/poGetPoDet/{po}");
            result.SerializeObject().ToFile("PoGetPoDet132.json");
            return result;
        }
        /// <summary>
        /// Get Po Pdf File and write the resulting stream to the OutputFile
        /// </summary>
        /// <param name="po">
        /// Purchase Order Number
        /// </param>
        /// <param name="OutputFile">
        /// Output File
        /// If Null No file will be created
        /// If File Exists it will be override
        /// </param>
        /// <returns></returns>
        public Stream PoGetPdf(int po, string OutputFile)
        {
            var result = base.JSON_Download($"/Vendor/poGetPdf/{po}");
            if (OutputFile != null)
                using (var fileStream = File.Create(OutputFile))
                {
                    result.CopyTo(fileStream);
                    fileStream.Flush();
                }
            return result;
        }

        public ResponseContainer<List<PoDistributionResult>> PoDistributeList(PoDistributionQuery distributionQuery)
        {
            var result = base.JSON_POST<List<PoDistributionResult>>("/Vendor/poDistributeList", distributionQuery);
            result.SerializeObject().ToFile("distributionQuery8888.json");
            return result;
        }


        public ResponseContainer<List<PoDistributionResultDetails>> PoDistributeListDetailedItems(PoDistributionDetailsQuery distributionQuery)
        {
            distributionQuery.SerializeObject().ToFile("distributionQuery.json");
            var result = base.JSON_POST<List<PoDistributionResultDetails>>("/Vendor/poDistributeListDetailedItems", distributionQuery);
            result.SerializeObject().ToFile("PoDistributeListDetailedItems1878233.json");
            return result;
        }

        public ResponseContainer<RiResponse> CreateRI(RiRequest NewRi)
        {
            var result = base.JSON_POST<RiResponse>("/Vendor/createRI", NewRi);
            result.SerializeObject().ToFile("newRi.json");
            return result;
        }
        public ResponseContainer<List<RiSearchResponse>> SearchRI(RiSearchRequest riSearchRequest)
        {
            var result = base.JSON_POST<List<RiSearchResponse>>("/Vendor/searchRi", riSearchRequest);
            result.SerializeObject().ToFile("searchRi1.json");
            return result;
        }
        /// <summary>
        /// Get Po Pdf File and write the resulting stream to the OutputFile
        /// </summary>
        /// <param name="ri">
        /// Reception Invoice Number
        /// </param>
        /// <param name="combineWithPo">
        /// Weather to combine the requested Ri with its corresponding Partial Purchase order or not
        /// True means Yes Combine with Partial PO
        /// False means No Don't Combine with Partial Po
        /// </param>
        /// <param name="OutputFile">
        /// Output File
        /// If Null No file will be created
        /// If File Exists it will be override
        /// </param>
        /// <returns></returns>
        public Stream RiGetPdf(int ri, bool combineWithPo, string OutputFile)
        {
            var result = base.JSON_Download($"/Vendor/riGetPdf/{ri}/{(combineWithPo ? 1 : 0)}");
            if (OutputFile != null)
                using (var fileStream = File.Create(OutputFile))
                {
                    result.CopyTo(fileStream);
                    fileStream.Flush();
                }
            return result;
        }

        public ResponseContainer<List<GetReturnedItemsResponse>> GetReturnedItems()
        {
            var result = base.JSON_Get<List<GetReturnedItemsResponse>>($"/Vendor/getReturnedItems");
            //result.SerializeObject().ToFile("GetReturnedItems.json");
            return result;
        }

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
                var result = base.Form_POST<RiReceiveResponse>($"/Vendor/riReceive", multipartFormContent);
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
    }
}
