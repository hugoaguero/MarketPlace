using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPlace.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Display(Name = "Product")]
        [Required(ErrorMessage = "Product name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Product price is required")]
        public string Price { get; set; }

        [Required(ErrorMessage = "Product description is required")]
        [Display(Name = "Description")]
        public string Description { get; set; }

        public string Image { get; set; }

        public bool Favorite { get; set; }

        [Required(ErrorMessage = "Product category is required")]
        public int categoryId { get; set; }

        public Category Category { get; set; }

        [Required(ErrorMessage = "Product brand is required")]
        public int brandId { get; set; }

        public Brand Brand { get; set; }
    }
}
