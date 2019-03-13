using CryptoAlgorithm.Model;
using System;

namespace CryptoTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var aes = new AESEncryption();

            var enc = aes.Encrypt("Simple text test pad", "0cb7add6af7f6798");
            Console.WriteLine(enc);

            var res = aes.Decrypt(enc, "0cb7add6af7f6798");
            Console.WriteLine(res);

            res = aes.Decrypt("IFyZBTM2aYSkjl82GU/+r7Vlgk9xJJqJtHzufk9p9og=", "1234567812345678");
            Console.WriteLine(res);


            Console.ReadLine();
        }
    }
}
