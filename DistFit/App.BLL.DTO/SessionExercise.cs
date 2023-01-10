using Base.Domain;

namespace App.BLL.DTO;

public class SessionExercise : DomainEntityId
{
    public int? Sets { get; set; }
    public int? Reps { get; set; }
    
    public Guid ExerciseTypeId { get; set; }
    public BLL.DTO.ExerciseType? ExerciseType { get; set; }
    
    public Guid SessionId { get; set; }
    public BLL.DTO.Session? Session { get; set; }
    
    public ICollection<UserSessionExercise>? UserSessionExercises { get; set; }
}