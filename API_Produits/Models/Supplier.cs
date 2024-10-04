using System;
using System.ComponentModel.DataAnnotations;

namespace API_Produits.Models
{
    public class Supplier
    {
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
        public string ContactEmail { get; set; }
    }
}
