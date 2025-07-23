using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoAPICS.Models
{
    public class UsersDetails : IdentityUser
    {

        // Add any custom properties for your user here if needed
        // public string FullName { get; set; }
    }
}
