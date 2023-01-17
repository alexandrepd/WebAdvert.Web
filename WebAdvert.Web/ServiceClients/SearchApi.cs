using System.Text.Json;
using WebAdvert.Web.Models;

namespace WebAdvert.Web.ServiceClients
{
    public class SearchApi : ISearchApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseAddress = string.Empty;

        public SearchApi(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _baseAddress = configuration.GetSection("SearchApi").GetValue<string>("url");
        }

        public async Task<List<AdvertType>> Search(string keyword)
        {
            List<AdvertType> result = new List<AdvertType>();
            string callUrl = $"{_baseAddress}/{keyword}";
            
            HttpResponseMessage response = await _httpClient.GetAsync(new Uri(callUrl));

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string content = await response.Content.ReadAsStringAsync();
                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                result.AddRange(JsonSerializer.Deserialize<List<AdvertType>>(content, options)!);
            }
            return result;
        }
    }
}
