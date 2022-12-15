using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using WebAdvert.Api.Models;
using WebAdvert.Web.Configuration;

namespace WebAdvert.Web.ServiceClients
{
    public class AdvertApiClient : IAdvertApiClient
    {
        private readonly AdvertApi _advertApi;
        private readonly HttpClient _httpClient;

        public AdvertApiClient(AdvertApi advertApi, HttpClient httpClient)
        {
            _advertApi = advertApi;
            _httpClient = httpClient;
            
        }

        public async Task<CreateAdvertResponse> Create(AdvertModel advertModel)
        {
            string _createEndPoint = _advertApi.BaseAddress + _advertApi.CreateUrl;
            Uri _uri = new Uri(_createEndPoint);

            string jsonModel = JsonConvert.SerializeObject(advertModel);

            HttpContent content = new StringContent(jsonModel, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PostAsync(_uri, content);

            string responseJson = await response.Content.ReadAsStringAsync();
            CreateAdvertResponse createAdvertResponse = JsonConvert.DeserializeObject<CreateAdvertResponse>(responseJson);

            return createAdvertResponse;
        }

        public async Task<bool> Confirm(ConfirmAdvertModel confirmAdvertModel)
        {
            string _confirmEndPoint = _advertApi.BaseAddress + _advertApi.ConfirmUrl;
            Uri _uri = new Uri(_confirmEndPoint);

            string jsonModel = JsonConvert.SerializeObject(confirmAdvertModel);

            HttpContent httpContent = new StringContent(jsonModel, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PutAsync(_uri, httpContent);

            return response.StatusCode == System.Net.HttpStatusCode.OK;
        }
    }
}
