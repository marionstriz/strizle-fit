using Base.Domain;

namespace App.DAL.DTO;

public class Performance : DomainEntityId
{
    public DateTime PerformedAt { get; set; }
    
    public Guid ExerciseId { get; set; }
    public App.DAL.DTO.UserExercise? Exercise { get; set; }
    
    public ICollection<App.DAL.DTO.SetEntry>? SetEntries { get; set; }
}