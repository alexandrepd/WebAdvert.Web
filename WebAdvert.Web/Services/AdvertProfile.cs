using AutoMapper;
using WebAdvert.Api.Models;
using WebAdvert.Web.Models;
using WebAdvert.Web.Models.AdvertManagement;
using WebAdvert.Web.Models.Home;

namespace WebAdvert.Web.Services
{
    public class AdvertProfile : Profile
    {
        public AdvertProfile()
        {
            CreateMap<CreateAdvertNewModel, AdvertModel >();
            CreateMap<AdvertType, SearchViewModel>();
            //CreateMap<>(CreateAdvertResponse, AdvertResponse);
        }
    }
}
