using Microsoft.EntityFrameworkCore;
using MyProject.Context;
using MyProject.Models;
using MyProject.Repository.Interface;
using MyProject.ViewModels;

namespace MyProject.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly MyContext myContext;
        public EmployeeRepository(MyContext context)
        {
            this.myContext = context;
        }

        public IEnumerable<Employee> Get() //GET Employye dan GET Department yang berelasi dengan departmentId
        {
            //return myContext.Employees.ToList();
            //Selain menggunakan Lazy Loading dapat menggunakan seperti ini
            var employees = myContext.Employees
                          .Include(e => e.Department)
                          .ToList();

            return employees;
        }

        //Check ID OR NIK
        public bool IsIdExists(string NIK)
        {
            //Any digunakan untuk memeriksa apakah setidaknya satu elemen dalam kumpulan memenuhi kondisi tertentu.
            //Metode ini mengembalikan nilai boolean, yaitu true
            return myContext.Employees.Any(e => e.NIK == NIK);
        }
        //Check Email duplicate
        public bool IsEmaillExists(string email)
        {
            //Any digunakan untuk memeriksa apakah setidaknya satu elemen dalam kumpulan memenuhi kondisi tertentu.
            return myContext.Employees.Any(e => e.Email == email);
        }
        //Check Phone
        public bool IsPhonelExists(string phone)
        {
            //Any digunakan untuk memeriksa apakah setidaknya satu elemen dalam kumpulan memenuhi kondisi tertentu.
            //Metode ini mengembalikan nilai boolean, yaitu true
            return myContext.Employees.Any(e => e.Phone == phone);
        }

        public bool IsDepIdEmployeeExists(int Id)
        {
            //Any digunakan untuk memeriksa apakah setidaknya satu elemen dalam kumpulan memenuhi kondisi tertentu.
            //Metode ini mengembalikan nilai boolean, yaitu true
            var employees = myContext.Employees
                         .Include(e => e.Department.Id == Id)
                         .ToList();

            return true;
        }

        //Auto Generate ID ddmmyyy + 000
        public string GenerateId()
        {
            var currentDate = DateTime.Now.ToString("ddMMyyy");
            int countEmployee = myContext.Employees.Count();
            /* var lastEmployee = myContext.Employees
                 .OrderByDescending(e => e.NIK)
                 .FirstOrDefault();*/

            if (countEmployee == 0)
            {
                // Jika belum ada data sama sekali, maka ID dimulai dari 0
                return DateTime.Now.ToString("ddMMyyyy") + "000";
            }

            return $"{currentDate}{countEmployee.ToString("D3")}";

            /*var lastNIK = lastEmployee.NIK;
            var lastNikConvert = Convert.ToInt32(lastNIK.Substring(8)); //convert Nik to int dan ambil substring dimulai dari 8
            return $"{currentDate}{(lastNikConvert + 1).ToString("D3")}";*/
        }



        public Employee Get(string NIK)
        {

            return myContext.Employees.Include(e => e.Department).SingleOrDefault(e => e.NIK == NIK);

            //return myContext.Employees.FirstOrDefault(e => e.NIK == NIK);
            //return myContext.Employees.Where(e => e.NIK == NIK).FirstOrDefault();
            //return myContext.Employees.Where(e => e.NIK == NIK).SingleOrDefault();
            //Metode Find hanya digunakan untuk mengambil entitas berdasarkan primary key. 
            //Metode FirstOrDefault dan SingleOrDefault digunakan untuk mengambil elemen-elemen dari kumpulan data yang memenuhi kondisi tertentu.
            //Metode Where digunakan untuk menentukan kondisi yang harus dipenuhi oleh elemen - elemen dalam kumpulan data sebelum diproses oleh metode lain.
        }

        public int Insert(Employee employee)
        {
            /*employee.NIK = GenerateId();
            myContext.Employees.Add(employee);
            var save = myContext.SaveChanges();
            return save;*/
            employee.NIK = GenerateId();
            employee.FirstName = employee.FirstName;
            employee.LastName = employee.LastName;
            myContext.Entry(employee).State = EntityState.Added;
            var save = myContext.SaveChanges();
            return save;
        }

        public int Register(RegisterVM registerVM)
        {
            var NIK = GenerateId();
            Employee employee = new Employee
            {
                NIK = NIK,
                FirstName = registerVM.FirstName,
                LastName = registerVM.LastName,
                Phone = registerVM.Phone,
                BirthDate = registerVM.BirthDate,
                Salary = registerVM.Salary,
                Email = registerVM.Email,
                Gender = registerVM.Gender,
                Department = new Department { Id = registerVM.DepartmentId }
            };
            myContext.Entry(employee).State = EntityState.Added;

            Account account = new Account
            {
                NIK = NIK,
                Password = BCrypt.Net.BCrypt.HashPassword(registerVM.Password)
                
            };
            myContext.Accounts.Add(account);

            // myContext.Entry(employee).State = EntityState.Added;
            var save = myContext.SaveChanges();
            return save;
        }

        public int Update(Employee employee)
        {
            //CARA 1
            /*    var emp = myContext.Employees.Find(employee.NIK);

                emp.NIK = employee.NIK;
                emp.FirstName = employee.FirstName;
                emp.LastName = employee.LastName;
                emp.Phone = employee.Phone;
                emp.BirthDate = employee.BirthDate;
                emp.Email = employee.Email;
                emp.Salary = employee.Salary;
                emp.Gender = employee.Gender;*/
            /*  emp.Department.Id = employee.Department.Id;*/
            /*
                        var update = myContext.SaveChanges();
                        return update;*/

            /*  var emp = myContext.Employees.Find(employee.NIK);
              myContext.Entry(emp).CurrentValues.SetValues(employee);
              //myContext.Entry(emp).State = EntityState.Modified;
              var save = myContext.SaveChanges();
              return save;*/

            //CARA 3
            myContext.Attach(employee);
            myContext.Entry(employee).State = EntityState.Unchanged;
            var emp = myContext.Employees.Find(employee.NIK);
            myContext.Entry(emp).State = EntityState.Modified;

            var save = myContext.SaveChanges();
            return save;
        }

        public int Delete(string NIK)
        {
            var emp = myContext.Employees.Where(e => e.NIK == NIK).SingleOrDefault();
            //myContext.Remove(emp); CARA 1
            myContext.Entry(emp).State = EntityState.Deleted;
            var delete = myContext.SaveChanges();
            return delete;
        }



        //NIK,FullName,DepartmentName
        public IEnumerable<object> GetEmployeesWithDepartments()
        {
            var result = myContext.Employees.Include(e => e.Department)
                         .Select(e => new
                         {
                             NIK = e.NIK,
                             FullName = e.FirstName + " " + e.LastName,
                             DepartmentName = e.Department.Name
                         }).ToList();
            return result;
            //Atau dapat menggunakan seperti dibawah
            /*var query = from employee in myContext.Employees
                        join department in myContext.Departments on employee.Department.Id equals department.Id
                        select new
                        {
                            NIK = employee.NIK,
                            FullName = employee.FirstName + " " + employee.LastName,
                            DepartmentName = department.Name
                        };

            return query.ToList();*/
        }





    }
}


