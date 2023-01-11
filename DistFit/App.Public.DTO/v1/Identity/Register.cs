using System.ComponentModel.DataAnnotations;

namespace App.Public.DTO.v1.Identity;

public class Register
{
    [StringLength(maximumLength: 254, MinimumLength = 5, ErrorMessage = "Wrong email length")]
    public string Email { get; set; } = default!;
    [StringLength(maximumLength: 128, MinimumLength = 1, ErrorMessage = "Wrong password length")]
    public string Password { get; set; } = default!;

    [StringLength(maximumLength: 128, MinimumLength = 1, ErrorMessage = "Wrong first name length")]
    public string FirstName { get; set; } = default!;

    [StringLength(maximumLength: 128, MinimumLength = 1, ErrorMessage = "Wrong last name length")]
    public string LastName { get; set; } = default!;
}