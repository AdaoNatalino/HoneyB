using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Honeywell_backend.ViewModel
{
    public class UserViewModel
    {
        public Guid Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Address { get; set; }
        
        [Required]
        [MaxLength(20)]
        public string Phone { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Email { get; set; }               
        
        [Required]
        public bool IsStaff { get; set; }
    }
}
