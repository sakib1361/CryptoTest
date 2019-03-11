using AESCryptoAlgorithm.Model;
using System;

namespace CryptoTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var aes = new AESEncryption();

            //var enc = aes.Encrypt("Simple text test", "0cb7add6af7f6798");
            //Console.WriteLine(enc);

            //var res = aes.Decrypt(enc, "0cb7add6af7f6798");
            //Console.WriteLine(res);

            //res = aes.Decrypt("ZTa5NYNsSdBDGe/3r91I1Q==", "0cb7add6af7f6798");

            //var enc = aes.Encrypt("Two One Nine Two", "Thats my Kung Fu");
            //Console.WriteLine(enc);

            //var res = aes.Decrypt(enc, "Thats my Kung Fu");
            //Console.WriteLine(res);


            Console.ReadLine();
        }
    }
}
