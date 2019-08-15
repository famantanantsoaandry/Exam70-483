using System;
using System.Threading;
using System.Windows.Forms;

namespace  Exam70_483.Exercices.Native
{
    public  class Course
    {
        static AutoResetEvent waitHandle = new AutoResetEvent(false);
        public static void Launch()
        {
            Example7();
        }

        static void Example1()
        {
            Console.WriteLine("***** The Amazing Thread App ******\n");
            Console.WriteLine("Do you want [1] or [2] threads? ");

            string threadCount = Console.ReadLine();

            // Name the current thread.
            Thread primaryThread = Thread.CurrentThread;
            primaryThread.Name = "Primary";

            // Display Thread info .
            Console.WriteLine("-> {0} is executing Main()", Thread.CurrentThread.Name);
            //Make a worker class
            Printer p = new Printer();

            switch (threadCount)
            {
                case "2":

                    //now make the thread

                    Thread backgroundThread = new Thread(new ThreadStart(p.PrintNumbers));
                    backgroundThread.Name = "Secondary";
                    backgroundThread.Start();
                    break;
                case "1":
                    p.PrintNumbers();
                    break;

                default:
                    Console.WriteLine("I don't know what you want ...you get 1 thread.");
                    goto case "1";
            }

            //Do some additional work.

            MessageBox.Show("I'm busy!", "Work on main thread...");

            Console.ReadLine();
        }

        static void Example2()
        {
            Console.WriteLine("****** Adding with Thread objects ******");
            Console.WriteLine("ID of thread in Main() : {0}", Thread.CurrentThread.ManagedThreadId);

            //make AddParams object to pass to the secondary thread.

            AddParams ap = new AddParams(10, 10);

            Thread t = new Thread(new ParameterizedThreadStart(Add));
            t.Start(ap);

            //Force a wait to let other thread finish.

            Thread.Sleep(5);

            Console.ReadLine();
        }

        static void Example3()
        {
            Console.WriteLine("****** Adding with Thread objects ******");
            Console.WriteLine("ID of thread in Main() : {0}", Thread.CurrentThread.ManagedThreadId);

            AddParams ap = new AddParams(10, 10);

            Thread t = new Thread(new ParameterizedThreadStart(Add1));
            t.Start(ap);

            //Wait here until you are notified!
            waitHandle.WaitOne();
            Console.WriteLine("Other Thread is done !");

            Console.ReadLine();
        }

        static void Example4()
        {
            Console.WriteLine("***** Background Threads *****\n");
            Printer p = new Printer();

            Thread bgroundThread = new Thread(new ThreadStart(p.PrintNumbers));

            //this is now a background thread.

            bgroundThread.IsBackground = true;
            bgroundThread.Start();
        }

        static void Example5()
        {
            Console.WriteLine("*****Synchronizing Threads ******\n");

            Printer p = new Printer();

            //Make 10 threads that are all pointing to the same
            //method on the same object 
            Thread[] threads = new Thread[10];

            for (int i = 0; i < 10; i++)
            {
                threads[i] = new Thread(new ThreadStart(p.PrintNumbers))
                {
                    Name = $"Worker thread #{i}"
                };

            }

            //Now start each one.

            foreach (Thread t in threads)
                t.Start();

            Console.ReadLine();

        }

        static void Example6()
        {
            Console.WriteLine("***** Worling with Timer type ****\n");

            //Create  the delegate for the timer type.
            TimerCallback timeCB = new TimerCallback(PrintTime);

            // Establish timer settings

            System.Threading.Timer t = new System.Threading.Timer(

                timeCB, // The TimerCallback delegate object.
                "Hello From Main" , //Any info to pass into the called method (null for no info).
                0, // Amount of time to wait before starting (in milliseconds).
                1000 // Interval of time between calls (in milliseconds).


                );

            // or discard 

            //var _ = new System.Threading.Timer(
            //    timeCB, // The TimerCallback delegate object.
            //    "Hello From Main 02", // Any info to pass into the called method (null for no info).
            //    0, // Amount of time to wait before starting (in milliseconds).
            //    1000); // Interval of time between calls (in milliseconds).



            Console.WriteLine("Hit Enter key to terminate ...");
            Console.ReadLine();

        }

        static void Example7()
        {
            Console.WriteLine("***** Fun with the CLR Thread Pool *****\n");

            Console.WriteLine("Main thread started . ThreadID = {0}", Thread.CurrentThread.ManagedThreadId);

            Printer p = new Printer();


            WaitCallback workItem = new WaitCallback(PrintTheNumbers);

            //Queue the method ten times.

            for (int i = 0; i < 10; i++)
            {

                ThreadPool.QueueUserWorkItem(workItem, p);
            }

            Console.WriteLine("All tasks queued");
            Console.ReadLine();
        }

        #region private Methods 
        static void Add(object data)
        {

            if (data is AddParams)
            {

                Console.WriteLine("ID of thread in Add(): {0}", Thread.CurrentThread.ManagedThreadId);

                AddParams ap = (AddParams)data;

                Console.WriteLine("{0} + {1} is {2}",ap.a,ap.b,ap.a+ap.b);
            }
        }

        static void Add1(object data)
        {

            if (data is AddParams)
            {

                Console.WriteLine("ID of thread in Add(): {0}", Thread.CurrentThread.ManagedThreadId);

                AddParams ap = (AddParams)data;

                Console.WriteLine("{0} + {1} is {2}", ap.a, ap.b, ap.a + ap.b);

                // Tell other thread we are done.

                waitHandle.Set();
            }
        }

        static void PrintTime(object state)
        {
            Console.WriteLine("Time is {0}, param is : {1}",DateTime.Now.ToLongTimeString(), state.ToString());
        }

        static void PrintTheNumbers(object state)
        {
            Printer task = (Printer)state;
            task.PrintNumbers();
        }
        #endregion

        #region internal class
        //this wil tell taht all methods of this object are all synchronized 
        // [Synchronization]
        public class Printer : ContextBoundObject
        {

            // Lock token.
            private object threadLock = new object();
            public void PrintNumbers()
            {
                // Display Thread info .
                Console.WriteLine(" ---> {0} is executing PrintNumbers()", Thread.CurrentThread.Name);

                // Print out numbers .

                Console.Write("Your numbers : ");

                for (int i = 0; i < 10; i++)
                {
                    Console.Write("{0}, ", i);
                    Thread.Sleep(2000);

                }

                Console.WriteLine();
            }

            public void PrintNumbers1()
            {
                //lock the consle the shared data between thread
                lock (threadLock)
                {
                    // Display Thread info .
                    Console.WriteLine(" ---> {0} is executing PrintNumbers()", Thread.CurrentThread.Name);

                    // Print out numbers .

                    Console.Write("Your numbers : ");

                    for (int i = 0; i < 10; i++)
                    {
                        // put Thread to sleep for a random amount of time.
                        Random r = new Random();
                        Thread.Sleep(1000 * r.Next(5));
                        Console.Write("{0}, ", i);

                    }

                    Console.WriteLine();
                }

            }

            public void PrintNumbers2()
            {
                //lock the consle the shared data between thread

                Monitor.Enter(threadLock);

                try
                {
                    // Display Thread info .
                    Console.WriteLine(" ---> {0} is executing PrintNumbers()", Thread.CurrentThread.Name);

                    // Print out numbers .

                    Console.Write("Your numbers : ");

                    for (int i = 0; i < 10; i++)
                    {
                        // put Thread to sleep for a random amount of time.
                        Random r = new Random();
                        Thread.Sleep(1000 * r.Next(5));
                        Console.Write("{0}, ", i);

                    }

                    Console.WriteLine();
                }
                finally
                {
                    Monitor.Exit(threadLock);
                }




            }
        }
        #endregion
    }
}
