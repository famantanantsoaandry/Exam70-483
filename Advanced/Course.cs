using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace   Exam70_483.Exercices.Advanced
{
    public class Course
    {
        public static void Launch()
        {
            // Example1();
            // Example2();
            // Example3();

            Example4();
        }

        public static void Example1()
        {
            PersonCollection myPeople = new PersonCollection();

            myPeople.Add(new Person("Lisa", "Simpson",9));
            myPeople.Add(new Person("Bart", "Simpson", 7));

            //change first person with indexer.

            myPeople[0] = new Person("Maggie", "Simpson", 2);

            //now obtain and display each item using indexer.

            for (int i = 0; i < myPeople.Count; i++)
            {
                Console.WriteLine("Person number : {0}",i);
                Console.WriteLine("Name: {0} {1}", myPeople[i].FirstName , myPeople[i].LastName);
                Console.WriteLine("Age: {0}", myPeople[i].Age);
                Console.WriteLine();
            }

            Console.ReadKey();


        }

        public static void Example2()
        {
            Console.WriteLine("*********** Fun with Indexers **************");

            PersonCollectionDic myPeople = new PersonCollectionDic();

            myPeople["Homer"] = new Person("Homer", "Simpson", 40);
            myPeople["Marge"] = new Person("Marge", "Simpson", 38);

            Person homer = myPeople["Homer"];

            Console.WriteLine(homer.ToString());

            Console.ReadLine();




        }

        public static void Example3()
        {

            Console.WriteLine("******** Fun with Overloaded Operators **************\n");

            //make two points.

            Point ptOne = new Point(100, 100);
            Point ptTwo = new Point(40, 40);

            Console.WriteLine("ptOne = {0}", ptOne);
            Console.WriteLine("ptTwo = {0}", ptTwo);

            //Add overload +

            Console.WriteLine("ptOne + ptTwo: {0}", ptOne + ptTwo);

            // Substract -

            Console.WriteLine("ptOne - ptTwo: {0}", ptOne - ptTwo);

            // Automatically the += is overloaded
            Point ptThree = new Point(90, 5);
            Console.WriteLine("ptThree = {0}", ptThree);
            Console.WriteLine("ptThree +=ptTwo {0}", ptThree +=ptTwo);

            //as well as -= 
            Point ptFour = new Point(0, 500);
            Console.WriteLine("ptFour = {0}", ptFour);
            Console.WriteLine("ptFour -=ptThree {0}", ptFour -= ptThree);



            Console.ReadLine();

        }

        public static void Example4()
        {
            Console.WriteLine("************** Fun with Conversions ***************\n");

            //make a rectangle.

            Rectangle r = new Rectangle(15, 4);
            Console.WriteLine(r.ToString());
            r.Draw();

            Console.WriteLine();

            //Convert r into a Square.
            //based on the height or the Rectangle.

            Square s = (Square)r;
            Console.WriteLine(s.ToString());
            s.Draw();
            Console.ReadLine();

        }

        #region internal class
        public class PersonCollection : IEnumerable
        {
            private ArrayList arPeople = new ArrayList();
            public IEnumerator GetEnumerator()
            {
                return arPeople.GetEnumerator();
            }

            // Custom indexer for this class.

            public Person this[int index]
            {
                get =>(Person) arPeople[index];
                set => arPeople.Insert(index, value);
            }

            public void Add(Person p)
            {
                arPeople.Add(p);
            }

            public void Clear()
            {
                arPeople.Clear();
            }

            public int Count { get { return arPeople.Count; } }
        }

        public class PersonCollectionDic : IEnumerable
        {
            

            private Dictionary<string, Person> listPeople = new Dictionary<string, Person>();
            public IEnumerator GetEnumerator()
            {
                return listPeople.GetEnumerator();
            }

            // Custom indexer for this class.

            public Person this[string name]
            {
                get => (Person)listPeople[name];
                set => listPeople[name] = value;
            }


            public void Clear()
            {
                listPeople.Clear();
            }

            public int Count => listPeople.Count;
        }

        public class Person
        {
            public int Age { get; set; }

            public string FirstName { get; set; }

            public string LastName { get; set; }

            public Person(string firstname, string lastname, int age)
            {
                FirstName = firstname;

                LastName = lastname;

                Age = age;
            }

            public override string ToString()
            {
                return "Name :" + FirstName + " LastName: " + LastName + " Age:" + Age;
            }
            
        }

        public class Point: IComparable<Point>
        {
            public int X { get; set; }

            public int Y { get; set; }

            public Point(int xPos, int yPos)
            {
                X = xPos;
                Y = yPos;

            }

            public override string ToString()  => $"[{this.X}, {this.Y}]";

            //Overloaded opererator +.

            public static Point operator + (Point p1, Point p2) => new Point(p1.X + p2.X, p1.Y + p2.Y);

            public static Point operator - (Point p1, Point p2) => new Point(p1.X - p2.X, p1.Y - p2.Y);

            public static Point operator ++ (Point p1) => new Point(p1.X +1, p1.Y + 1);

            public static Point operator -- (Point p1) => new Point(p1.X - 1, p1.Y - 1);


           //overload == and != operator
            public override bool Equals(object o) => o.ToString() == this.ToString();

            public override int GetHashCode() => this.ToString().GetHashCode();

            public static bool operator == (Point p1, Point p2) => p1.Equals(p2);

            public static bool operator != (Point p1, Point p2) => !p1.Equals(p2);


            //overloading > < operator 

            public int CompareTo(Point other)
            {
                if(this.X > other.X && this.Y > other.Y)
                    return 1;

                if (this.X < other.X && this.Y < other.Y)
                    return -1;

                else return 0;
            }

            public static bool operator <(Point p1, Point p2) => p1.CompareTo(p2) < 0;
            public static bool operator >(Point p1, Point p2) => p1.CompareTo(p2) > 0;

            public static bool operator <=(Point p1, Point p2) => p1.CompareTo(p2) <= 0;

            public static bool operator >=(Point p1, Point p2) => p1.CompareTo(p2) >= 0;
        }

        public struct Rectangle
        {

            public int Width { get; set; }
            public int Height { get; set; }

            public Rectangle(int w, int h) : this()
            {
                Width = w;
                Height = h;
            }

            public void Draw()
            {
                for (int i = 0; i < Height; i++)
                {
                    for (int j = 0; j < Width; j++)
                    {
                        Console.Write("*");
                    }

                    Console.WriteLine();
                }
            }

            public override string ToString() => $"[Width = {Width} ; Height = {Height}]";

        }

        public struct Square
        {
            public int Lenght { get; set; }

            public Square(int l) : this()
            {
                Lenght = l;
            }

            public void Draw()
            {
                for (int i = 0; i < Lenght; i++)
                {
                    for (int j = 0; j < Lenght; j++)
                    {
                        Console.Write("*");
                    }

                    Console.WriteLine();
                }
            }

            public override string ToString() => $"[Length = {Lenght}]";

            //Rectangles can be explicitly converted into Squares.
            public static explicit operator Square(Rectangle r)
            {
                Square s = new Square { Lenght = r.Height };

                return s;
            }

        }
        #endregion
    }
}
