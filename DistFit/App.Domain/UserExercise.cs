using App.Domain.Identity;
using Base.Domain;

namespace App.Domain;

public class UserExercise : DomainEntityMetaId
{
    public Guid ExerciseTypeId { get; set; }
    public ExerciseType? ExerciseType { get; set; }
    
    public Guid AppUserId { get; set; }
    public AppUser? AppUser { get; set; }
    
    public ICollection<UserSessionExercise>? UserSessionExercises { get; set; }
}