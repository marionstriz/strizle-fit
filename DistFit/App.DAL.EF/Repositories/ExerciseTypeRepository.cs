using App.Contracts.DAL;
using Base.Contracts.Base;
using Base.DAL.EF;

namespace App.DAL.EF.Repositories;

public class ExerciseTypeRepository : 
    BaseEntityRepository<DTO.ExerciseType, Domain.ExerciseType, AppDbContext>, IExerciseTypeRepository
{
    public ExerciseTypeRepository(
        AppDbContext dbContext, 
        IMapper<DTO.ExerciseType, Domain.ExerciseType> mapper) 
        : base(dbContext, mapper)
    {
    }
}