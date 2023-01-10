using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.Public.DTO.v1;

public class UserSessionExercise : DomainEntityMetaId
{
    [Display(ResourceType = typeof(App.Resources.App.Domain.Entities), Name = nameof(UserExerciseId))]
    public Guid UserExerciseId { get; set; }
    [Display(ResourceType = typeof(App.Resources.App.Domain.Entities), Name = nameof(UserExercise))]
    public UserExercise? UserExercise { get; set; }
    
    [Display(ResourceType = typeof(App.Resources.App.Domain.Entities), Name = nameof(SessionExerciseId))]
    public Guid SessionExerciseId { get; set; }
    [Display(ResourceType = typeof(App.Resources.App.Domain.Entities), Name = nameof(SessionExercise))]
    public SessionExercise? SessionExercise { get; set; }
}