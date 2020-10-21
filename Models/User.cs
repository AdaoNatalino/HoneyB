using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Honeywell_backend.Models
{
    public class User
    {            
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }        
        public bool IsStaff { get; set; }

    }
}
