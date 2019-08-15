using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace   Exam70_483.Exercices.Delegate
{
   public  class Course
    {
        public delegate int BinaryOp(int x, int y);
        private static bool isDone = false;
       

        public static void Launch()
        {
            Example1();
            //Example2();
            //Example3();
            // Example4();
            // Example5();
            //Example6();
           // Example7();
            
        }

        static void Example1()
        {
            Console.WriteLine("***** Synch Deletgate Review ****");

            //Print out the ID of the executing thread.
            Console.WriteLine("main () invoked on thread {0}.", Thread.CurrentThread.ManagedThreadId);

            //invoke Add() in a synhcronous manner.
            BinaryOp b = new BinaryOp(Add);

            //Could laso write b.Invoke(10,10);

            int answer = b(10, 10);

            //these lines will not execute unitl 
            //the Add() method has completed .

            Console.WriteLine("Doing more work in Main()!");
            Console.WriteLine("10 + 10 is {0}.", answer);

            Console.ReadLine();
        }

        static void Example2()
        {
            Console.WriteLine("****** Async Delegate Invocation ******");

            // Print out the ID of the executing thread.

            Console.WriteLine("Main()  invoked on thread {0}.", Thread.CurrentThread.ManagedThreadId);

            // Invoke Add() on secondary thread.

            BinaryOp b = new BinaryOp(Add);

            IAsyncResult ar = b.BeginInvoke(10, 10, null, null);

            //Do other work on primary thread ....
            //this message will keep printing until
            //the Add() method is finished.

            while (!ar.IsCompleted)
            {
                Console.WriteLine("Doing more work in Main()!");
                Thread.Sleep(1000);
            }

            //while (!ar.AsyncWaitHandle.WaitOne(1000, true))
            //{
            //    Console.WriteLine("Doing more work in Main()!");
            //}

            // Now we know the Add() method is completed.
            int answer = b.EndInvoke(ar);

            Console.WriteLine("10 +10 is {0}.", answer);
            Console.ReadLine();
        }

        static void Example3()
        {
            Console.WriteLine("****** AsynccallbackDelegate Example ******");

            // Print out the ID of the executing thread.

            Console.WriteLine("Main()  invoked on thread {0}.", Thread.CurrentThread.ManagedThreadId);

            // Invoke Add() on secondary thread.

            BinaryOp b = new BinaryOp(Add);

            IAsyncResult ar = b.BeginInvoke(10, 10, new AsyncCallback(AddComplete), "Main() thanks you for adding these numbers.");

            //Do other work on primary thread ....
            //this message will keep printing until
            //the Add() method is finished.

            while (!isDone)
            {
                Console.WriteLine("Working .....");
                Thread.Sleep(1000);
            }

            Console.ReadLine();
        }

        public static void Example4()
        {
            Console.WriteLine("******* Delegate as event enablers *********\n");

            //First , make a Car object.

            Car c1 = new Car("SlugBug", 100, 10);

            c1.RegisterWithCarEngine(OnCarEngineEvent);

            // Speed upp (this will trigger the events).

            for (int i = 0; i < 6; i++)
                c1.Accelerate(20);

            Console.ReadLine();


        }

        static void Example5()
        {
            Console.WriteLine("***** Fun with Events ******\n");

            Car c1 = new Car("SlugBug", 100, 10);

            //Register Event Handler

            c1.AboutToBlow += CarIsAlmostDoomed;
            c1.AboutToBlow += CarAboutToBlow;
            c1.Exploded += CarExploded;

            Console.WriteLine("**********Sppeding up ************");

            for (int i = 0; i < 6; i++)
            {
                c1.Accelerate(20);
            }

            c1.Exploded -= CarExploded;

            Console.WriteLine("\n****** Speeding up *******");

            for (int i = 0; i < 6; i++)
            {
                c1.Accelerate(20);
            }

            Console.ReadLine();

        }

        static void Example6()
        {
            Console.WriteLine("******* Anonymous Methods *************\n");

            Car c1 = new Car("Slugbug",100,10);

            //Register Event HAndler as anonymous methods

            c1.AboutToBlow1 += delegate
            {

                Console.WriteLine("Eek! Going too fast!");
            };

            c1.AboutToBlow1 += delegate (object sender, CarEventArgs e)
            {

                Console.WriteLine("Message from Car: {0}", e.msg);
            };

            c1.AboutToBlow1 +=  (sender, e) =>
            {

                Console.WriteLine("Message from Car: {0}", e.msg);
            };





        }

        static void Example7()
        {
            //List of integers

            List<int> list = new List<int>();

            list.AddRange(new int[] { 20, 1, 4, 8, 9, 44 });

            //Call Find All using traditional delegate syntax.

            Predicate<int> callBack = IsEvenNumber;

           List<int> evenNumbers = list.FindAll(callBack);

            //  evenNumbers = list.FindAll(IsEvenNumber);

            //  evenNumbers = list.FindAll(delegate(int i) { return i % 2 == 0;  });

          //  evenNumbers = list.FindAll(i => i % 2 ==0);

            Console.WriteLine("Here are your even Numbers :");

            foreach (int evenNumber in evenNumbers)
                Console.Write("{0}\t", evenNumber);

            Console.WriteLine();

            Console.Read();

        }

        public static bool IsEvenNumber(int i)
        {
            return (i % 2) == 0;
        }

        public static void OnCarEngineEvent(string msg)
        {
            Console.WriteLine("\n********* Message From Car Object ********");
            Console.WriteLine("=> {0}", msg);
            Console.WriteLine("***************************************\n");
        }

        public static void CarAboutToBlow(string msg)
        {
            Console.WriteLine(msg);
        }

        public static void CarIsAlmostDoomed(string msg)
        {
            Console.WriteLine("=> Critical Message from Car : {0}", msg);
        }

        public static void CarExploded(string msg)
        {
            Console.WriteLine(msg);
        }

        #region private methods
        static int Add(int x, int y)
        {
            //Print out the Id of the executing Thread.
            Console.WriteLine("Add() invoked on thread {0},",Thread.CurrentThread.ManagedThreadId);

            //pause to simulate a lengthy operation.
            Thread.Sleep(5000);
            return x + y;
        }

        static void AddComplete(IAsyncResult iar)
        {
            Console.WriteLine("AddComplete() invoked on thread {0}.", Thread.CurrentThread.ManagedThreadId);

            Console.WriteLine("Your addition is complete");

            //now get the result.

            AsyncResult ar = (AsyncResult)iar;

            BinaryOp b = (BinaryOp)ar.AsyncDelegate;

            Console.WriteLine("10 + 10 is {0}.", b.EndInvoke(iar));

            //retrieve the object passed in params of begin invoke

            string msg = (string)iar.AsyncState;

            Console.WriteLine(msg);

            isDone = true;
        }

        #endregion

        #region internal class
        public class Car
        {

            // define a delagate type.
            public delegate void CarEngineHandler(string msgForCaller);

            public Action<object, CarEventArgs> CarEngineHandler1;

            // Define a member variable of this delegate

            public CarEngineHandler listOfHandlers;


            public event CarEngineHandler Exploded;

            public event EventHandler<CarEventArgs> Exploded1;

            public event CarEngineHandler AboutToBlow;

            public event EventHandler<CarEventArgs> AboutToBlow1;



            public void RegisterWithCarEngine(CarEngineHandler methodToCall)
            {
                listOfHandlers = methodToCall;
            }

            //Impelemnt tgeh Accelerate() method to invoke the delegate's invocation list under the correct circumstances.

            public void Accelerate(int delta)
            {
                // if this car is dead , send dead message.

                if (carIsDead)
                {
                    if (Exploded != null)
                        Exploded("Sorry , this car is dead ....");

                   /// can use also c#6 Exploded?.Invoke("Sorry , this car is dead ....");
                }
                else
                {
                    CurrentSpeed += delta;

                    //Is this Car almost dead

                    if (10 == (MaxSpeed - CurrentSpeed) && AboutToBlow != null)
                    {
                        AboutToBlow("Careful buddy! Gonna blow!");
                    }

                    //Still Ok
                    if (CurrentSpeed >= MaxSpeed)
                        carIsDead = true;
                    else
                        Console.WriteLine("Current ={0}", CurrentSpeed);
                }
            }

            //Internal state data.
            public int CurrentSpeed { get; set; }
            public int MaxSpeed { get; set; } = 100;

            public string PetName { get; set; }

            private bool carIsDead;

            public Car()
            {

            }

            public Car(string name, int maxSp, int currSp)
            {
                CurrentSpeed = currSp;
                MaxSpeed = maxSp;
                PetName = name;
            }

        }

        public class CarEventArgs : EventArgs
        {
            public readonly string msg;

            public CarEventArgs(string msg)
            {
                this.msg = msg;
            }

        }
        #endregion
    }
}
