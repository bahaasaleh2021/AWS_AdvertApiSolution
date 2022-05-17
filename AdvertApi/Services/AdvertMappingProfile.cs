using AdvertApi.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdvertApi.Services
{
    public class AdvertMappingProfile:Profile
    {
        public AdvertMappingProfile()
        {
            CreateMap<AdvertModel, AdvertDBModel>()
                .ForMember(dest=>dest.CreationDateTime,x=>x.MapFrom(src=>DateTime.Now))
                .ForMember(dest=>dest.Id,x=>x.MapFrom(src=>Guid.NewGuid().ToString()))
                .ForMember(dest=>dest.Status,x=>x.MapFrom(src=>AdvertStatus.Pending)).ReverseMap();
        }
    }
}
