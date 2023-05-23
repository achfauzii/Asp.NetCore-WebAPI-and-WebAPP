using MyProject.Models;

namespace MyProject.Repository.Interface
{
    public interface IDepartmentRepository
    {

        //Department
        IEnumerable<Department> Get_Department();
        Department Get_DepartmentId(int Id);
        int Insert(Department department);
        int Update(Department department);
        int Delete(int Id);
    }
}
