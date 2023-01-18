using Base.Domain;

namespace App.BLL.DTO;

public class Performance : DomainEntityId
{
    public DateTime PerformedAt { get; set; }
    
    public Guid UserExerciseId { get; set; }
    
    public App.BLL.DTO.UserExercise? UserExercise { get; set; }
    
    public ICollection<App.BLL.DTO.SetEntry>? SetEntries { get; set; }
}