using BOL.Login;
using BOL.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Login
{
    public interface ILogin
    {
        Task<string?> Login(LoginInputDTO loginInput);
        Task<UserOutputDTO?> Register(UserInputDTO userInput);
    }
}
