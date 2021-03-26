using System;
using System.Collections.Generic;
using System.Text;

namespace WebShopRauf.Models
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Klassen skapar modell för användare
    /// </summary>
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public DateTime LastLogin { get; set; }
        public DateTime SessionTime { get; set; }
        public bool IsActive { get; set; }
        public bool IsAdmin { get; set; }
    }
}
