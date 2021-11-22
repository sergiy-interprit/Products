using System.Collections.Generic;

namespace Products.API.Models
{
    public class AccountDto 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public int NumberOfProducts
        {
            get
            {
                return Products.Count;
            }
        }

        public ICollection<ProductDto> Products { get; set; }
          = new List<ProductDto>();
    }
}
