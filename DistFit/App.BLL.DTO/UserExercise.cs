using Base.Domain;

namespace App.BLL.DTO;

public class UserExercise : DomainEntityId
{
    public Guid ExerciseTypeId { get; set; }
    public App.BLL.DTO.ExerciseType? ExerciseType { get; set; }

    public Guid AppUserId { get; set; }
    public App.BLL.DTO.Identity.AppUser? AppUser { get; set; }
    
    public ICollection<UserSessionExercise>? UserSessionExercises { get; set; }
}