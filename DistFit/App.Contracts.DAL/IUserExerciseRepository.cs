using Base.Contracts.DAL;

namespace App.Contracts.DAL;

public interface IUserExerciseRepository : IOwnedEntityRepository<App.DAL.DTO.UserExercise>, IEntityRepository<App.DAL.DTO.UserExercise>
{
}