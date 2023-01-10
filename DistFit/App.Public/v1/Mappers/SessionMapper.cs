using AutoMapper;
using Base.DAL;

namespace App.Public.v1.Mappers;

public class SessionMapper : BaseMapper<App.Public.DTO.v1.Session, App.BLL.DTO.Session>
{
    public SessionMapper(IMapper mapper) : base(mapper)
    {
    }
}