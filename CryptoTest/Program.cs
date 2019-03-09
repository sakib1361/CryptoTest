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

            var enc = aes.Encrypt("Simple text test", "0cb7add6af7f6798");
            Console.WriteLine(enc);

            var res = aes.Decrypt(enc, "0cb7add6af7f6798");
            Console.WriteLine(res);

            res = aes.Decrypt("q551elT6GfBsv0sjEGfE0Y6tOGcgZNcWebEDmAcfcfg=", "0cb7add6af7f6798");
            Console.WriteLine(res);
            Console.ReadLine();
        }
    }
}
