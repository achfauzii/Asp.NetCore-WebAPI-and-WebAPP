using Microsoft.EntityFrameworkCore;
using MyProject.Context;
using MyProject.Models;
using MyProject.Repository.Interface;
using MyProject.ViewModels;

namespace MyProject.Repository
{
    public class AccountRepository : IAccountRepository

    {
        private readonly MyContext myContext;
        public AccountRepository(MyContext context)
        {
            myContext = context;
        }
        public bool IsIdExists(string NIK)
        {
            //Any digunakan untuk memeriksa apakah setidaknya satu elemen dalam kumpulan memenuhi kondisi tertentu.
            //Metode ini mengembalikan nilai boolean, yaitu true
            return myContext.Accounts.Any(e => e.NIK == NIK);
        }
        public IEnumerable<Account> Get()
        {
            var employees = myContext.Accounts
                          .Include(e => e.Employee)
                          .Include(e => e.Employee.Department)
                          .ToList();

            return employees;
        }

        public int Insert(Account account)
        {
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(account.Password);
            account.NIK = account.NIK;
            account.Password = passwordHash;
            myContext.Entry(account).State = EntityState.Added;
            var save = myContext.SaveChanges();
            return save;
        }
     /*   public string Verify (string password)
        {
            var user = myContext.Accounts
               .Include(e => e.Employee)
               .SingleOrDefault(e => e.Employee.Email == email);
        }*/
        public int Login(LoginVM loginVM)
        {
         
            //string passwordHash = BCrypt.Net.BCrypt.HashPassword(Pass);
            var user = myContext.Accounts
                .Include(e => e.Employee)
                .SingleOrDefault(e => e.Employee.Email == loginVM.email);
            /* var login = myContext.Accounts
                           .Include(e => e.Employee)
                           .Where(e => e.Employee.Email == emai && e.Password == loginVM.Password).SingleOrDefault();*/
            var verify = 0 ;
            if (user == null ){
               
                return verify ;
            }else if (!BCrypt.Net.BCrypt.Verify(loginVM.password, user.Password))
            {
                return verify = 1;
            }
       
            return verify = 2;

        }

        //REPO LOGIN BOOLEAN
   /*     public bool LoginBool (LoginVM loginVM)
        {
            var user = myContext.Accounts
               .Include(e => e.Employee)
               .SingleOrDefault(e => e.Employee.Email == loginVM.email);
            //bool password;
            //bool email;
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginVM.password, user.Password))
            {

                return false;
            }
            return true;
        }*/


        public int Update(Account Account)
        {
            throw new NotImplementedException();
        }
    }
}
