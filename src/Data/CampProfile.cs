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
        .ReverseMap()
        .ForMember(t => t.Camp, opt => opt.Ignore())
        .ForMember(t => t.Speaker, opt => opt.Ignore());

      CreateMap<Speaker, SpeakerModel>()
        .ReverseMap();
    }
  }
}