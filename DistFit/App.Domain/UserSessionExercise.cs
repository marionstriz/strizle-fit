using Base.Domain;

namespace App.Domain;

public class UserSessionExercise : DomainEntityMetaId
{
    public Guid UserExerciseId { get; set; }
    public UserExercise? UserExercise { get; set; }
    
    public Guid SessionExerciseId { get; set; }
    public SessionExercise? SessionExercise { get; set; }
}