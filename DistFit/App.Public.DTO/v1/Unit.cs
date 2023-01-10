using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.Public.DTO.v1;

public class Unit : DomainEntityId
{
    [Display(ResourceType = typeof(Base.Resources.Common), Name = nameof(Name))]
    public string Name { get; set; } = default!;
    [Display(ResourceType = typeof(Base.Resources.Common), Name = nameof(Symbol))]
    public string Symbol { get; set; } = default!;
}