using Newtonsoft.Json;

namespace WebAdvert.Web.Models.AdvertManagement
{
    public class AdvertCardModel
    {
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("id")]
        public string Description { get; set; }
        public string Image { get; set; }
        public string Id { get; set; }
        [JsonProperty("creationDateTime")]
        public DateTime CreationDateTime { get; set; }
    }
}
