using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Tumanji.Models
{
    [Keyless]
    public class UserEntity
    {
       // private readonly Guid _id;
       //// [Key]
       // public Guid UserID
       // {
       //     get { return _id; }
       // }
        public Guid UserID { get; set; }
        public required string UserName { get; set; } 
        public required string Password { get; set; }
        public DateTime DataCreazione { get; set; }
        public bool Admin {  get; set; }
        public bool SuperAdmin { get; set; }
    }
}
