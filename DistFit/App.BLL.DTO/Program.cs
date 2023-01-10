using Base.Domain;
using Microsoft.EntityFrameworkCore;

namespace App.BLL.DTO;

public class Program : DomainEntityId
{
    public LangStr Name { get; set; } = default!;
    public LangStr? Description { get; set; }
    public bool IsPublic { get; set; }
    [Precision(12, 3)]
    public decimal? Duration { get; set; }
    public Guid? DurationUnitId { get; set; }
    public App.BLL.DTO.Unit? DurationUnit { get; set; }
    
    public ICollection<App.BLL.DTO.ProgramSaved>? ProgramSaves { get; set; }
    public ICollection<App.BLL.DTO.Session>? Sessions { get; set; }
}