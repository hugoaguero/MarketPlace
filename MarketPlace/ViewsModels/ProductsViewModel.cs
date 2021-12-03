using MarketPlace.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPlace.ViewsModels
{
    public class ProductsViewModel
    {
        public List<Product> Products { get; set; }
        public SelectList Categories { get; set; }
        public string Name { get; set; }
    }
}
