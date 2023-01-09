using App.Domain.Identity;
using Base.Domain;

namespace App.Domain;

public class ProgramSaved : DomainEntityMetaId
{
    public Guid? AppUserId { get; set; }
    public AppUser? AppUser { get; set; }
    public bool IsCreator { get; set; }
    
    public Guid ProgramId { get; set; }
    public Program? Program { get; set; }
    
    public DateTime? StartedAt { get; set; }
    public DateTime? FinishedAt { get; set; }
}