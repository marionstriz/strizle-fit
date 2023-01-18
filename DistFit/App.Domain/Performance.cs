using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.Domain;

public class Performance : DomainEntityMetaId
{
    [Display(ResourceType = typeof(App.Resources.App.Domain.Entities), Name = nameof(PerformedAt))]
    public DateTime PerformedAt { get; set; }
    
    [Display(ResourceType = typeof(App.Resources.App.Domain.Entities), Name = "ExerciseId")]
    public Guid UserExerciseId { get; set; }
    [Display(ResourceType = typeof(App.Resources.App.Domain.Entities), Name = "Exercise")]
    public UserExercise? UserExercise { get; set; }
    
    public ICollection<SetEntry>? SetEntries { get; set; }
}