using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace АРМ_билиотекаря
{
    public class Reader
    {
        public String id;
        public String name;
        public String surname;
        public String patronymic;
        public DateTime birthday;
        public string phone_number;
        public string adress;

        public Reader(string id, string name, string surname, string patronymic, DateTime birthday, string phone_number, string adress)
        {
            this.id = id;
            this.name = name;
            this.surname = surname;
            this.patronymic = patronymic;
            this.birthday = birthday;
            this.phone_number = phone_number;
            this.adress = adress;
        }
    }
}
