using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace labb5_affarssystem_api
{
    public abstract class Product
    {
        public int Id { get; }
        public string Name { get; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        public string Category { get; }

        public static List<Product> ProductList { get; set; } = new List<Product>();

        public Product(string category, int quantity, int Id, string name, decimal price)
        {
            this.Id = Id;
            this.Name = name;
            this.Price = price;
            this.Category = category;
            this.Quantity = quantity;
        }

        public abstract string GetCsvLine();
    }
}
