using Base.Domain;

namespace App.DAL.DTO;

public class MeasurementType : DomainEntityId
{
    public LangStr Name { get; set; } = default!;
    
    public ICollection<DAL.DTO.Goal>? Goals { get; set; }
    public ICollection<DAL.DTO.Measurement>? Measurements { get; set; }
}