using System;
using System.ComponentModel.DataAnnotations;

namespace Server.Api.Models.Input
{
    public class UserForRegistrationDto
    {
        [Required(ErrorMessage = "Required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Required")]
        [EmailAddress(ErrorMessage = "Invalid email address!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Required")]
        [StringLength(8, MinimumLength = 4, ErrorMessage = "Please specify a password between 4 and 8 characters!")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Required")]
        [StringLength(8, MinimumLength = 4, ErrorMessage = "Please specify a password between 4 and 8 characters!")]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        public string Gender { get; set; }

        public string KnownAs { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public DateTime Created { get; set; }

        public DateTime LastActive { get; set; }

        public UserForRegistrationDto()
        {
            Created = DateTime.Now;
            LastActive = DateTime.Now;
        }
    }
}
