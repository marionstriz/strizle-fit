using Base.Domain;

namespace App.DAL.DTO;

public class Unit : DomainEntityId
{
    public LangStr Name { get; set; } = default!;
    public LangStr Symbol { get; set; } = default!;
    
    public ICollection<DAL.DTO.SetEntry>? QuantitySets { get; set; }
    public ICollection<DAL.DTO.SetEntry>? WeightSets { get; set; }
    public ICollection<DAL.DTO.Goal>? Goals { get; set; }
    public ICollection<DAL.DTO.Measurement>? Measurements { get; set; }
    public ICollection<DAL.DTO.Program>? Programs { get; set; }
}