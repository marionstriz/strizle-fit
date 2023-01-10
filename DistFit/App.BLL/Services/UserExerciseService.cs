using App.BLL.DTO;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using Base.BLL;
using Base.Contracts.Base;

namespace App.BLL.Services;

public class UserExerciseService 
    : BaseEntityService<App.BLL.DTO.UserExercise, App.DAL.DTO.UserExercise, IUserExerciseRepository>, IUserExerciseService
{
    public UserExerciseService(
        IUserExerciseRepository repository, 
        IMapper<App.BLL.DTO.UserExercise, DAL.DTO.UserExercise> mapper
    ) : base(repository, mapper)
    {
    }

    public async Task<IEnumerable<UserExercise>> GetAllAsync(Guid userId, bool noTracking)
    {
        return (await Repository.GetAllAsync(userId, noTracking)).Select(x => Mapper.Map(x)!);
    }
}