using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyProject.Models
{
    [Table("Department")]
    public class Department

    {
        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.None)] // set identity menjadi OFF
        public int Id { get; set; }
        public string Name { get; set; }


    }
}
