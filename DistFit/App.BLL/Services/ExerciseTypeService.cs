using App.BLL.DTO;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using Base.BLL;
using Base.Contracts.Base;

namespace App.BLL.Services;

public class ExerciseTypeService 
    : BaseEntityService<App.BLL.DTO.ExerciseType, App.DAL.DTO.ExerciseType, IExerciseTypeRepository>, IExerciseTypeService
{
    public ExerciseTypeService(
        IExerciseTypeRepository repository, 
        IMapper<ExerciseType, DAL.DTO.ExerciseType> mapper
    ) : base(repository, mapper)
    {
    }
}