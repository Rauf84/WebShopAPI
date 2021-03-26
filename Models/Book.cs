using System;
using System.Collections.Generic;
using System.Text;

namespace WebShopRauf.Models
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Klassen skapar modell för böcker
    /// </summary>
    public class Book
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int Price { get; set; }
        public int Amount { get; set; }
        public int CategoryId { get; set; }

    }
}
