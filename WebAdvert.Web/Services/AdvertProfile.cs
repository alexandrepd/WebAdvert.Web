using AutoMapper;
using WebAdvert.Api.Models;
using WebAdvert.Web.Models.AdvertManagement;

namespace WebAdvert.Web.Services
{
    public class AdvertProfile : Profile
    {
        public AdvertProfile()
        {
            CreateMap<CreateAdvertNewModel, AdvertModel >();
            //CreateMap<>(CreateAdvertResponse, AdvertResponse);
        }
    }
}
