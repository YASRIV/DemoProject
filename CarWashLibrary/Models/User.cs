using System;
using System.Collections.Generic;

namespace Car_Wash_Library.Models;

public partial class User
{
    public int UserId { get; set; }

    public string UserName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public bool IsActive { get; set; }

    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}

public class FaultContract

{

    public int FaultId { get; set; }

    public string FaultName { get; set; } = string.Empty; //Exception Class name

    public string FaultDescription { get; set; }// exception message

    public string FaultType { get; set; } // controller, repository, external call,

}
