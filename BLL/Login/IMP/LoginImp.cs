using BOL.Login;
using BOL.User;
using DAL.DBAccess.Models;
using Helper.Custom;
using Microsoft.EntityFrameworkCore;
using Serilog;
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
                                                              .Include(r => r.Role)
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
                Log.Error($"Error Login[02]: {e}");
                throw new Exception(e.Message);
            }
        }

        public async Task<UserOutputDTO?> Register(UserInputDTO userInput)
        {
            try
            {
                //Validar si el usuario ya se encuentra registrado en db
                User? user = await _dbaccessContext.Users.Where(u => u.Rut == userInput.Rut)
                                                              .Include(u => u.Role)
                                                              .FirstOrDefaultAsync();
                UserOutputDTO userOutput = new();
                if (user != null)
                {
                    userOutput = MapingUserRegister(user, "Error", "Usuario ya se encuentra registrado");
                    return userOutput;
                }
                //Crear entidad para ser guardada en db
                User createUser = new()
                {
                    Email = userInput.Email,
                    Name = userInput.Name,
                    Password = _utilities.encryptedSHA256(userInput.Password),
                    Rut = userInput.Rut,
                    Phone = userInput.Phone,
                    RoleId = userInput.IdRole
                };
                //Crear usuario en db
                await _dbaccessContext.Users.AddAsync(createUser);
                int result = await _dbaccessContext.SaveChangesAsync();
                //Validamos si se pudo guardar el registro
                if (result == 0)
                {
                    userOutput = MapingUserRegister(createUser, "Error", "Error al crear el usuario");
                    return userOutput;
                }
                //Validamos que el usuario exista en bd
                user = await _dbaccessContext.Users.Where(u => u.Rut == userInput.Rut)
                                                              .Include(u => u.Role)
                                                              .FirstOrDefaultAsync();

                userOutput = MapingUserRegister(user, "OK", "Usuario registrado exitosamente");

                return userOutput;
            }
            catch (Exception e)
            {
                Log.Error($"Error register[02]: {e}");
                throw new Exception(e.Message);
            }
        }
        private static UserOutputDTO MapingUserRegister(User user, string status, string message)
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
