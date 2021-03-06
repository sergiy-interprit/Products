using System;

namespace Products.API.Dto
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public DateTime DateAdded { get; set; }

        //public AccountDto Account { get; set; }

        //public AccountWithoutProductsDto Account { get; set; }

        public int AccountId { get; set; }
    }
}
