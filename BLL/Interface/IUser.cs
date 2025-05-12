using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.DBAccess.Models;

namespace BLL.Interface
{
    public interface IUser
    {
        Task<List<User>?> GetAllUser();
        Task<User?> GetUserById(int id);
        Task<User> UpdateUserById(int id);
        Task<bool> DeleteUserById(int id);
    }
}
