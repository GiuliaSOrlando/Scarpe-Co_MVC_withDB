using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Scarpe_Co_MVC.Models
{
    public class Utente
    {
        public int Id { get; set; }

        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set;}
        public bool isAdmin { get; set; }
    }
}