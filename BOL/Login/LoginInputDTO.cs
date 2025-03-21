using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOL.Login
{
    public class LoginInputDTO
    {
        public required string Rut {  get; set; }
        public required string Password { get; set; }
    }
}
