﻿using AutoMapper;
using CoreCodeCamp.Models;

namespace CoreCodeCamp.Data
{
  public class CampProfile : Profile
  {
    public CampProfile()
    {
      this.CreateMap<Camp, CampModel>()
        .ForMember(c => c.VenueName, o => o.MapFrom(l => l.Location.VenueName))
        .ReverseMap();
    }
  }
}