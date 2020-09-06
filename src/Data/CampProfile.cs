using AutoMapper;
using CoreCodeCamp.Models;

namespace CoreCodeCamp.Data
{
  public class CampProfile : Profile
  {
    public CampProfile()
    {
      CreateMap<Camp, CampModel>()
        .ForMember(c => c.VenueName, o => o.MapFrom(l => l.Location.VenueName))
        .ReverseMap();

      CreateMap<Talk, TalkModel>()
        .ReverseMap();

      CreateMap<Speaker, SpeakerModel>()
        .ReverseMap();
    }
  }
}