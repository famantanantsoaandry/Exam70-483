using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace   Exam70_483.Exercices.Reflexivite
{
    public class Course
    {
        public static void Launch()
        {
            // Example1();
            // Example2();

            Example3();
        }

        public static void Example1()
        {
            var program = new Program();
            program.Affiche();

            Console.ReadKey();
        }

        public static void Example2()
        {
            Type type = typeof(string);

            foreach (MethodInfo info in type.GetMethods())
            {
                Console.WriteLine(info.Name);
            }

            Console.ReadKey();
        }

        public static void Example3()
        {
            Chien chien = new Chien();

            SeeDescription(chien);


            Console.ReadKey();
        }

        public static void SeeDescription<T>(T obj)
        {
            Type type = typeof(T);

            if (!type.IsClass)
                return;


            Attribute[] attributes = Attribute.GetCustomAttributes(type, typeof(DescriptionClassAttribute));

            if (attributes.Length == 0)
                Console.WriteLine(" There are no description for the class " + type.Name + "\n");

            else
            {
                Console.WriteLine("Description for the class " + type.Name);

                foreach (DescriptionClassAttribute attribute in attributes)
                {
                    Console.WriteLine("\t" + attribute.Description);
                }
            }

        }

        public class Program
        {
            [Obsolete("Utilisez plutot la methode ToString() pour avoir une representation de l'objet")]
            public void Affiche()
            {

            }
        }

        [AttributeUsage(AttributeTargets.Class, AllowMultiple =true)]
        public class DescriptionClassAttribute : Attribute
        {
            public string Description { get; set; }

            public DescriptionClassAttribute()
            {

            }

            public DescriptionClassAttribute(string description)
            {
                Description = description;
            }

        }

        public class Animal
        {

        }

        [DescriptionClass (Description ="This class is a Dog")]
        [DescriptionClass(Description ="inherits from Animal class")]
        public class Chien: Animal
        {
           // [DescriptionClass]
            public void Aboyer()
            {

            }
        }
    }
}
