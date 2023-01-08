using WebAdvert.Api.Models;
using WebAdvert.Web.Models.AdvertManagement;

namespace WebAdvert.Web.ServiceClients
{
    public interface IAdvertApiClient
    {
        Task<CreateAdvertResponse> Create(AdvertModel advertModel);
        Task<AdvertModel> Get(string id);
        Task<bool> Confirm(ConfirmAdvertModel confirmAdvertModel);
        Task<List<AdvertModel>> GetAll();
        Task<bool> Delete(string Id);
        Task<bool> Update(AdvertModel advertModel);

    }
}
