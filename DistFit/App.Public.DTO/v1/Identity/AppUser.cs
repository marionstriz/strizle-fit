using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.Public.DTO.v1.Identity;

public class AppUser : DomainEntityId
{
    [MaxLength(254)]
    public string Email { get; set; } = default!;
    [MaxLength(254)]
    public string? FirstName { get; set; }
    [MaxLength(254)]
    public string? LastName { get; set; }
    public DateOnly? DateOfBirth { get; set; }
}