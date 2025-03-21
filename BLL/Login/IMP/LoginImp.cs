using BOL.Login;
using BOL.User;
using DAL.DBAccess.Models;
using Helper.Custom;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Login.IMP
{
    public class LoginImp : ILogin
    {
        private readonly DbaccessContext _dbaccessContext;
        private readonly Utilities _utilities;

        public LoginImp(DbaccessContext dbaccessContext, Utilities utilities)
        {
            _dbaccessContext = dbaccessContext;
            _utilities = utilities;
        }
        public async Task<string?> Login(LoginInputDTO loginInput)
        {
            try
            {
                User? user = await _dbaccessContext.Users.Where(u => u.Rut == loginInput.Rut
                                                              && u.Password == _utilities.encryptedSHA256(loginInput.Password))
                                                              .FirstOrDefaultAsync();
                if (user == null)
                {
                    return null;
                }

                string jwt = _utilities.createJWT(user);

                return jwt;
            }
            catch (Exception e)
            {

                throw;
            }
        }

        public async Task<UserOutputDTO> Register(UserInputDTO userInput)
        {
            try
            {
                User? user = await _dbaccessContext.Users.Where(u => u.Rut == userInput.Rut)
                                                              .Include(u => u.Role)
                                                              .FirstOrDefaultAsync();
                UserOutputDTO userOutput = new UserOutputDTO();
                if (user != null)
                {
                    userOutput = this.MapingUserRegister(user, "Error", "Usuario ya se encuentra registrado");
                    return userOutput;
                }

                User createUser = new User();
                createUser.Email = userInput.Email;
                createUser.Name = userInput.Name;
                createUser.Password = userInput.Password;
                createUser.Rut = userInput.Rut;
                createUser.Phone = userInput.Phone;
                createUser.RoleId = userInput.IdRole;

                await _dbaccessContext.Users.AddAsync(createUser);
                int result = await _dbaccessContext.SaveChangesAsync();

                if (result == 0)
                {
                    userOutput = this.MapingUserRegister(createUser,"Error", "Error al crear el usuario");
                    return userOutput;
                }

                user = await _dbaccessContext.Users.Where(u => u.Rut == userInput.Rut)
                                                              .Include(u => u.Role)
                                                              .FirstOrDefaultAsync();

                userOutput = this.MapingUserRegister(user, "OK", "Usuario registrado exitosamente");

                return userOutput;
            }
            catch (Exception e)
            {

                throw;
            }
        }
        private UserOutputDTO MapingUserRegister(User user,string status, string message)
        {
            UserOutputDTO userOutput = new UserOutputDTO();
            userOutput.Rut = user.Rut;
            userOutput.Phone = user.Phone;
            userOutput.Email = user.Email;
            userOutput.Name = user.Name;
            userOutput.IdRole = user.RoleId;
            if (user.Role != null) userOutput.Role = user.Role.Name;
            userOutput.IdUser = user.IdUser;
            userOutput.Status = status;
            userOutput.Message = message;
            return userOutput;
        }
    }
}
