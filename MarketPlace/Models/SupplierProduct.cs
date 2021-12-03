using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPlace.Models
{
    public class SupplierProduct
    {
        public int Id { get; set; }
        public int supplierId { get; set; }
        public Supplier Supplier { get; set; }
        public int productId { get; set; }
        public Product Product { get; set; }
    }
}
