using System;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using   Exam70_483.Exercices.Launcher.ServiceReference1;
using   Exam70_483.Exercices.WCF;
using static   Exam70_483.Exercices.WCF.Course;

namespace   Exam70_483.Exercices.Launcher
{
    class Program
    {
        static void Main(string[] args)
        {
            
            //   Exam70_483.Exercices.WCF.Course.Launch();

                Console.WriteLine("****** Console Based WCF Host *******");


                using (ServiceHost serviceHost = new ServiceHost(typeof(MagicEightBallService)))
                {
                    //open the host and start listening for incoming message
                    serviceHost.Open();

                    DisplayHostInfo(serviceHost);

                    //Keep Service Runing until key pressed.
                    Console.WriteLine("The service is ready.");
                    Console.WriteLine("Press Enter key to terminate service.");
                    Console.ReadLine();

                }

        }

        static void DisplayHostInfo(ServiceHost host)
        {
            Console.WriteLine();
            Console.WriteLine("****** Host Info *******");

            foreach (System.ServiceModel.Description.ServiceEndpoint se in host.Description.Endpoints)
            {

                Console.WriteLine("Adress: {0}",se.Address);
                Console.WriteLine("Binding: {0}",se.Binding.Name);
                Console.WriteLine("Contract: {0}", se.Contract.Name);

            }

            Console.WriteLine();

        }
    }
}
