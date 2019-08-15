using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters.Soap;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace   Exam70_483.Exercices.Serialization
{
    public class Course
    {
        public static void Launch()
        {
            // Example1();
            // Example2();
            // Example3();
            // Example4();
            //Example5();
            // Example6();
            // Example7();
            //Example8();
            // Example9();
            //Example10();
            //Example11();
            //Example12();
            // Example13();
            // Example14();
            // Example15();
            Example16();
        }

        public static void Example1()
        {
            DirectoryInfo dir = new DirectoryInfo(@"C:\Windows");

            //1- Basic manips
            Console.WriteLine("********Directory Info ********");
            Console.WriteLine(" Fullname: {0}", dir.FullName);
            Console.WriteLine("Name: {0}", dir.Name);
            Console.WriteLine("Parent : {0}", dir.Parent);
            Console.WriteLine("Creation: {0}", dir.CreationTime);
            Console.WriteLine("Attributes: {0}", dir.Attributes);
            Console.WriteLine("Root: {0}", dir.Root);
            Console.WriteLine("************************************\n");

            //2-

            dir = new DirectoryInfo(@"\\smals-mvm.be\data\Users\ano\Documents\TRAINING");
            //find all files with .png extension
            Console.WriteLine("List all PNG files in Direcotry : {0}", dir.FullName);
            FileInfo[] imageFiles = dir.GetFiles("*.pdf", SearchOption.AllDirectories);
            Console.WriteLine("Found {0} *.png", imageFiles.Length);

            //print out each files 

            foreach (FileInfo f in imageFiles)
            {
                Console.WriteLine("*************************");

                Console.WriteLine("File name: {0}",f.Name);
                Console.WriteLine("File size: {0}", f.Length);
                Console.WriteLine("Creation: {0}", f.CreationTime);
                Console.WriteLine("Attributes: {0}", f.Attributes);

                Console.WriteLine("*************************");
            }

            Console.ReadKey();
        }

        public static void Example2()
        {
            DirectoryInfo dir = new DirectoryInfo(@"C:\");

            var subDir = "MyFolder";

            Console.WriteLine("Create Sub Dir {1} in {0} ", dir.FullName, subDir);

            dir.CreateSubdirectory(subDir);

            var subsubDir = @"MyFolder\Data";

            Console.WriteLine("Create Sub Sub Dir {1} in {0} ", dir.FullName, subsubDir);
            dir.CreateSubdirectory(subsubDir);

            Console.ReadKey();

        }

        public static void Example3()
        {
            //using the static Directory class
            string[] drives = Directory.GetLogicalDrives();
            Console.WriteLine("Here are your drives:");

            foreach (string s in drives)
                Console.WriteLine("--> {0} ",s);

            //Delete wghat was created .

            Console.WriteLine("Press Enter to delete directories");
            Console.ReadLine();

            try
            {
                //delete , throw an exception if no empty
                Directory.Delete(@"C:\MyFolder");

                //delete with subdirectories
                Directory.Delete(@"C:\MyFolder2", true);

            }
            catch (IOException e)
            {
                Console.WriteLine("Error : "+e.Message);
            }


            Console.ReadKey();
        }

        public static void Example4()
        {
            //DriveInfo

            Console.WriteLine("********* Fun with DriveInfo **************\n");

            //get info regading all drives.

            DriveInfo[] myDrives = DriveInfo.GetDrives();

            //print all driive stats.

            foreach (DriveInfo d in myDrives)
            {
                Console.WriteLine("Name : {0}", d.Name);
                Console.WriteLine("Type: {0}", d.DriveType);

                //check to see if the drive is mounted.
                if (d.IsReady)
                {
                    Console.WriteLine("Free space: {0}", d.TotalFreeSpace);
                    Console.WriteLine("Format: {0}", d.DriveFormat);
                    Console.WriteLine("Label: {0}", d.VolumeLabel);
                }
            }



            Console.ReadKey();

        }

        public static void Example5()
        {

            Console.WriteLine("Create file using FileInfo");
            //create a new file on c drive
            FileInfo f = new FileInfo(@"C:\KOUl\Data\Test.dat");

            using (FileStream fs = f.Create())
            {

                //Use FileStream ...to do

               
            }

            FileInfo f2 = new FileInfo(@"C:\KOUl\Data\Test2.dat");

            using (FileStream fs2 = f2.Open(FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
            {

                //use opend filestream
            }


            Console.ReadKey();
        }

        public static void Example6()
        {
            Console.WriteLine("******* Fun with FileStreams ********\n");

            //Obtain File Stream Object 

            using (FileStream fs = File.Open(@"C:\KOUl\Data\Testfs.dat", FileMode.Create))
            {

                //encode string to bytes
                string msg = "Hello!";

                byte[] msgAsByteArray = Encoding.Default.GetBytes(msg);

                //write bytes to fs 

                fs.Write(msgAsByteArray, 0, msgAsByteArray.Length);

                //Reset internal posiion of stream

                fs.Position = 0;

                //Read the types from file and display to console

                Console.Write("Your message as an array of bytes: ");

                byte[] bytesFromFile = new byte[msgAsByteArray.Length];

                for (int i = 0; i< msgAsByteArray.Length; i++)
                {
                    bytesFromFile[i] = (byte)fs.ReadByte();

                    Console.Write(bytesFromFile[i]);
                }

                //Display Decoded Message 

                Console.WriteLine(" Decoded message : ");

                Console.WriteLine(Encoding.Default.GetString(bytesFromFile));

            }

            Console.ReadKey();
        }

        public static void Example7()
        {
            Console.WriteLine("Using Stream Reader Opentext() .......");
            //stream reader 
            FileInfo f5 = new FileInfo(@"C:\KOUl\Data\boot.ini");

            using (StreamReader sreader = f5.OpenText())
            {
                //Use the StreamReader object .....
            }
            Console.WriteLine("Using Stream Reader Createtext() .......");

            FileInfo f6 = new FileInfo(@"C:\KOUl\Data\Test6.txt");

            using (StreamWriter swriter = f6.CreateText())
            {
                //Use Stream Writer
            }

            Console.WriteLine("Using Stream Reader Appendtext() .......");

            FileInfo f7 = new FileInfo(@"C:\KOUl\Data\Finaltest.txt");

            using (StreamWriter swriter = f7.AppendText())
            {
                //Use Stream Writer
            }

            Console.ReadKey();
        }

        public static void Example8()
        {
            Console.WriteLine("***** Simple I/O with the File Type *******\n");

            string[] myTasks = {"Fix bathroom sink", "Call Dave" , "Call Mom and Dad" , "Play Xbox one"};

            //write out all data to a file 

            File.WriteAllLines(@"C:\KOUl\Data\boot.ini", myTasks);

            //Read it all back and print out 

            Console.WriteLine("******Read previous Data as array of string[] ***************");

            foreach (string task in File.ReadAllLines(@"C:\KOUl\Data\boot.ini"))
            {
                Console.WriteLine("TODO: {0}", task);
            }

            //Read it once as string
            Console.WriteLine("Read previous Data as one string[]");

            Console.WriteLine(File.ReadAllText(@"C:\KOUl\Data\boot.ini"));


            Console.ReadKey();

        }

        public static void Example9()
        {
            //Streamwriter and StreamReader

            Console.WriteLine("********* StreamWriter and StreamReader *******");

            //equivalent to StreamWriter writer = new StreamWriter(@"C:\KOUl\Data\reminders.txt")
            using (StreamWriter writer = File.CreateText(@"C:\KOUl\Data\reminders.txt"))
            {
                writer.WriteLine("Don't forget Mother's Day this year.....");
                writer.WriteLine("Don't forget Father's Day this year.....");
                writer.WriteLine("Don't forget these numbers:");

                for (int i = 0; i < 10; i++)
                {
                    writer.Write(i + " ");
                }

                //insert a new Line

                writer.Write(writer.NewLine);
            }
            //equivalent to StreamReader sr = new StreamReader(@"C:\KOUl\Data\reminders.txt")
            using (StreamReader sr = File.OpenText(@"C:\KOUl\Data\reminders.txt"))
            {
                string input = null;

                while ((input = sr.ReadLine()) != null)
                {
                    Console.WriteLine(input);
                }

            }

                Console.WriteLine("*****************");

            Console.ReadLine();

        }

        public static void Example10()
        {
            //string writer   and string reader : in memory stream rather than file

            Console.WriteLine("****** Fun with StringWriter / StringReader *********\n");

            using (StringWriter strWriter = new StringWriter())
            {
                strWriter.WriteLine("Don't forget Mother's Day this year");

                Console.WriteLine("Contents of StringWriter:\n {0}",strWriter);

                //Get internal string builder

                StringBuilder sb = strWriter.GetStringBuilder();

                sb.Insert(0, "Hey!! ");

                Console.WriteLine("-> {0}", sb.ToString());

                sb.Remove(0, "Hey!! ".Length);

                Console.WriteLine("-> {0}", sb.ToString());


                Console.WriteLine("******* Read string in memory using StringReader *************");

                using (StringReader strReader = new StringReader(strWriter.ToString()))
                {
                    string input = null;

                    while ((input = strReader.ReadLine()) != null)
                    {
                        Console.WriteLine(input);
                    }
                }



            }

            Console.ReadKey();
        }

        public static void Example11()
        {
            //Binary writer and read , extented directly from Object

            Console.WriteLine("****** Fun with Binary Writers / Readers ******\n");

            FileInfo f = new FileInfo(@"C:\KOUl\Data\BinFile.dat");

            using (BinaryWriter bw = new BinaryWriter(f.OpenWrite()))
            {
                Console.WriteLine("Base stream is: {0}", bw.BaseStream);

                //Save any type to the file.

                double aDouble = 1234.679;
                int anInt = 3456799;
                string aString = "A,B,C,D";
               
                //Write teh data

                bw.Write(aDouble);
                bw.Write(anInt);
                bw.Write(aString);
                
            }

            Console.WriteLine("***** Extracting the written data object ******");

            using (BinaryReader br = new BinaryReader(f.OpenRead()))
            {
                Console.WriteLine(br.ReadDouble());
                Console.WriteLine(br.ReadInt32());
                Console.WriteLine(br.ReadString());
            }

            Console.ReadKey();
        }

        public static void Example12()
        {
            Console.WriteLine("********** Demo Serilizing Object ******");
            UserPrefs userData = new UserPrefs();

            userData.WindowColor = "Yellow";
            userData.FontSize = 50;

            //Use Binary Formatter class 

            BinaryFormatter binFormat = new BinaryFormatter();

            //store object in file using formatter and stream

            using (Stream fsStream = new FileStream(@"C:\KOUl\Data\user.dat", FileMode.Create, FileAccess.Write,FileShare.None))
            {

                binFormat.Serialize(fsStream, userData);
            }

            Console.ReadKey();

        }


        public static void Example13()
        {
            Console.WriteLine("************* Fun with Object Serialization **************\n");

            JamesBondCar jbc = new JamesBondCar();

            jbc.canFly = true;
            jbc.canSubmerge = true;
            jbc.theRadio.stationPresets = new double[] { 89.3,105.1,97.1};
            jbc.theRadio.hasTweeters = true;

            //save as binaryformat into a file

            BinaryFormatter binFormat = new BinaryFormatter();

            using (Stream fstream = new FileStream(@"C:\KOUl\Data\CarData.dat", FileMode.Create, FileAccess.Write, FileShare.None))
            {
                binFormat.Serialize(fstream, jbc);

            }

            Console.WriteLine("=> Saved car in binary format !");

            using (Stream fstream = File.OpenRead(@"C:\KOUl\Data\CarData.dat"))
            {
                JamesBondCar carFromDisk =(JamesBondCar) binFormat.Deserialize(fstream);

                Console.WriteLine("Can this car fly ? : {0}", carFromDisk.canFly);

            }

            Console.ReadKey();
        }

        public static void Example14()
        {

            JamesBondCar jbc = new JamesBondCar();

            jbc.canFly = true;
            jbc.canSubmerge = true;
            jbc.theRadio.stationPresets = new double[] { 89.3, 105.1, 97.1 };
            jbc.theRadio.hasTweeters = true;

            SoapFormatter soapFormat = new SoapFormatter();

            using (Stream fstream = new FileStream(@"C:\KOUl\Data\CarData.soap", FileMode.Create, FileAccess.Write, FileShare.None))
            {
                soapFormat.Serialize(fstream, jbc);
            }

            Console.WriteLine("=> Saved in soap format");

            

            using (Stream fstream = File.OpenRead(@"C:\KOUl\Data\CarData.soap"))
            {
                JamesBondCar carFromDisk = (JamesBondCar)soapFormat.Deserialize(fstream);

                Console.WriteLine("Can this car fly ? : {0}", carFromDisk.canFly);

            }

            Console.ReadKey();

        }


        public static void Example15()
        {
            JamesBondCar jbc = new JamesBondCar();

            jbc.canFly = true;
            jbc.canSubmerge = true;
            jbc.theRadio.stationPresets = new double[] { 89.3, 105.1, 97.1 };
            jbc.theRadio.hasTweeters = true;

            XmlSerializer xmlFormat = new XmlSerializer(typeof(JamesBondCar));

            using (Stream fstream = new FileStream(@"C:\KOUl\Data\CarData.xml", FileMode.Create, FileAccess.Write, FileShare.None))
            {

                xmlFormat.Serialize(fstream, jbc);
            }

            Console.WriteLine("=> Saved in xml format");

            using (Stream fstream = File.OpenRead(@"C:\KOUl\Data\CarData.xml"))
            {
                JamesBondCar carFromDisk = (JamesBondCar)xmlFormat.Deserialize(fstream);

                Console.WriteLine("Can this car fly ? : {0}", carFromDisk.canFly);

            }


            Console.ReadKey();
        }

        public static void Example16()
        {

            StringData myData = new StringData();

            SoapFormatter soapFormat = new SoapFormatter();

            using (Stream fStream = new FileStream(@"C:\KOUl\Data\Customdata.soap", FileMode.Create, FileAccess.Write, FileShare.None))
            {
                soapFormat.Serialize(fStream, myData);
            }

            Console.WriteLine("**** Custom Serialization done ******");

            Console.ReadKey();

        }

        #region internal class

        [Serializable]
        public class UserPrefs
        {
            public string WindowColor;
            public int FontSize;

        }

        [Serializable]
        public class Radio
        {
            public bool hasTweeters;
            public bool hasSubWoofers;
            public double[] stationPresets; 
            [NonSerialized]
            public string radioID = "XF-552RR6";
        }

        [Serializable]
        public class Car
        {
            public Radio theRadio = new Radio();
            public bool isHatchBack;

        }

        [Serializable , XmlRoot(Namespace ="http://wwww.andry.com")]
        public class JamesBondCar : Car
        {
            [XmlAttribute]
            public bool canFly;
            [XmlAttribute]
            public bool canSubmerge;

            public JamesBondCar(bool skyWorthy, bool seaWorthy)
            {
                canFly = skyWorthy;
                canSubmerge = seaWorthy;
            }

            //xml serializer require default constructor

            public JamesBondCar()
            {

            }

        }

        [Serializable]
        class StringData : ISerializable
        {
            private string dataItemOne = "First data block";
            private string dataItemTwo = "More";

            public StringData()
            {

            }

           
            protected StringData(SerializationInfo si , StreamingContext context)
            {
                //populdate member variables from stream

                dataItemOne = si.GetString("First_Item").ToLower();
                dataItemTwo = si.GetString("dataItemTwo").ToLower();

            }

            public void GetObjectData(SerializationInfo info, StreamingContext context)
            {
                info.AddValue("First_Item", dataItemOne.ToUpper());
                info.AddValue("dataItemTwo", dataItemTwo.ToUpper());
            }
        }

        #endregion



    }
}
