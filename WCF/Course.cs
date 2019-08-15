using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace   Exam70_483.Exercices.WCF
{
    public class Course
    {
        public static void Launch()
        {
           // Example1();
        }

        //public static void Example1()
        //{
        //    Console.WriteLine("****** Console Based WCF Host *******");

        //    using (ServiceHost serviceHost = new ServiceHost(typeof(MagicEightBallService)))
        //    {
        //        //open the host and start listening for incoming message
        //        serviceHost.Open();

        //        //Keep Service Runing until key pressed.
        //        Console.WriteLine("The service is ready.");
        //        Console.WriteLine("Press Enter key to terminate service.");
        //        Console.ReadLine();

        //    }


        //    Console.ReadLine();
        }

        [ServiceContract(Namespace ="http://MyCompany.com")]
        public interface IEigthBall
        {
            //ask a question , receive and answer !
            [OperationContract]
            string ObtainAnswerToQuestion(string userQuestion);
        }

        public class MagicEightBallService : IEigthBall
        {
            //Just for display purposes on the host
            public MagicEightBallService()
            {
                Console.WriteLine("The 8-Ball awaits your question ......");
            }
            public string ObtainAnswerToQuestion(string userQuestion)
            {
                string[] answers = {"Future Incertain" , "Yes", "No", "Hazy", "Ask again later", "Definitely"};

                //return random response

                Random r = new Random();

                return answers[r.Next(answers.Length)];
            }
        }
    }

