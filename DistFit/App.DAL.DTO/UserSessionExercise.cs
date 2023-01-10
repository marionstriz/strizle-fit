using Base.Domain;

namespace App.DAL.DTO;

public class UserSessionExercise : DomainEntityId
{
    public Guid UserExerciseId { get; set; }
    public App.DAL.DTO.UserExercise? UserExercise { get; set; }
    
    public Guid SessionExerciseId { get; set; }
    public App.DAL.DTO.SessionExercise? SessionExercise { get; set; }
}