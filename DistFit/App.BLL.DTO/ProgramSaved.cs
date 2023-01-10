using Base.Domain;

namespace App.BLL.DTO;

public class ProgramSaved : DomainEntityId
{
    public Guid? AppUserId { get; set; }
    public App.BLL.DTO.Identity.AppUser? AppUser { get; set; }
    public bool IsCreator { get; set; }
    
    public Guid ProgramId { get; set; }
    public App.BLL.DTO.Program? Program { get; set; }
    
    public DateTime? StartedAt { get; set; }
    public DateTime? FinishedAt { get; set; }
}