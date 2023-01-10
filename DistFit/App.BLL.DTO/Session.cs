using Base.Domain;

namespace App.BLL.DTO;

public class Session : DomainEntityId
{
    public int Week { get; set; }
    public int Day { get; set; }
    
    public Guid ProgramId { get; set; }
    public App.BLL.DTO.Program? Program { get; set; }
    
    public ICollection<App.BLL.DTO.SessionExercise>? SessionExercises { get; set; }
}