using Base.Domain;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.DTO;

public class Program : DomainEntityId
{
    public LangStr Name { get; set; } = default!;
    public LangStr? Description { get; set; }
    public bool IsPublic { get; set; }
    [Precision(12, 3)]
    public decimal? Duration { get; set; }
    public Guid? DurationUnitId { get; set; }
    public App.DAL.DTO.Unit? DurationUnit { get; set; }
    
    public ICollection<App.DAL.DTO.ProgramSaved>? ProgramSaves { get; set; }
    public ICollection<App.DAL.DTO.Session>? Sessions { get; set; }
}