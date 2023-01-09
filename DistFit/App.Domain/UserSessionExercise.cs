namespace App.Domain;

public class UserSessionExercise
{
    public Guid UserExerciseId { get; set; }
    public UserExercise? UserExercise { get; set; }
    
    public Guid SessionExerciseId { get; set; }
    public SessionExercise? SessionExercise { get; set; }
}