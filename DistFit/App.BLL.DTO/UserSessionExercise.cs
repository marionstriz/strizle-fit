using Base.Domain;

namespace App.BLL.DTO;

public class UserSessionExercise : DomainEntityId
{
    public Guid UserExerciseId { get; set; }
    public App.BLL.DTO.UserExercise? UserExercise { get; set; }
    
    public Guid SessionExerciseId { get; set; }
    public App.BLL.DTO.SessionExercise? SessionExercise { get; set; }
}