using MyProject.Models;

namespace MyProject.ViewModels
{
    public class RegisterVM
    {
       // public string NIK { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public DateTimeOffset BirthDate { get; set; }
        public int Salary { get; set; }
        public string Email { get; set; }
        public Gender Gender { get; set; }

      
        public  int DepartmentId{ get; set; }
     

        public string Password { get; set; }
    }
}
