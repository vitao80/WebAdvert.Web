using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdvertApiAppPro.Models;
using AutoMapper;
using WebAdvert.Web.Models;

namespace WebAdvert.Web.ServiceClients
{
    public class AdvertApiProfile: Profile
    {
        public AdvertApiProfile()
        {
            CreateMap<AdvertModel, CreateAdvertModel>().ReverseMap();
            CreateMap<CreateAdvertResponse, AdvertResponse>().ReverseMap();
            CreateMap<ConfirmAdvertRequest, ConfirmAdvertModel>().ReverseMap();

            CreateMap<AdvertType, Models.Home.SearchViewModel>()
                .ForMember(dest => dest.Title, src => src.MapFrom(field => field.Title))
                .ForMember(dest => dest.Id, src => src.MapFrom(field => field.Id));
        }
    }
}
