using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Honeywell_backend.Serializers
{
    public class UserSerializer
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Address { get; set; }
        
        [Required]
        public int Phone { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Email { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Username { get; set; }    
        
        [Required]
        [MaxLength(100)]           
        public string Password { get; set; }
        
        [Required]
        public bool IsStaff { get; set; }
    }
}
