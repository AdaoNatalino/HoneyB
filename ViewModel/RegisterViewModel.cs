using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Honeywell_backend.ViewModel
{
    public class RegisterViewModel
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
        [MaxLength(100)]
        public string Password { get; set; }

        [Required]
        public bool IsStaff { get; set; }
    }

    public class RegisterViewModelValidation: AbstractValidator<RegisterViewModel>
    {
        public RegisterViewModelValidation()
        {
            RuleFor(r => r.Email)
                .EmailAddress()
                .NotEmpty()
                .Length(1, 256);

            RuleFor(r => r.Password)
                .Matches(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{10,256}$");

            RuleFor(r => r.Address)
                .NotEmpty()
                .Length(1, 100);

            RuleFor(r => r.Name)
                .NotEmpty()
                .Length(1, 100);

            RuleFor(r => r.Phone)
                .NotEmpty()
                .Length(1, 20);

        }
    }
}
