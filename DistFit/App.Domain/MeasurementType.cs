using System.ComponentModel.DataAnnotations.Schema;
using Base.Domain;

namespace App.Domain;

public class MeasurementType : DomainEntityId
{
    [Column(TypeName = "jsonb")]
    public LangStr Name { get; set; } = default!;
    
    public ICollection<Goal>? Goals { get; set; }
    public ICollection<Measurement>? Measurements { get; set; }
}