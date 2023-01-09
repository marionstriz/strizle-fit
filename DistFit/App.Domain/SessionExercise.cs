using Base.Domain;

namespace App.Domain;

public class SessionExercise : DomainEntityMetaId
{
    public int? Sets { get; set; }
    public int? Reps { get; set; }
    
    public Guid ExerciseTypeId { get; set; }
    public ExerciseType? ExerciseType { get; set; }
    
    public Guid SessionId { get; set; }
    public Session? Session { get; set; }
    
    public ICollection<UserSessionExercise>? UserSessionExercises { get; set; }
}