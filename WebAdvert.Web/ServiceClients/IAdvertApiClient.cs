using WebAdvert.Api.Models;
using WebAdvert.Web.Models.AdvertManagement;

namespace WebAdvert.Web.ServiceClients
{
    public interface IAdvertApiClient
    {
        Task<CreateAdvertResponse> Create(AdvertModel advertModel);
    }
}
