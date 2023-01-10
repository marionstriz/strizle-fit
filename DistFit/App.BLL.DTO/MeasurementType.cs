using Base.Domain;

namespace App.BLL.DTO;

public class MeasurementType : DomainEntityId
{
    public LangStr Name { get; set; } = default!;
    
    public ICollection<BLL.DTO.Goal>? Goals { get; set; }
    public ICollection<BLL.DTO.Measurement>? Measurements { get; set; }
}