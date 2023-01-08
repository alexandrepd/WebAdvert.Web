using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Nodes;
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

        public async Task<List<AdvertModel>> GetAll()
        {
            string _confirmEndPoint = _advertApi.BaseAddress + _advertApi.GetAllUrl;
            Uri _uri = new Uri(_confirmEndPoint);

            HttpResponseMessage response = await _httpClient.GetAsync(_uri);

            string responseJson = await response.Content.ReadAsStringAsync();

            List<AdvertModel> advertModels = JsonConvert.DeserializeObject<List<AdvertModel>>(responseJson);


            return advertModels;
        }

        public async Task<bool> Delete(string Id)
        {
            string _url = _advertApi.BaseAddress + _advertApi.DeleteUrl + $"?id={Id}";
            Uri _uri = new Uri(_url);

            HttpContent content = new StringContent("application/json");
            HttpResponseMessage response = await _httpClient.DeleteAsync(_uri);

            return response.StatusCode == System.Net.HttpStatusCode.OK;
        }

        public async Task<bool> Update(AdvertModel advertModel)
        {

            string _confirmEndPoint = _advertApi.BaseAddress + _advertApi.UpdateUrl;
            Uri _uri = new Uri(_confirmEndPoint);

            string jsonModel = JsonConvert.SerializeObject(advertModel);

            HttpContent httpContent = new StringContent(jsonModel, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PutAsync(_uri, httpContent);

            return response.StatusCode == System.Net.HttpStatusCode.OK;

            throw new NotImplementedException();
        }

        public async Task<AdvertModel> Get(string Id)
        {

            string _url = _advertApi.BaseAddress + _advertApi.GetUrl + Id;
            Uri _uri = new Uri(_url);

            HttpContent content = new StringContent("application/json");
            HttpResponseMessage response = await _httpClient.GetAsync(_uri);

            string responseJson = await response.Content.ReadAsStringAsync();
            AdvertModel advertModel = JsonConvert.DeserializeObject<AdvertModel>(responseJson);

            if (advertModel != null)
            {
                return advertModel;
            }
            return null;
        }
    }
}
