using System.ComponentModel.DataAnnotations;
using Base.Domain.Identity;

namespace App.Domain.Identity;

public class AppUser : BaseUser
{
    [MaxLength(254)]
    public string? FirstName { get; set; }
    [MaxLength(254)]
    public string? LastName { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    
    /*
    public ICollection<UserExercise>? Exercises { get; set; }
    public ICollection<Goal>? Goals { get; set; }
    public ICollection<Measurement>? Measurements { get; set; }
    public ICollection<LeaderboardEntry>? LeaderboardEntries { get; set; }
    public ICollection<ProgramSaved>? ProgramSaves { get; set; }
    
    public ICollection<RefreshToken>? RefreshTokens { get; set; }
    */
}