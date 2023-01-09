using Base.Domain;

namespace App.Domain;

public class Performance : DomainEntityMetaId
{
    public DateTime PerformedAt { get; set; }
    
    public Guid ExerciseId { get; set; }
    public UserExercise? Exercise { get; set; }
    
    public ICollection<SetEntry>? SetEntries { get; set; }
}