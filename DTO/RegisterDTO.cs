﻿using System.ComponentModel.DataAnnotations;

namespace ShortURL.DTO
{
    public class RegistrationDTO
    {

        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
