using Base.Domain;

namespace App.Domain;

public class Session : DomainEntityMetaId
{
    public int Week { get; set; }
    public int Day { get; set; }
    
    public Guid ProgramId { get; set; }
    public Program? Program { get; set; }
    
    public ICollection<SessionExercise>? SessionExercises { get; set; }
}