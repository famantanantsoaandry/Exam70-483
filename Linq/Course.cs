using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace   Exam70_483.Exercices.Linq
{
    public class Course
    {

        public static void Launch()
        {
           Example1();
           // Example2();
           //Example3();
           // Example4();
           //Example5();
           // Example6();
           // Example7();


        }

        #region Basic


        public static void Example1()
        {
            //linq to object 
            string[] currentVideoGames = { "Morrowind", "Uncharted 2", "Fallout 3" , "Draxter", "System Shock 2"};

            IEnumerable<string> subset = from g in currentVideoGames where g.Contains(" ") orderby g select g;

            RefectOverQueryResults(subset);

            //Print the result 

            foreach (string s in subset)
                Console.WriteLine("Item: {0}", s);

            Console.ReadKey(); 
        }

        public static void Example2()
        {
            //linq to object using extensions methods 
            string[] currentVideoGames = { "Morrowind", "Uncharted 2", "Fallout 3", "Draxter", "System Shock 2" };

            IEnumerable<string> subset = currentVideoGames.Where(g => g.Contains(" ")).OrderBy(g => g).Select(g => g);

            RefectOverQueryResults(subset);

            //Print the result 

            foreach (string s in subset)
                Console.WriteLine("Item: {0}", s);

            Console.ReadKey();
            
        }

        public static void Example3()
        {

            int[] numbers = { 10,20 ,30,40 ,1,2,3,8};

            //can use
            //IEnumerable subset =
            //var subset = 
            IEnumerable<int> subset = from i in numbers where i < 10 select i;

            RefectOverQueryResults(subset);

            //Lind deferred execution
            foreach (int i in subset)
                Console.WriteLine("Item : {0} < 10", i);

            Console.WriteLine();

            //change some data
            numbers[0] = 4;

            //Evaluated again 
            foreach(int i in subset)
                Console.WriteLine("Item : {0} < 10", i);

            Console.WriteLine();

            Console.ReadKey();
        }

        #endregion

        public static void Example4()
        {

            //generic collection implementing IEnumrable

            Console.WriteLine("****** LINQ over Generic Collections ******* \n");

            //Make a List<> of Car objects.

            List<Car> myCars = new List<Car>()
            {
                new Car{ Petname = "Henry", Color = "Silver" , Speed = 100 , Make = "BMW" },
                new Car{ Petname = "Daisy", Color = "Tan" , Speed = 90 , Make = "BMW" },
                new Car{ Petname = "Mary", Color = "Black" , Speed = 55 , Make = "VW" },
                new Car{ Petname = "Clunker", Color = "Rust" , Speed = 5 , Make = "Yugo" },
                new Car{ Petname = "Melvin", Color = "White" , Speed = 43 , Make = "Ford" }

            };

            var fastCars = from c in myCars where c.Speed > 55 && c.Make == "BMW" select c;

            foreach (var car in fastCars)
            {
                Console.WriteLine("{0} is going too fast!" , car.Petname);
            }

            Console.ReadKey();

        }

        public static void Example5()
        {
            //Linq over no generic collection

            ArrayList myCars = new ArrayList()
            {
                new Car{ Petname = "Henry", Color = "Silver" , Speed = 100 , Make = "BMW" },
                new Car{ Petname = "Daisy", Color = "Tan" , Speed = 90 , Make = "BMW" },
                new Car{ Petname = "Mary", Color = "Black" , Speed = 55 , Make = "VW" },
                new Car{ Petname = "Clunker", Color = "Rust" , Speed = 5 , Make = "Yugo" },
                new Car{ Petname = "Melvin", Color = "White" , Speed = 43 , Make = "Ford" }
            };

            var myCarsEnum = myCars.OfType<Car>();

            var fastCars = from c in myCarsEnum where c.Speed > 55  select c;

            foreach (var car in fastCars)
            {
                Console.WriteLine("{0} is going too fast!", car.Petname);
            }

            Console.ReadKey();
        }

        public static void Example6()
        {
            //Extracts data of certan type in the arrayList

            ArrayList myStuff = new ArrayList();

            myStuff.AddRange(new object[] { 10, 400, 8, false, new Car(), "string data" });

            var myInts = myStuff.OfType<int>();

            foreach (int i in myInts)
                Console.WriteLine("Int Value: {0}",i);

            Console.ReadKey();

        }


        public static void Example7()
        {
            Console.WriteLine("Fun with Query Expression");

            ProductInfo[] itemInStock =
            {
                new ProductInfo{ Name = "Mac's Cofee", Description = "Coffee with TEETH", NumberInStock = 24},

                new ProductInfo{ Name = "Milk Maid Milk", Description = "Milk cow's love", NumberInStock = 100},

                new ProductInfo{ Name = "Pure Silk Tofu", Description = "Bland as Possible", NumberInStock = 120},

                new ProductInfo{ Name = "Crunchy Pops", Description = "Cheezy , peppery goodness", NumberInStock = 2},

                new ProductInfo{ Name = "RipOff Water", Description = "From the tap to your wallet", NumberInStock = 100},

                new ProductInfo{ Name = "Classic Valpo Pizza", Description = "Everyone loves pizza !", NumberInStock = 73}

            };

            //1- get everything
            Console.WriteLine("All product details:");

            var allProducts = from p in itemInStock select p;

            foreach (var prod in allProducts)
                Console.WriteLine(prod.ToString());

            Console.WriteLine();

            //2- get only names of the products 
            Console.WriteLine("Only product names:");
            var names = from p in itemInStock select p.Name;

            foreach (var n in names)
                Console.WriteLine("Name: {0}", n);
           
            Console.WriteLine();

            //3-subset var result = from item in container where BooleanExpression select item;

            Console.WriteLine("The overstock items!");
            var overstock = from p in itemInStock where p.NumberInStock > 25 select p;

            foreach (var p in overstock)
                Console.WriteLine(p.ToString());

            Console.WriteLine();

            //4-Anonymous result

            Console.WriteLine("Names and Descriptions");

            var nameDesc = from p in itemInStock select new { p.Name, p.Description };

            foreach (var item in nameDesc)
                Console.WriteLine(item.ToString());

            Console.WriteLine();

            //5- Count

            int numb = (from p in itemInStock where p.NumberInStock > 25 select p).Count();

            Console.WriteLine("{0} items honor the LINQ query.", numb);

            //reverse , orderby


            Console.ReadKey();


        }

        #region internal object
        class Car
        {
            public string Petname { get; set; } = "";
            public string Color { get; set; } = "";
            public int Speed { get; set; }
            public string Make { get; set; } = "";

        }

        class ProductInfo
        {
            public string Name { get; set; } = "";
            public string Description { get; set; } = "";

            public int NumberInStock { get; set; } = 0;

            public override string ToString()
            
                => $"Name={Name} , Description={Description}, Number in Stock={NumberInStock}";
            
        }
        #endregion

        #region private Methods
        static void RefectOverQueryResults(object resultSet, string queryType = "Query Expressions")
        {
            Console.WriteLine($"***** Info about your query using {queryType}");
            Console.WriteLine("resultSet is of type : {0}", resultSet.GetType().Name);
            Console.WriteLine("resultSet Location : {0}", resultSet.GetType().Assembly.GetName().Name);
        }
        #endregion
    }
}
