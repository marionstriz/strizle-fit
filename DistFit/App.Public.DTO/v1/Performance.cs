using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.Public.DTO.v1;

public class Performance : DomainEntityId
{
    [Display(ResourceType = typeof(App.Resources.App.Domain.Entities), Name = nameof(PerformedAt))]
    public DateTime PerformedAt { get; set; }
    
    [Display(ResourceType = typeof(App.Resources.App.Domain.Entities), Name = "Exercise")]
    public Guid UserExerciseId { get; set; }
    [Display(ResourceType = typeof(App.Resources.App.Domain.Entities), Name = "ExerciseId")]
    public UserExercise? UserExercise { get; set; }
    
}