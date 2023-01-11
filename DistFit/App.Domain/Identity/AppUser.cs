using System.ComponentModel.DataAnnotations;
using Base.Domain.Identity;

namespace App.Domain.Identity;

public class AppUser : BaseUser
{
    [MaxLength(254)]
    [Display(ResourceType = typeof(Base.Resources.Common), Name = nameof(FirstName))]
    public string? FirstName { get; set; }
    [MaxLength(254)]
    [Display(ResourceType = typeof(Base.Resources.Common), Name = nameof(LastName))]
    public string? LastName { get; set; }
    [Display(ResourceType = typeof(Base.Resources.Common), Name = nameof(DateOfBirth))]
    public DateOnly? DateOfBirth { get; set; }
    
    public ICollection<UserExercise>? Exercises { get; set; }
    public ICollection<Goal>? Goals { get; set; }
    public ICollection<Measurement>? Measurements { get; set; }
    public ICollection<ProgramSaved>? ProgramSaves { get; set; }
    
    public ICollection<RefreshToken>? RefreshTokens { get; set; }
}