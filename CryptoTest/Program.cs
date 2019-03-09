using AESCryptoAlgorithm.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var aes = new AESEncryption();

            var enc = aes.Encrypt("This is a very long text to enumerate texts", "0cb7add6af7f6798", true);
            Console.WriteLine(enc);
            var res = aes.Encrypt(enc, "0cb7add6af7f6798", false);
            Console.WriteLine(res);
            Console.ReadLine();
        }
    }
}
