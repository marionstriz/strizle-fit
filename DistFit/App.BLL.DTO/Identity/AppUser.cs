using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.BLL.DTO.Identity;

public class AppUser : DomainEntityId
{
    [MaxLength(254)]
    public string Email { get; set; } = default!;

    [MaxLength(254)] public string FirstName { get; set; } = default!;
    [MaxLength(254)] public string LastName { get; set; } = default!;
    
    public ICollection<App.BLL.DTO.UserExercise>? Exercises { get; set; }
    public ICollection<App.BLL.DTO.Goal>? Goals { get; set; }
    public ICollection<App.BLL.DTO.Measurement>? Measurements { get; set; }
    public ICollection<App.BLL.DTO.ProgramSaved>? ProgramSaves { get; set; }
}