using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.DBAccess.Models;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace BLL.Interface.IMP
{
    public class UserIMP : IUser
    {
        readonly DbaccessContext _dbaccessContext;
        public UserIMP(DbaccessContext dbaccessContext) { _dbaccessContext = dbaccessContext; }

        public async Task<List<User>?> GetAllUser()
        {
            try
            {
                List<User>? list = await _dbaccessContext.Users.ToListAsync();
                return list;
            }
            catch (Exception e)
            {
                Log.Error($"Error GetAllUser[02]: {e}");
                throw new Exception(e.Message);
            }
        }

        public Task<User?> GetUserById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<User> UpdateUserById(int id)
        {
            throw new NotImplementedException();
        }
        public Task<bool> DeleteUserById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
