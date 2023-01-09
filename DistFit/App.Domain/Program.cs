using System.ComponentModel.DataAnnotations.Schema;
using Base.Domain;
using Microsoft.EntityFrameworkCore;

namespace App.Domain;

public class Program : DomainEntityMetaId
{
    [Column(TypeName = "jsonb")]
    public LangStr Name { get; set; } = default!;
    [Column(TypeName = "jsonb")]
    public LangStr? Description { get; set; }
    public bool IsPublic { get; set; }
    [Precision(12, 3)]
    public decimal? Duration { get; set; }
    public Guid? DurationUnitId { get; set; }
    public Unit? DurationUnit { get; set; }
    
    public ICollection<ProgramSaved>? ProgramSaves { get; set; }
    public ICollection<Session>? Sessions { get; set; }
}