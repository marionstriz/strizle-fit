using Base.Domain;

namespace App.BLL.DTO;

public class Unit : DomainEntityId
{
    public LangStr Name { get; set; } = default!;
    public LangStr Symbol { get; set; } = default!;
    
    public ICollection<BLL.DTO.SetEntry>? QuantitySets { get; set; }
    public ICollection<BLL.DTO.SetEntry>? WeightSets { get; set; }
    public ICollection<BLL.DTO.Goal>? Goals { get; set; }
    public ICollection<BLL.DTO.Measurement>? Measurements { get; set; }
    public ICollection<BLL.DTO.Program>? Programs { get; set; }
}