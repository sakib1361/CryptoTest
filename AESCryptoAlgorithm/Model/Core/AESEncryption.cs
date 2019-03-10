using AESCryptoAlgorithm.Engine;
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
            var data = Encoding.UTF8.GetBytes(plainText);

            if (data.Length % blockSize == 0)
                data = data.Concat(ByteFactory(blockSize, blockSize)).ToArray();
            else
            {
                var remainder = blockSize - data.Length % blockSize;
                data = data.Concat(ByteFactory((byte)remainder, remainder)).ToArray();
            }

            var encByte = Process(data, password, true);
            return Convert.ToBase64String(encByte);
        }

        private byte[] ByteFactory(byte val, int count)
        {
            var b = new byte[count];
            for (int no = 0; no < count; no++) b[no] = val;
            return b;
        }

        public string Decrypt(string base64Text, string password)
        {
            var data = Convert.FromBase64String(base64Text);
            var decByte = Process(data, password, false);
            var lastByte = (int)decByte.LastOrDefault();
            decByte = decByte.Take(decByte.Length - lastByte).ToArray();
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
                    {
                        currentByteBlock[j, i] = byteInput[blockStart + i * 4 + j];
                    }
                LogEngine.LogBytes(currentByteBlock, "Current Block");
                if (isEncrypt)
                {
                    addKey.AddRoundKey(0, currentByteBlock);
                    LogEngine.LogBytes(currentByteBlock, "Current Block , Round" + 0);
                    for (int rnd = 1; rnd < Nr; rnd++)
                    {
                        substitute.ByteSubstitute(currentByteBlock);
                        LogEngine.LogBytes(currentByteBlock, "Substitute Byte");
                        shiftRow.ByteShift(currentByteBlock);
                        LogEngine.LogBytes(currentByteBlock, "Shift Row");
                        mixColumn.ApplyColumn(currentByteBlock);
                        LogEngine.LogBytes(currentByteBlock, "Mix Column");
                        addKey.AddRoundKey(rnd, currentByteBlock);
                        LogEngine.LogBytes(currentByteBlock, "Current Block , Round" + rnd);
                    }
                    substitute.ByteSubstitute(currentByteBlock);
                    shiftRow.ByteShift(currentByteBlock);
                    addKey.AddRoundKey(Nr, currentByteBlock);

                    LogEngine.LogBytes(currentByteBlock, "Current Block , Round" + Nr);
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
