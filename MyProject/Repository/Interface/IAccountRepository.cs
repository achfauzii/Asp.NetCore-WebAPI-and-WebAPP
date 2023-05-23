using MyProject.Models;
using MyProject.ViewModels;

namespace MyProject.Repository.Interface
{
    public interface IAccountRepository
    {
        IEnumerable<Account> Get();
 
        int Insert(Account Account);
        int Update(Account Account);

        int Login(LoginVM loginVM);

        //bool LoginBool (LoginVM loginVM);

        /*int Delete(int Id);*/
    }
}
