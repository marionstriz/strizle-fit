using Base.Domain;

namespace App.DAL.DTO;

public class ExerciseType : DomainEntityId
{
    public LangStr Name { get; set; } = default!;

    public ICollection<App.DAL.DTO.UserExercise>? UserExercises { get; set; }
    public ICollection<App.DAL.DTO.SessionExercise>? SessionExercises { get; set; }
    public ICollection<App.DAL.DTO.Goal>? Goals { get; set; }
}