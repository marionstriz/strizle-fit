using Base.Domain;

namespace App.DAL.DTO;

public class UserExercise : DomainEntityId
{
    public Guid ExerciseTypeId { get; set; }
    public App.DAL.DTO.ExerciseType? ExerciseType { get; set; }
    
    public Guid AppUserId { get; set; }
    public App.DAL.DTO.Identity.AppUser? AppUser { get; set; }
    
    public ICollection<UserSessionExercise>? UserSessionExercises { get; set; }
}