using AESCryptoAlgorithm.Model.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AESCryptoAlgorithm.Model
{
    public class AESEncryption
    {
        private const int Nr = 10;
        private const int blockSize = 16;

        public string Encrypt(string plainText, string password)
        {
            if (plainText.Length % blockSize != 0)
                plainText += new string(' ', blockSize - plainText.Length % blockSize);
            var data = Encoding.UTF8.GetBytes(plainText);
            var encByte = Process(data, password, true);
            return Convert.ToBase64String(encByte);
        }

        public string Decrypt(string base64Text, string password)
        {
            var data = Convert.FromBase64String(base64Text);
            var decByte = Process(data, password, false);
            return Encoding.UTF8.GetString(decByte).Trim();
        }

        private byte[] Process(byte[] byteInput, string password, bool isEncrypt)
        {
            if (password.Length != blockSize) throw new Exception("Password is not 128bit long");

            var result = new List<byte>();
            //We will divide the byteInput into Block of 16 chars
            var blockIteration = (int)Math.Ceiling((byteInput.Length * 1.0) / blockSize);
            var currentByteBlock = new byte[4, 4];


            var substitute = new Substitution();
            var addKey = new AddKey(substitute, password);
            var mixColumn = new MixColumns();
            var shiftRow = new ShiftRow();

            for (var blockCount = 0; blockCount < blockIteration; blockCount++)
            {
                var blockStart = blockCount * blockSize;
                //COnvert to a blockArray of 4x4
                for (int i = 0; i < 4; i++)
                    for (int j = 0; j < 4; j++)
                        currentByteBlock[j, i] = byteInput[blockStart + i * 4 + j];

                if (isEncrypt)
                {
                    addKey.AddRoundKey(0, currentByteBlock);
                    for (int rnd = 1; rnd < Nr; rnd++)
                    {
                        substitute.ByteSubstitute(currentByteBlock);
                        shiftRow.ByteShift(currentByteBlock);
                        mixColumn.ApplyColumn(currentByteBlock);
                        addKey.AddRoundKey(rnd, currentByteBlock);
                    }
                    substitute.ByteSubstitute(currentByteBlock);
                    shiftRow.ByteShift(currentByteBlock);
                    addKey.AddRoundKey(Nr, currentByteBlock);
                }
                else
                {
                    addKey.AddRoundKey(Nr, currentByteBlock);
                    shiftRow.InvByteShift(currentByteBlock);
                    substitute.InvByteSub(currentByteBlock);

                    for (int rnd = Nr - 1; rnd > 0; rnd--)
                    {
                        addKey.AddRoundKey(rnd, currentByteBlock);
                        mixColumn.InvApplyColumn(currentByteBlock);
                        shiftRow.InvByteShift(currentByteBlock);
                        substitute.InvByteSub(currentByteBlock);
                    }
                    addKey.AddRoundKey(0, currentByteBlock);
                }
                
                byte[] getByte = new byte[16];
                for (int i = 0; i < 4; i++)
                    for (int j = 0; j < 4; j++)
                    {
                        result.Add(currentByteBlock[j, i]);
                    }

            }
            return result.ToArray();
        }

        private byte[] GetInput(string input, int blockSize)
        {

            var padAdd = 0;
            var byteList = new List<byte>();

            if (input.Length % blockSize == 0)
                padAdd = blockSize;
            else padAdd = blockSize - input.Length % blockSize;


            var paddByte = new byte[padAdd];
            for (int i = 0; i < paddByte.Length; i++)
            {
                paddByte[i] = 0x20;
            }
            var charByte = input.ToCharArray();
            foreach(var item in charByte)
            {
                byteList.Add((byte)(item));
            }
            byteList.AddRange(paddByte);

            return byteList.ToArray();
        }
    }
}
