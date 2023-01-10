using Base.Domain;

namespace App.DAL.DTO;

public class SessionExercise : DomainEntityId
{
    public int? Sets { get; set; }
    public int? Reps { get; set; }
    
    public Guid ExerciseTypeId { get; set; }
    public DAL.DTO.ExerciseType? ExerciseType { get; set; }
    
    public Guid SessionId { get; set; }
    public DAL.DTO.Session? Session { get; set; }
    
    public ICollection<UserSessionExercise>? UserSessionExercises { get; set; }
}