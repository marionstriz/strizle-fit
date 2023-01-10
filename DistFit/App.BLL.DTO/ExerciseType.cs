using Base.Domain;

namespace App.BLL.DTO;

public class ExerciseType : DomainEntityId
{
    public LangStr Name { get; set; } = default!;

    public ICollection<App.BLL.DTO.UserExercise>? UserExercises { get; set; }
    public ICollection<App.BLL.DTO.SessionExercise>? SessionExercises { get; set; }
    public ICollection<App.BLL.DTO.Goal>? Goals { get; set; }
}