using System;
using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class BasketItemDto
    {
        
        [Required]
        public int Id { get; set; }
        [Required]
        public string ProductName { get; set; }
        [Required]
        [Range(0.1,double.MaxValue,ErrorMessage = "Price must not be zero")]
        public decimal Price { get; set; }
        [Required]
        [Range(1,double.MaxValue,ErrorMessage ="Can't Add Zero quantity to the basket")]
        public int Quantity { get; set; }
        [Required]
        public string PictureUrl { get; set; }
        [Required]
        public string Brand { get; set; }
        [Required]
        public string Type { get; set; }
    }
}