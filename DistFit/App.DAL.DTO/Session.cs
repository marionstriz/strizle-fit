using Base.Domain;

namespace App.DAL.DTO;

public class Session : DomainEntityId
{
    public int Week { get; set; }
    public int Day { get; set; }
    
    public Guid ProgramId { get; set; }
    public DAL.DTO.Program? Program { get; set; }
    
    public ICollection<App.DAL.DTO.SessionExercise>? SessionExercises { get; set; }
}