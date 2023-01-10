using System.ComponentModel.DataAnnotations;
using App.Public.DTO.v1.Identity;
using Base.Domain;

namespace App.Public.DTO.v1;

public class ProgramSaved : DomainEntityId
{
    [Display(ResourceType = typeof(Base.Resources.Common), Name = nameof(AppUserId))]
    public Guid? AppUserId { get; set; }
    [Display(ResourceType = typeof(Base.Resources.Common), Name = nameof(AppUser))]
    public AppUser? AppUser { get; set; }
    [Display(ResourceType = typeof(App.Resources.App.Domain.Entities), Name = nameof(IsCreator))]
    public bool IsCreator { get; set; }
    
    
    [Display(ResourceType = typeof(App.Resources.App.Domain.Entities), Name = nameof(ProgramId))]
    public Guid ProgramId { get; set; }
    [Display(ResourceType = typeof(App.Resources.App.Domain.Entities), Name = nameof(Program))]
    public Program? Program { get; set; }
    
    [Display(ResourceType = typeof(Base.Resources.Common), Name = nameof(StartedAt))]
    public DateTime? StartedAt { get; set; }
    [Display(ResourceType = typeof(Base.Resources.Common), Name = nameof(FinishedAt))]
    public DateTime? FinishedAt { get; set; }
}