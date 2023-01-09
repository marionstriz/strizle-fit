using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.Domain;

public class SessionExercise : DomainEntityMetaId
{
    [Display(ResourceType = typeof(App.Resources.App.Domain.Entities), Name = nameof(Sets))]
    public int? Sets { get; set; }
    [Display(ResourceType = typeof(App.Resources.App.Domain.Entities), Name = nameof(Reps))]
    public int? Reps { get; set; }
    
    [Display(ResourceType = typeof(App.Resources.App.Domain.Entities), Name = nameof(ExerciseTypeId))]
    public Guid ExerciseTypeId { get; set; }
    [Display(ResourceType = typeof(App.Resources.App.Domain.Entities), Name = nameof(ExerciseType))]
    public ExerciseType? ExerciseType { get; set; }
    
    [Display(ResourceType = typeof(App.Resources.App.Domain.Entities), Name = nameof(SessionId))]
    public Guid SessionId { get; set; }
    [Display(ResourceType = typeof(App.Resources.App.Domain.Entities), Name = nameof(Session))]
    public Session? Session { get; set; }
    
    public ICollection<UserSessionExercise>? UserSessionExercises { get; set; }
}