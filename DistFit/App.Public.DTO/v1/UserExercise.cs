using System.ComponentModel.DataAnnotations;
using App.Public.DTO.v1.Identity;
using Base.Domain;

namespace App.Public.DTO.v1;

public class UserExercise : DomainEntityId
{
    [Display(ResourceType = typeof(App.Resources.App.Domain.Entities), Name = nameof(ExerciseTypeId))]
    public Guid ExerciseTypeId { get; set; }
    [Display(ResourceType = typeof(App.Resources.App.Domain.Entities), Name = nameof(ExerciseType))]
    public ExerciseType? ExerciseType { get; set; }

    [Display(ResourceType = typeof(Base.Resources.Common), Name = nameof(AppUserId))]
    public Guid AppUserId { get; set; }
    [Display(ResourceType = typeof(Base.Resources.Common), Name = nameof(AppUser))]
    public AppUser? AppUser { get; set; }
}