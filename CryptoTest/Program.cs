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

            res = aes.Decrypt("PEfUMhJ0UcYMelSQ7CsxvhY/ZMB/3Fil5phz7aQ+O7vjC68D1m3Wx3bKR5KPAVlV",
                "1234567812345678");
            Console.WriteLine(res);


            Console.ReadLine();
        }
    }
}
