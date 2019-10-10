using AutoMapper;
using Server_WEB_Programming.Lab2.Dal.Entities;

namespace Server_WEB_Programming.Lab2.Infrastructure.MappingProfiles
{
    public class BookMappingProfile : Profile
    {
        public BookMappingProfile()
        {
            CreateMap<Book, Book>()
                .ForMember(x => x.IdBook, o => o.Ignore());
        }
    }
}