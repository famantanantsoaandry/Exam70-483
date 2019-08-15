using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace   Exam70_483.Exercices.Parallel
{
    public struct Person
    {
        public string Name;
        public string Surname { get; set; }
        public string Adress;


        public Person(string name,string surname,string adress)
        {
            Name = name;
            Surname = surname;
            Adress = adress;

        }

      
    }
}
