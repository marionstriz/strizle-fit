using System.ComponentModel.DataAnnotations;

namespace App.Public.DTO.v1.Identity;

public class Login
{
    [StringLength(256, MinimumLength = 5, ErrorMessage = "Wrong email length")]
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
}