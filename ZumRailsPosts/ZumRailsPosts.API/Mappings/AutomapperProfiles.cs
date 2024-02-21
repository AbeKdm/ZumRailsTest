using AutoMapper;

namespace ZumRailsPosts.API.Mappings
{
    public class AutomapperProfiles : Profile
    {
        public AutomapperProfiles()
        {
            CreateMap<Models.DTO.Post, Core.Infrastructure.Models.Post>().ReverseMap();
        }
    }
}
