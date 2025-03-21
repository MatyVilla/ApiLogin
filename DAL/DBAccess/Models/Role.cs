using System;
using System.Collections.Generic;

namespace DAL.DBAccess.Models;

public partial class Role
{
    public int IdRole { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
