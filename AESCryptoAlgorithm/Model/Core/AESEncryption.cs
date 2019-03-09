using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AESCryptoAlgorithm.Model
{
    public class AESEncryption
    {
        private const int KeySize = 16;
        private const int StateSize = 16;
        private readonly byte[,] state = new byte[4, 4];
        private const int Nr = 10;  
        private readonly byte[,] EK = new byte[44, 4];
        private readonly byte[,] K = new byte[4, 4];
        private readonly string strKey;
        public string result = "";
        public bool isHexKey = true;
        public bool isHexInput = true;
        public bool isHexOutput = true;

        public string Encrypt(string input, string password)
        {
            if (password.Length != 16) throw new Exception("Password is not 128bit long");

            result = "";

            return result;
        }
        public void Cipher(string strInput, bool encrypt)
        {
            byte[] inp = new byte[KeySize];

            //padding
            //string strInput must not contain a character '~'
            const string spaceAppend = "~                               ";
            const string spaceHexAppend = "7E202020202020202020202020202020";

            int ite; //number of iterations or 16-byte blocks of input
            string str; //appended input
            int someSize; //32 if hex & 16 if chars
            result = "";

            if (isHexInput)
                someSize = 32;
            else someSize = 16;

            ite = (int)Math.Ceiling((strInput.Length * 1.0) / someSize);

            for (int k = 0; k < ite; k++)
            {
                int someSize1 = strInput.Substring(k * someSize).Length;
                if (someSize1 < someSize)//appends the input string if not a perfect block
                {
                    if (isHexInput)
                        strInput += spaceHexAppend.Substring(0, someSize - someSize1);
                    else strInput += spaceAppend.Substring(0, someSize - someSize1);
                }
                str = strInput.Substring(k * someSize, someSize);

                //converts the string representation of Key to an array of Bytes
                if (isHexKey) //if key is a string representation of hex
                {
                    string strKey1 = strKey.ToUpper();
                    for (int i = 0; i < 4; i++)
                        for (int j = 0; j < 4; j++)
                        {
                            int sum = 0;
                            try { sum += int.Parse(strKey1[i * 8 + j * 2] + "") * 16; }
                            catch
                            {
                                int mysomeval = ((int)strKey1[i * 8 + j * 2] - 55) * 16;
                                if (mysomeval < 10 * 16 || mysomeval > 15 * 16)
                                    throw new SystemException("strKey: Hex code is not readable");
                                else sum += mysomeval;
                            }
                            try { sum += int.Parse(strKey1[i * 8 + j * 2 + 1] + ""); }
                            catch
                            {
                                int mysomeval = ((int)strKey1[i * 8 + j * 2 + 1] - 55);
                                if (mysomeval < 10 || mysomeval > 15)
                                    throw new SystemException("strKey: Hex code is not readable");
                                else sum += mysomeval;
                            }
                            K[i, j] = (byte)sum;
                        }
                }
                else
                {
                    string str1 = strKey;
                    for (int i = 0; i < 4; i++)
                        for (int j = 0; j < 4; j++)
                            K[i, j] = (byte)(str1[i * 4 + j]);
                }
                //end of conversion

                //converts the string representation of input string to an array of Bytes
                //the input string is now str, already appended
                if (!isHexInput)
                {
                    string str1 = str;
                    for (int i = 0; i < str1.Length; i++)
                        inp[i] = (byte)str1[i];

                }
                else
                {
                    string str1 = str.ToUpper();
                    for (int i = 0; i < 16; i++)
                    {
                        int sum = 0;
                        try { sum += int.Parse(str1[i * 2] + "") * 16; }
                        catch
                        {
                            int mysomeval = ((int)str1[i * 2] - 55) * 16;
                            if (mysomeval < 10 * 16 || mysomeval > 15 * 16)
                                throw new SystemException("str: Hex code is not readable");
                            else sum += mysomeval;
                        }
                        try { sum += int.Parse(str1[i * 2 + 1] + ""); }
                        catch
                        {
                            int mysomeval = ((int)str1[i * 2 + 1] - 55);
                            if (mysomeval < 10 || mysomeval > 15)
                                throw new SystemException("str: Hex code is not readable");
                            else sum += mysomeval;
                        }
                        inp[i] = (byte)sum;
                    }
                }
                //end of convertion

                if (encrypt)   ////////////////////////
                {
                    for (int i = 0; i < 4; i++)
                        for (int j = 0; j < 4; j++)
                            state[j, i] = inp[i * 4 + j];
                    expandKey();
                    AddRoundKey(0);
                    for (int rnd = 1; rnd < Nr; rnd++)
                    {
                        byteSub();
                        byteShift();
                        mixCol();
                        AddRoundKey(rnd);
                    }

                    byteSub();
                    byteShift();
                    AddRoundKey(Nr);
                }
                else //decrypt
                {
                    for (int i = 0; i < 4; i++)
                        for (int j = 0; j < 4; j++)
                            state[j, i] = inp[i * 4 + j];
                    expandKey();
                    AddRoundKey(Nr);
                    invByteShift();
                    invByteSub();
                    for (int rnd = Nr - 1; rnd > 0; rnd--)
                    {
                        AddRoundKey(rnd);
                        invMixCol();
                        invByteShift();
                        invByteSub();
                    }
                    AddRoundKey(0);
                }

                string strResult = "";
                byte[] getByte = new byte[16];
                for (int i = 0; i < 4; i++)
                    for (int j = 0; j < 4; j++)
                    {
                        getByte[i * 4 + j] = state[j, i];
                        strResult += (char)state[j, i];
                    }

                //REMOVES THE APPENDED STRING
                if (isHexOutput)
                {
                    result += BitConverter.ToString(getByte).Replace("-", "");
                    if (k == ite - 1 & !encrypt)
                    {
                        try { result = result.Substring(0, result.LastIndexOf("7E")); }
                        catch { }
                    }
                }
                else
                {
                    result += strResult;
                    if (k == ite - 1 & !encrypt)
                    {
                        try { result = result.Substring(0, result.LastIndexOf('~')); }
                        catch { }
                    }
                }
            }
        }
    }
}
