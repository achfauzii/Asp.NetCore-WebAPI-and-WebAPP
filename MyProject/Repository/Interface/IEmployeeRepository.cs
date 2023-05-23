using MyProject.Models;
using MyProject.ViewModels;
using System.Collections.Generic;

namespace MyProject.Repository.Interface
{
    public interface IEmployeeRepository
    {
        //Get all data tabel Employee
        IEnumerable<Employee> Get();
       

        //Get data Employee by NIK
        Employee Get(string NIK);
   
        int Register (RegisterVM registerVM);
        int Insert (Employee employee);
        int Update (Employee employee);
        int Delete (string NIK);

      

       

    }
}
