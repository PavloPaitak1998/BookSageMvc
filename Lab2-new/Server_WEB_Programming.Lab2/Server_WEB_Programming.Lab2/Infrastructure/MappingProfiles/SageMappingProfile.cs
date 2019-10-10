using AutoMapper;
using Server_WEB_Programming.Lab2.Dal.Entities;


namespace Server_WEB_Programming.Lab2.Infrastructure.MappingProfiles
{
    public class SageMappingProfile : Profile
    {
        public SageMappingProfile()
        {
            CreateMap<Sage, Sage>()
                .ForMember(x => x.IdSage, o => o.Ignore());
        }
    }
}