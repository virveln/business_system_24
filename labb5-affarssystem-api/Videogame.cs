using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Media;

namespace labb5_affarssystem_api
{
    public class Videogame : Product
    {
        public string Platform { get; }
        public Videogame(string category, int quantity, int id, string name, decimal price, string platform) : base(category, quantity, id, name, price)
        {
            this.Platform = platform;
        }

        public override string GetCsvLine()
        {
            return $"{Category};{Quantity};{Id};{Name};{Price};{Platform}";
        }
    }
}
