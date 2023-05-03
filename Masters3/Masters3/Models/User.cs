using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Masters3.Models
{
    public class User: IdentityUser
    {
        public string? RoleId { get; set; }

        public string? Imagepath { get; set; }   
    }
}
