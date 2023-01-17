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
            var result = new List<AdvertType>();
            string callUrl = $"{_baseAddress}/{keyword}";
            //HttpContent content = new StringContent("application/json");

            var response = await _httpClient.GetAsync(new Uri(callUrl));
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var content = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var Alladverts = JsonSerializer.Deserialize<List<AdvertType>>(content, options);
                result.AddRange(Alladverts);
            }
            return result;
        }
    }
}
