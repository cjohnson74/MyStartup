using System;
using System.ComponentModel.DataAnnotations;

namespace WiredSneakz.Models
{
    public class Sneaker
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }
        public string Colorway { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
    }
}
