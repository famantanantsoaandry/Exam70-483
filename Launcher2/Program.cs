using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using   Exam70_483.Exercices.Launcher2.ServiceReference1;

namespace   Exam70_483.Exercices.Launcher2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("***** Ask the Magic 8 Ball ******\n");

            using (EigthBallClient ball = new EigthBallClient())
            {
                Console.Write("Your question: ");
                string question = Console.ReadLine();
                string answer = ball.ObtainAnswerToQuestion(question);

                Console.WriteLine("8-ball says: {0}", answer);
            }

            Console.ReadLine();
        }
    }
}
