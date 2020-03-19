using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace АРМ_билиотекаря
{
    public class Book
    {
        public String title;
        public String author;
        public String language;

        public Book(string title, string author, string language)
        {
            this.title = title;
            this.author = author;
            this.language = language;
        }
    }
}
