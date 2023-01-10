using App.DAL.DTO;
using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class SessionMapper : BaseMapper<Session, Domain.Session>
{
    public SessionMapper(IMapper mapper) : base(mapper)
    {
    }
}