using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Rocky.Models;

public class AppUserDTO : IdentityUser
{
    [Required]
    public string FullName { get; set; } = null!;
}

