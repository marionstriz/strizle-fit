using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.Public.DTO.v1;

public class Session : DomainEntityId
{
    [Display(ResourceType = typeof(Base.Resources.Common), Name = nameof(Week))]
    public int Week { get; set; }
    [Display(ResourceType = typeof(Base.Resources.Common), Name = nameof(Day))]
    public int Day { get; set; }
    
    [Display(ResourceType = typeof(App.Resources.App.Domain.Entities), Name = nameof(ProgramId))]
    public Guid ProgramId { get; set; }
    [Display(ResourceType = typeof(App.Resources.App.Domain.Entities), Name = nameof(Program))]
    public Program? Program { get; set; }
}