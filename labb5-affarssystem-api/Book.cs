using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace labb5_affarssystem_api
{
    internal class Book : Product
    {
        public string Author { get; }
        public string Genre { get; }
        public string Format { get; }
        public string Language { get; }

        public Book(string category, int quantity, int id, string name, decimal price, string author, string genre, string format, string language) : base(category, quantity, id, name, price)
        {
            this.Author = author;
            this.Genre = genre;
            this.Format = format;
            this.Language = language;
        }

        public override string GetCsvLine()
        {
            return $"{Category};{Quantity};{Id};{Name};{Price};{Author};{Genre};{Format};{Language}";
        }
    }
}
