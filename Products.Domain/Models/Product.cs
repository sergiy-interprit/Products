using System;

namespace Products.Domain.Models
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public DateTime DateAdded { get; set; }

        // Foreign Key
        public int AccountId { get; set; }
        public Account Account { get; set; }  // Navigation property
    }
}