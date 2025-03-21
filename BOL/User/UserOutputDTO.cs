using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOL.User
{
    public class UserOutputDTO
    {
        public int? IdUser { get; set; }
        public string? Rut { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public int? IdRole { get; set; }
        public string? Role { get; set; }
        public string? Status { get; set; }
        public string? Message { get; set; }
    }
}
