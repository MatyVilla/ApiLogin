using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOL.User
{
    public class UserInputDTO
    {
        public required string Name {  get; set; }
        public required string Rut { get; set; }
        public required string Password { get; set; }
        public string ?Email { get; set; }
        public string ?Phone { get; set; }
        public required int IdRole { get; set; }
    }
}
