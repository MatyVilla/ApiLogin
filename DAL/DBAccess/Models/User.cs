using System;
using System.Collections.Generic;

namespace DAL.DBAccess.Models;

public partial class User
{
    public int IdUser { get; set; }

    public string? Rut { get; set; }

    public string? Name { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public int RoleId { get; set; }

    public string? Phone { get; set; }

    public virtual Role Role { get; set; } = null!;
}
