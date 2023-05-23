//using Castle.Components.DictionaryAdapter;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyProject.Models
{
    [Table("Account")]
    public class Account
    {

        [Key,ForeignKey("Employee")]
        public string NIK { get; set; }
        public string Password { get; set; }
        public Employee Employee { get; set; }
    }
}
