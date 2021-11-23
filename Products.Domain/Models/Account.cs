using System.Collections.Generic;

namespace Products.Domain.Models
{
    public class Account
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public ICollection<Product> Products { get; set; }
          = new List<Product>();
    }
}