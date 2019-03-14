using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoAlgorithm.Model
{
    public interface IEncryption
    {
        string Encrypt(string plainText, string password);
        string Decrypt(string base64Text, string password);
    }
}
