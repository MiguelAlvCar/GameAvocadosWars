using System.ComponentModel.DataAnnotations;
using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;

namespace DTO_Models
{
    public class CreateModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }

    public class LoginModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
