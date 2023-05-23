using Microsoft.EntityFrameworkCore;
using MyProject.Context;
using MyProject.Models;
using MyProject.Repository.Interface;

namespace MyProject.Repository
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly MyContext myContext;
        public DepartmentRepository(MyContext context)
        {
            myContext = context;
        }
        //Department'
        public bool IsDepIdExists(int Id)
        {
            //Any digunakan untuk memeriksa apakah setidaknya satu elemen dalam kumpulan memenuhi kondisi tertentu.
            //Metode ini mengembalikan nilai boolean, yaitu true
            return myContext.Departments.Any(e => e.Id == Id);
        }
        public IEnumerable<Department> Get_Department()
        {
            return myContext.Departments.ToList();
        }

        public Department Get_DepartmentId(int Id)
        {
            return myContext.Departments.Find(Id);
        }

        public int Insert(Department department)
        {

            myContext.Departments.Add(department);
            var save = myContext.SaveChanges();
            return save;
        }

        public int Update(Department department)
        {
            var empid = myContext.Departments.Find(department.Id);
            myContext.Entry(empid).CurrentValues.SetValues(department);
            var save = myContext.SaveChanges();
            return save;
        }

        public int Delete(int Id)
        {
            /*var employeesToUpdate = myContext.Employees.Where(e => e.Department.Id == Id);
            foreach (var employee in employeesToUpdate)
            {
                
                employee.Department= null;
            }*/
     
            var dept = myContext.Departments.Where(d => d.Id == Id).SingleOrDefault();
            //myContext.Remove(emp); CARA 1
            myContext.Entry(dept).State = EntityState.Deleted;
            var delete = myContext.SaveChanges();
            return delete;
        }
    }
}
