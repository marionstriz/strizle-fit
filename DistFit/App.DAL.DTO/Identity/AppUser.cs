using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.DAL.DTO.Identity;

public class AppUser : DomainEntityId
{
    [MaxLength(254)]
    public string Email { get; set; } = default!;
    [MaxLength(254)]
    public string? FirstName { get; set; }
    [MaxLength(254)]
    public string? LastName { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    
    public ICollection<App.DAL.DTO.UserExercise>? Exercises { get; set; }
    public ICollection<App.DAL.DTO.Goal>? Goals { get; set; }
    public ICollection<App.DAL.DTO.Measurement>? Measurements { get; set; }
    public ICollection<App.DAL.DTO.ProgramSaved>? ProgramSaves { get; set; }
}