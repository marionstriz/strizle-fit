using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using Base.BLL;
using Base.Contracts.Base;

namespace App.BLL.Services;

public class UserSessionExerciseService 
    : BaseEntityService<App.BLL.DTO.UserSessionExercise, App.DAL.DTO.UserSessionExercise, IUserSessionExerciseRepository>, IUserSessionExerciseService
{
    public UserSessionExerciseService(
        IUserSessionExerciseRepository repository, 
        IMapper<App.BLL.DTO.UserSessionExercise, DAL.DTO.UserSessionExercise> mapper
    ) : base(repository, mapper)
    {
    }
}