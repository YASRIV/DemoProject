using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Car_Wash_Library.DTOClass
{
    public class SignupDto
    {

        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Role { get; set; } = null!; // "Customer" or "Washer"
        public string PhoneNumber { get; set; } = null!;

        // Customer-specific fields
        public string? Address { get; set; }
        public string? City { get; set; }
        public int? ZipCode { get; set; }

        // Washer-specific fields
        public bool? IsActive { get; set; }
    }
}
