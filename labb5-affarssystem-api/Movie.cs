using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Media.Casting;

namespace labb5_affarssystem_api
{
    public class Movie : Product
    {
        public string Format { get; }
        public int? Duration { get; }

        public Movie(string category, int quantity, int id, string name, decimal price, string format, int duration) : base(category, quantity, id, name, price)
        {
            Format = format;
            if (duration != 0)
                Duration = duration;
            else
                Duration = null;
        }

        public override string GetCsvLine()
        {
            return $"{Category};{Quantity};{Id};{Name};{Price};{Format};{Duration}";
        }

        //Writes "min" if Duration has value
        public string DurationAndMin
        {
            get
            {
                if (Duration.HasValue)
                    return $"{Duration} min";
                else
                    return string.Empty;
            }
        }
    }
}
