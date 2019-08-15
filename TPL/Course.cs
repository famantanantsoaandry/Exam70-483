using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Exam70_483.Exercices.TPL
{
    public class Course
    {
         //global varibale shared between all the threads but they will have their own  copy 
        //[ThreadStatic] attribute
        [ThreadStatic]
        public static int _field;

        //Initialize a local data intilized for each Thread
        public static ThreadLocal<int> _field1 = new ThreadLocal<int>(() =>
        {
            return Thread.CurrentThread.ManagedThreadId;

        });

        public static void Launch()
        {

            Example1();
            //Example2();
            //Example3();
            // Example4();
            // Example44();
            // Example5();
            //Example6();
            // Example7();
            //Example8();
            //Example9();
            //Example10();
            //  Example11();
            //  Example12();
            // Example13();
            //  Example14();
            // Example15();
            // Example16();
            // Example17();
            //Example18();
            // Example19();
            //Example20();
           // Example21();
            //Example22();

        }


        #region classic Thread
        public static void Example1()
        {

            Thread t = new Thread(new ThreadStart(ThreadMethod));
            //this instruction says that the thread is a background thread 
            //main thread won't wait for it to finished it's task 
            t.IsBackground = true;
            t.Start();

            for (var i = 0; i < 4; i++)
            {
                Console.WriteLine("main Thread  : do some work");
                Thread.Sleep(0);
            }


            //force all other threads ( escepecially main thread ) to wait for it to finisshed 
            //comment or uncomment it to see the result
            //t.Join();

        }

        public static void Example2()
        {
            //use a thread with the method with params
            Thread t = new Thread(new ParameterizedThreadStart(ThreadMethod));
            t.IsBackground = true;
            t.Start(5);


            for (var i = 0; i < 4; i++)
            {
                Console.WriteLine("main Thread  : do some work");
                Thread.Sleep(0);
            }
        }

        public static void Example3()
        {
            //the better way to stop a thread by finishing its calling method
            //exaple of using lambda expression shortcut of delegate
            bool stopped = false;

            Thread t = new Thread(new ThreadStart(() =>

            {

                while (!stopped)
                {
                    Console.WriteLine("Running....");
                    Thread.Sleep(1000);
                }



            }));

            t.Start();
            Console.WriteLine("Press any key to exit ...");
            Console.ReadKey();
            stopped = true;
            t.Join();
        }
        public static void Example4()
        {
            //these thread will have the copy of the _field property because it a ThreadStatic property
            new Thread(() =>
            {
                for (int x = 0; x < 10; x++)
                {
                    _field++;
                    Console.WriteLine("Thread A : {0} ", _field);
                }
            }

            ).Start();

            new Thread(() =>
            {
                for (int x = 0; x < 10; x++)
                {
                    _field++;
                    Console.WriteLine("Thread B : {0}", _field);
                }
            }

           ).Start();

            Console.ReadKey();
        }

        public static void Example44()
        {
            new Thread(() =>
            {

                for (int x = 0; x < _field1.Value; x++)
                {
                    Console.WriteLine("Thread A : {0}", x);
                }

            }).Start();

            new Thread(() =>
            {
                for (int x = 0; x < _field1.Value; x++)
                {
                    Console.WriteLine("Thread B : {0}", x);
                }

            }).Start();

            Console.ReadKey();
        }

        public static void Example5()
        {
            //Queing a method to the system's thread pool
            //smiliar as connection pooling in database , more fatser because re creatring thread is resource consuming
            ThreadPool.QueueUserWorkItem((s) =>
            {
                Console.WriteLine("Working on a thread from threadpool");
            });

            Console.ReadLine();
        }

        #endregion

        #region Task
        public static void Example6()
        {
            //shortcoming of thread pool
            Task t = Task.Run(() =>
            {
                for (int x = 0; x < 100; x++)
                {
                    Console.Write('*');
                }
            });

            t.Wait();
        }

        public static void Example7()
        {
            //here the main thred is blocked by the task , wait for it to finished
            Task<int> t = Task.Run(() =>
            {
                return 42;
            });

            Console.WriteLine(t.Result); //should be 42
        }
        public static void Example8()
        {

            //notify the main thread when finished
            Task<int> t = Task.Run(() =>
            {

                Thread.Sleep(5000);
                return 42;
            }).ContinueWith((i) =>
            {

                return i.Result * 2;
            });

            Console.WriteLine(t.Result); //should be 84
        }

        public static void Example9()
        {

            //task in more advance way when succes , faulted , exceptuion ...etc
            Task<int> t = Task.Run(() =>
            {
                return 42;
            });

            t.ContinueWith((i) =>
            {
                Console.WriteLine("Canceled");
            }, TaskContinuationOptions.OnlyOnCanceled);

            t.ContinueWith((i) =>
            {
                Console.WriteLine("Faulted");
            }, TaskContinuationOptions.OnlyOnFaulted);

            var completedtask = t.ContinueWith((i) =>
            {
                Console.WriteLine("Completed");
            }, TaskContinuationOptions.OnlyOnRanToCompletion);

            completedtask.Wait();
        }

        public static void Example10()
        {
            //childs Taks
            //it is finished when all child tasks has completed
            Task<int[]> parent = Task.Run(() =>
            {
                var results = new int[3];

                new Task(() => results[0] = 0, TaskCreationOptions.AttachedToParent).Start();
                new Task(() => results[1] = 1, TaskCreationOptions.AttachedToParent).Start();
                new Task(() => results[2] = 2, TaskCreationOptions.AttachedToParent).Start();

                return results;

            });

            var finalTask = parent.ContinueWith(parenttask => {
                foreach (int i in parenttask.Result)
                    Console.WriteLine("Task :" + i);
            });

            finalTask.Wait();

        }

        public static void Example11()
        {
            //wait for all task to finhised
            Task[] tasks = new Task[3];
            tasks[0] = Task.Run(() =>
            {
                Thread.Sleep(1000);
                Console.WriteLine("1");
                return 1;
            });

            tasks[1] = Task.Run(() =>
            {
                Thread.Sleep(1000);
                Console.WriteLine("2");
                return 2;
            });

            tasks[2] = Task.Run(() =>
            {
                Thread.Sleep(1000);
                Console.WriteLine("3");
                return 3;
            });

            Task.WaitAll(tasks);

            //return a task to perform when all thread are finished
            Task.WhenAll(tasks);

        }

        public static void Example12()
        {
            Task<int>[] tasks = new Task<int>[3];

            tasks[0] = Task.Run(() =>
            {
                Thread.Sleep(2000);
                return 1;

            });


            tasks[1] = Task.Run(() =>
            {
                Thread.Sleep(1000);
                return 2;

            });

            tasks[2] = Task.Run(() =>
            {
                Thread.Sleep(3000);
                return 3;

            });

            while (tasks.Length > 0)
            {
                //we are going to get the index of the first completed task
                int i = Task.WaitAny(tasks);
                Task<int> completedTask = tasks[i];

                Console.WriteLine("task completed : " + completedTask.Result);

                var temp = tasks.ToList();
                //remove it when finished and resize the array then as it is
                temp.RemoveAt(i);
                tasks = temp.ToArray();

            }

        }

        #endregion

        #region parallel
        public static void Example13()
        {

            //1-exapmle 01
            Parallel.For(0, 10, i => {

                Thread.Sleep(1000);

            });


            //example 2

            var numbers = Enumerable.Range(0, 10);

            Parallel.ForEach(numbers, i => {

                Thread.Sleep(1000);

            });

            //example 3
            ParallelLoopResult result = Parallel.For(0, 1000, (int i, ParallelLoopState loopState) =>
            {
                if (i == 500)
                {
                    Console.WriteLine("Breaking Loop");
                    loopState.Break();
                }

            });

            Console.WriteLine("LowerBreak iteration : " + result.LowestBreakIteration);
            Console.WriteLine("Is Completed : " + result.IsCompleted);

            Console.ReadKey();

        }

        #endregion


        #region Asynchronous

        public static void Example14()
        {

            string result = DownloadContent().Result;
            Console.WriteLine(result);

        }

        #endregion

        #region PLINQ
        public static void Example15()
        {
            var numbers = Enumerable.Range(0, 100);

            Console.WriteLine("Before Processing .....");

            var parallelResult = numbers.AsParallel().AsOrdered().Where(i => i % 2 == 0).ToArray();

            // var parallelResultSequential = numbers.AsParallel().AsOrdered().Where(i => i % 2 == 0).AsSequential();

            foreach (var i in parallelResult.Take(50))
                Console.WriteLine(i);

            Console.WriteLine("Post Processing .......");

            Console.ReadKey();
        }

        public static void Example16()
        {
            var numbers = Enumerable.Range(0, 100);

            var parallelResult = numbers.AsParallel().Where(i => i % 2 == 0);

            parallelResult.ForAll(e => Console.WriteLine(e));

            Console.ReadKey();
        }

        public static void Example17()
        {
            var numbers = Enumerable.Range(0, 20);

            try
            {
                var parallelResult = numbers.AsParallel().Where(i => IsEven(i));

                parallelResult.ForAll(e => Console.WriteLine(e));
            }
            catch (AggregateException e)
            {
                Console.WriteLine("There where {0} exceptions", e.InnerExceptions.Count);
            }

            Console.ReadKey();

        }

        public static bool IsEven(int i)
        {
            if (i % 10 == 0) throw new ArgumentException("i");
            return i % 2 == 0;
        }

        #endregion

        #region Concurent Collections
        public static void Example18()
        {
            BlockingCollection<string> col = new BlockingCollection<string>();

            Task read = Task.Run(() =>
            {
                while (true)
                {
                    Console.WriteLine(col.Take());
                }

            });

            Task write = Task.Run(() =>
            {
                while (true)
                {
                    string s = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(s))
                        break;
                    col.Add(s);
                }

            });

            write.Wait();
        }

        public static void Example19()
        {
            BlockingCollection<string> col = new BlockingCollection<string>();

            Task read = Task.Run(() =>
            {
                foreach (string v in col.GetConsumingEnumerable())
                    Console.WriteLine(v);
            });

            Task write = Task.Run(() =>
            {
                while (true)
                {
                    string s = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(s))
                        break;
                    col.Add(s);
                }

            });

            write.Wait();
        }

        public static void Example20()
        {
            ConcurrentBag<int> bag = new ConcurrentBag<int>();

            bag.Add(42);
            bag.Add(21);

            int result;

            if (bag.TryTake(out result))
                Console.WriteLine(result);

            if (bag.TryPeek(out result))
                Console.WriteLine("There is a next item : {0}", result);

            Console.ReadKey();
        }

        public static void Example21()
        {
            ConcurrentBag<int> bag = new ConcurrentBag<int>();

            Task.Run(() =>
            {
                bag.Add(42);
                Thread.Sleep(1000);
                bag.Add(21);

            });

            Task.Run(() =>
            {
                foreach (int i in bag)
                    Console.WriteLine(i);
            }).Wait();

            Console.ReadKey();

        }

        public static void Example22()
        {
            //A stack is LIFO
            ConcurrentStack<int> stack = new ConcurrentStack<int>();
            stack.Push(42);

            int result;

            if (stack.TryPop(out result))
                Console.WriteLine("Popped : {0}", result);

            stack.PushRange(new int[] { 1, 2, 3 });

            int[] values = new int[2];

            stack.TryPopRange(values);

            foreach (int i in values)
                Console.WriteLine(i);

            Console.ReadKey();
        }

        public static void Example23()
        {
            //queue is FIFO
            ConcurrentQueue<int> queue = new ConcurrentQueue<int>();
            queue.Enqueue(42);
            int result;

            if (queue.TryDequeue(out result))
                Console.WriteLine("Dequeued : {0}", result);
            Console.ReadKey();
        }

        public static void Example24()
        {
            var dict = new ConcurrentDictionary<string, int>();

            if (dict.TryAdd("k1", 42))
            {
                Console.WriteLine("Added");
            }

            if (dict.TryUpdate("k1", 21, 42))
            {
                Console.WriteLine("42 updated to 21");
            }

            dict["k1"] = 42; // Overwrite unconditionally

            int r1 = dict.AddOrUpdate("k1", 3, (s, i) => i * 2);
            int r2 = dict.GetOrAdd("k2", 3);
        }

        #endregion

        #region Private Methods

        private static async Task<string> DownloadContent()
        {
            using (HttpClient client = new HttpClient())
            {
                string result = await client.GetStringAsync("https://www.microsoft.com");
                return result;
            }
        }

        public Task SleepAsyncA(int millisecondsTimeout)
        {
            return Task.Run(() => Thread.Sleep(millisecondsTimeout));
        }

        public Task SleepAsynccB(int millisecondsTimeout)
        {
            TaskCompletionSource<bool> tcs = null;
            var t = new Timer(delegate { tcs.TrySetResult(true); }, null, -1, -1);
            tcs = new TaskCompletionSource<bool>(t);
            t.Change(millisecondsTimeout, -1);
            return tcs.Task;
        }
        private static void ThreadMethod()
        {
            for (int i = 0; i < 10; i++)
            {

                Console.WriteLine("ThreadProc : {0}", i);
                Thread.Sleep(1000);

            }
        }

        private static void ThreadMethod(object o)
        {
            for (int i = 0; i < (int)o; i++)
            {

                Console.WriteLine("ThreadProc : {0}", i);
                Thread.Sleep(1000);

            }
        }
        #endregion
    }
}
