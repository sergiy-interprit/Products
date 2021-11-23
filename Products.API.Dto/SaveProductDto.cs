using System;

namespace Products.API.Dto
{
    public class SaveProductDto
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public int AccountId { get; set; }
    }
}