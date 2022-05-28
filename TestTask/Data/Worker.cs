using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTask.Data
{
    public class Worker
    {

        String name { get; set; }
        String birthday { get; set; }
        String gender { get; set; }
        String namediv { get; set; }
        String post { get; set; }
        String info { get; set; }

        int _id;
        public int Id { get { return _id; } set { _id = value; } }
        public String Name { get { return name; } set { name = value; } }
        public String Birthday { get { return birthday; } set { birthday = value; } }
        public String Gender { get { return gender; } set { gender = value; } }
        public String NameDiv { get { return namediv; } set { namediv = value; } }
        public String Post { get { return post; } set { post = value; } }
        public String Info { get { return info; } set { info = value; } }

        public Worker()
        {
            _id = 999;
            name = "s";
            birthday = "b";
            gender = "g";
            namediv = "m";
            post = "p";
            info = "i";
        }


    }
}
