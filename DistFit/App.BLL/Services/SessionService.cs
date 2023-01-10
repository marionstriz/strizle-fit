using App.BLL.DTO;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using Base.BLL;
using Base.Contracts.Base;

namespace App.BLL.Services;

public class SessionService 
    : BaseEntityService<App.BLL.DTO.Session, App.DAL.DTO.Session, ISessionRepository>, ISessionService
{
    public SessionService(
        ISessionRepository repository, 
        IMapper<Session, DAL.DTO.Session> mapper
    ) : base(repository, mapper)
    {
    }
}