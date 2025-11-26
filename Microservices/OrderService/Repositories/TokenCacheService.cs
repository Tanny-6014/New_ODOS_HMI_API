using Microsoft.Extensions.Caching.Memory;
using OrderService.Interfaces;
using System.Net;
using System.Text;

namespace OrderService.Repositories
{
    public class TokenCacheService: ITokenCacheService
    {
        private IMemoryCache _cache;
        public TokenCacheService(IMemoryCache memoryCache)
        {
            _cache = memoryCache;
        }

        public void SaveToken(string key, string token, TimeSpan expirationTime)
        {
            // Save the token in cache with the specified expiration time
            _cache.Set(key, token, expirationTime);
        }

        public string GetToken(string key)
        {
            // Retrieve the token from cache

            if (_cache.TryGetValue(key, out string token))
            {
                return token;
            }
            return null; // Token not found in cache
        }

        public class Claim
        {
            public string issuer { get; set; }
            public string originalIssuer { get; set; }
            public Dictionary<string, object> properties { get; set; }
            public object subject { get; set; }
            public string type { get; set; }
            public string value { get; set; }
            public string valueType { get; set; }
        }

        public class Payload
        {
            public string email { get; set; }
            public string Name { get; set; }
            public string LoginId { get; set; }
            public string UserGroup { get; set; }
            public string Department { get; set; }
            public string FinAccess { get; set; }
            public string ContactNumber { get; set; }
            public string CustomerData { get; set; }
            public string jti { get; set; }
            public long exp { get; set; }
            public string iss { get; set; }
            public string aud { get; set; }
        }

        public class RootObject
        {
            public object actor { get; set; }
            public List<string> audiences { get; set; }
            public List<Claim> claims { get; set; }
            public string encodedHeader { get; set; }
            public string encodedPayload { get; set; }
            public Dictionary<string, object> header { get; set; }
            public string id { get; set; }
            public string issuer { get; set; }
            public Payload payload { get; set; }
            public object innerToken { get; set; }
            public object rawAuthenticationTag { get; set; }
            public object rawCiphertext { get; set; }
            public string rawData { get; set; }
            public object rawEncryptedKey { get; set; }
            public object rawInitializationVector { get; set; }
            public string rawHeader { get; set; }
            public string rawPayload { get; set; }
            public string rawSignature { get; set; }
            public object securityKey { get; set; }
            public string signatureAlgorithm { get; set; }
            public object signingCredentials { get; set; }
            public object encryptingCredentials { get; set; }
            public object signingKey { get; set; }
            public object subject { get; set; }
            public DateTime validFrom { get; set; }
            public DateTime validTo { get; set; }
            public DateTime issuedAt { get; set; }
        }

     
        public async Task<string> GetUserName()
        {
            try
            {

                var lToken = GetToken("AuthToken");
                //  var tempobj= JsonConvert.DeserializeObject<T>(lToken);

                // MyDetails myDetails = JsonConvert.DeserializeObject<MyDetails>(lToken);

                if (lToken == "")
                {
                    // Throw an error 
                    // To be handled
                    return "";
                }
                else
                {
                    HttpClient httpClient = new HttpClient();

                    //string apiUrl = "http://172.25.1.224:98/IndexUMP"; //Route to be updated
                    string apiUrl = "https://localhost:7297/IndexUMP";

                    // HttpContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                    string apiUrlWithParameters = string.Format("{0}/{1}", apiUrl, lToken);
                    HttpResponseMessage response = httpClient.GetAsync(apiUrlWithParameters).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        string responseContent = await response.Content.ReadAsStringAsync();
                        RootObject data = System.Text.Json.JsonSerializer.Deserialize<RootObject>(responseContent);
                        var UserName = data.payload.LoginId;// response.payload.LoginId;
                        if (UserName != null || UserName != "")
                        {
                            return UserName;
                        }
                        else
                        {
                            return "";
                        }

                    }
                    else
                    {
                        if (response.StatusCode == HttpStatusCode.BadRequest)
                        {
                            return "";

                        }

                        return "";
                    }
                }

            }
            catch (Exception)
            {
                return "";
                //throw;
            }
        }
    }
}
