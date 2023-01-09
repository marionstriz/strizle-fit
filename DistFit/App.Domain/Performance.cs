using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.Domain;

public class Performance : DomainEntityMetaId
{
    [Display(ResourceType = typeof(App.Resources.App.Domain.Entities), Name = nameof(PerformedAt))]
    public DateTime PerformedAt { get; set; }
    
    [Display(ResourceType = typeof(App.Resources.App.Domain.Entities), Name = nameof(ExerciseId))]
    public Guid ExerciseId { get; set; }
    [Display(ResourceType = typeof(App.Resources.App.Domain.Entities), Name = nameof(Exercise))]
    public UserExercise? Exercise { get; set; }
    
    public ICollection<SetEntry>? SetEntries { get; set; }
}