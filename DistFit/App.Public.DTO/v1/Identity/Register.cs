using System.ComponentModel.DataAnnotations;

namespace App.Public.DTO.v1.Identity;

public class Register
{
    [StringLength(maximumLength: 128, MinimumLength = 5, ErrorMessage = "Wrong email length")]
    public string Email { get; set; } = default!;
    [StringLength(maximumLength: 128, MinimumLength = 1, ErrorMessage = "Wrong password length")]
    public string Password { get; set; } = default!;
}