using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace   Exam70_483.Exercices.Security
{
    public class Course
    {
        public static void Launch()
        {
            // Example1();

            //Example2();

            // Example3();

            Example4();
        }

        public static void Example1()
        {
            string original = "My secret data!";
            byte[] encrypted;

            byte[] cypher;

            string message;

            //encrypt into a byte of array

            Console.WriteLine("Original Message :" + original);

            using (SymmetricAlgorithm symmetricAlgorithm = new AesManaged())
            {

                Console.WriteLine("Key  => : " + Encoding.Default.GetString(symmetricAlgorithm.Key, 0, symmetricAlgorithm.Key.Length));

                ICryptoTransform encryptor = symmetricAlgorithm.CreateEncryptor(symmetricAlgorithm.Key, symmetricAlgorithm.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt,encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(original);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }

                Console.WriteLine("Cyphered message => : " + Encoding.Default.GetString(encrypted, 0, encrypted.Length));
                //decript into plain message

                cypher = encrypted;



                ICryptoTransform decryptor = symmetricAlgorithm.CreateDecryptor(symmetricAlgorithm.Key, symmetricAlgorithm.IV);

                using (MemoryStream msDecrypt = new MemoryStream(cypher))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            message = srDecrypt.ReadToEnd();
                        }

                    }
                }
            }


            Console.WriteLine("Decrypted Message => :" + message);


            Console.ReadKey();

        }

        public static void Example2()
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();

            string publicKeyXML = rsa.ToXmlString(false);
            string privateKeyXML = rsa.ToXmlString(true);

            Console.WriteLine(" ************** Public Key ********************");

            Console.WriteLine(publicKeyXML);


            Console.WriteLine(" *************** Private Key *****************");

            Console.WriteLine(privateKeyXML);

            string original = "My secret Data !";
            UnicodeEncoding byteConverter = new UnicodeEncoding();

            byte[] dataToEncrypt = byteConverter.GetBytes(original);

            byte[] encryptedData;

            using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
            {
                RSA.FromXmlString(publicKeyXML);

                encryptedData = RSA.Encrypt(dataToEncrypt, false);
            }

            byte[] decryptedData;

            using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
            {
                RSA.FromXmlString(privateKeyXML);
                decryptedData = RSA.Decrypt(encryptedData, false);
            }

            string decryptedString = byteConverter.GetString(decryptedData);

            Console.WriteLine("Decrypted string : => " + decryptedString);


           




            Console.ReadKey();
        }

        public static void Example3()
        {
            //hask code
            UnicodeEncoding byteConverter = new UnicodeEncoding();
            SHA256 sHA256 = SHA256.Create();

            string data = "A paragraph of text";
            byte[] hashA = sHA256.ComputeHash(byteConverter.GetBytes(data));

            data = "A paragraph of changed text";

            byte[] hashB = sHA256.ComputeHash(byteConverter.GetBytes(data));

            data = "A paragraph of text";

            byte[] hashC = sHA256.ComputeHash(byteConverter.GetBytes(data));


            Console.WriteLine(hashA.SequenceEqual(hashB));//false
            Console.WriteLine(hashA.SequenceEqual(hashC)); // true


            Console.ReadKey();

        }

        /// <summary>
        /// 
        ///   makecert -n “CN=WouterDeKort” -sr currentuser -ss testCertStore
        /// 
        /// </summary>
        public static void Example4()
        {
            string textToSign = "Test paragraph";

            byte[] signature = Sign(textToSign, "cn=WouterDeKort");

            //Uncomment this line to make the verification step fail 
            //signature[0] = 0;

            Console.WriteLine(Verify(textToSign, signature));

            Console.ReadKey();
        }

        private static byte[] Sign(string text, string certSubject)
        {
            X509Certificate2 cert = GetCertificate();

            var csp = (RSACryptoServiceProvider)cert.PrivateKey;

            byte[] hash = HashData(text);

            return csp.SignHash(hash, CryptoConfig.MapNameToOID("SHA1"));
        }
       

        private static bool Verify(string text , byte[] signature)
        {
            X509Certificate2 cert = GetCertificate();

            var csp =(RSACryptoServiceProvider) cert.PublicKey.Key;

            byte[] hash = HashData(text);

            return csp.VerifyHash(hash, CryptoConfig.MapNameToOID("SHA1"), signature);
        }

        private static byte[] HashData(string text)
        {
            HashAlgorithm hashAlgorithm = new SHA1Managed();

            UnicodeEncoding encoding = new UnicodeEncoding();

            byte[] data = encoding.GetBytes(text);

            byte[] hash = hashAlgorithm.ComputeHash(data);

            return hash;
        }
        private static X509Certificate2 GetCertificate()
        {
            X509Store certificateStore = new X509Store("testCertStore", StoreLocation.CurrentUser);

            certificateStore.Open(OpenFlags.ReadOnly);

            var certificate = certificateStore.Certificates[0];

            return certificate;
        }
    }
}
