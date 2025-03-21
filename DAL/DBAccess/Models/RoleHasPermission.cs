using System;
using System.Collections.Generic;

namespace DAL.DBAccess.Models;

public partial class RoleHasPermission
{
    public int PermissionId { get; set; }

    public int RoleId { get; set; }

    public virtual Permission Permission { get; set; } = null!;

    public virtual Role Role { get; set; } = null!;
}
