using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Honeywell_backend.ViewModel
{
        public class UserLoginResponse
        {
            public string AccessToken { get; set; }
            public int ExpiresIn { get; set; }
            public UserViewModel UserToken { get; set; }
        }
}
