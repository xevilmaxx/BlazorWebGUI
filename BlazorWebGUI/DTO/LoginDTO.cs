using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorWebGUI.DTO
{
    public class LoginDTO
    {

        [Required]
        [StringLength(8, ErrorMessage = "Name length can't be more than 8.")]
        public string Username { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "Password must be at least 3 characters long.", MinimumLength = 3)]
        public string Password { get; set; }

    }
}
