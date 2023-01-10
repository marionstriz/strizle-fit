using AutoMapper;
using Base.DAL;

namespace App.BLL.Mappers;

public class SessionMapper : BaseMapper<App.BLL.DTO.Session, App.DAL.DTO.Session>
{
    public SessionMapper(IMapper mapper) : base(mapper)
    {
    }
}