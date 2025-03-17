using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Car_Wash_Library.Models
{
    public class AuthResponse
    {
        public User User { get; set; }
        public Role Role { get; set; }
        public string Token { get; set; }
        public AuthResponse(User user, Role role, string token) => (User, Role, Token) = (user, role, token);

    }
}

