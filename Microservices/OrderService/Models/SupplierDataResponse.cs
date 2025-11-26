using Newtonsoft.Json;

namespace OrderService.Models
{
    public class SupplierDataResponse
    {
        [JsonProperty("CODE")]
        public string Code { get; set; }

        [JsonProperty("ERROR_MSG")]
        public string ErrorMessage { get; set; }

        [JsonProperty("SUPPLIERS")]
        public List<Supplier> Suppliers { get; set; }

        public class Supplier
        {
            [JsonProperty("SUPPLIER_CODE")]
            public string SupplierCode { get; set; }

            [JsonProperty("SUPPLIER_NAME")]
            public string SupplierName { get; set; }

            [JsonProperty("PRODUCT_TYPE")]
            public string ProductType { get; set; }

            [JsonProperty("SERVICE_TYPE")]
            public string ServiceType { get; set; }

            [JsonProperty("CONTRACT_LIST")]
            public List<ContractList> ContractList { get; set; }
         
        }

        public class ContractList
        {
            [JsonProperty("CONTRACT_NO")]
            public string ContractNo { get; set; }

            [JsonProperty("CONTRACT_DESC")]
            public string ContractDesc { get; set; }
        }

        public class SupplierRes
        {
            public string SupplierCode { get; set; }
            public string SupplierName { get; set; }
            public string ProductType { get; set; }
            public string ServiceType { get; set; }
            public List<ContractList> Contracts { get; set; }
        }
    }
}
