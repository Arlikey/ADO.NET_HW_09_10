using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADO.NET_HW_09_10.Models
{
    internal class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public int Year { get; set; }
        public Genres Genre { get; set; }
    }

    public enum Genres
    {
        Fantasy,
        Science,
        Romance,
        Horror
    }
}
