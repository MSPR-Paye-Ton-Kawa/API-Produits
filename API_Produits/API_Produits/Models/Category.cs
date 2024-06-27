using System;
using System.ComponentModel.DataAnnotations;

namespace API_Produits.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string CategoryType { get; set; }
    }
}
