using Base.Domain;

namespace App.DAL.DTO;

public class ProgramSaved : DomainEntityId
{
    public Guid? AppUserId { get; set; }
    public App.DAL.DTO.Identity.AppUser? AppUser { get; set; }
    public bool IsCreator { get; set; }
    
    public Guid ProgramId { get; set; }
    public DAL.DTO.Program? Program { get; set; }
    
    public DateTime? StartedAt { get; set; }
    public DateTime? FinishedAt { get; set; }
}