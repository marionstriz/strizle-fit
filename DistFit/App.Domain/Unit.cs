using System.ComponentModel.DataAnnotations.Schema;
using Base.Domain;

namespace App.Domain;

public class Unit : DomainEntityId
{
    [Column(TypeName = "jsonb")]
    public LangStr Name { get; set; } = default!;
    [Column(TypeName = "jsonb")]
    public LangStr Symbol { get; set; } = default!;
    
    [InverseProperty("QuantityUnit")]
    public ICollection<SetEntry>? QuantitySets { get; set; }
    [InverseProperty("WeightUnit")]
    public ICollection<SetEntry>? WeightSets { get; set; }
    [InverseProperty("ValueUnit")]
    public ICollection<Goal>? Goals { get; set; }
    [InverseProperty("ValueUnit")]
    public ICollection<Measurement>? Measurements { get; set; }
    [InverseProperty("DurationUnit")]
    public ICollection<Program>? Programs { get; set; }
}