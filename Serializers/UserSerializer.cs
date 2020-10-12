using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Honeywell_backend.Serializers
{
    public class UserSerializer
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int Phone { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }               
        public string Password { get; set; }
        public bool IsStaff { get; set; }
    }
}
