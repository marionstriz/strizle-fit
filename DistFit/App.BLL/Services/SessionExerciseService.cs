using App.BLL.DTO;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using Base.BLL;
using Base.Contracts.Base;

namespace App.BLL.Services;

public class SessionExerciseService 
    : BaseEntityService<App.BLL.DTO.SessionExercise, App.DAL.DTO.SessionExercise, ISessionExerciseRepository>, ISessionExerciseService
{
    public SessionExerciseService(
        ISessionExerciseRepository repository, 
        IMapper<SessionExercise, DAL.DTO.SessionExercise> mapper
    ) : base(repository, mapper)
    {
    }
}