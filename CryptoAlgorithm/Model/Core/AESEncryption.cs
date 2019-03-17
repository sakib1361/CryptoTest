using CryptoAlgorithm.Engine;
using CryptoAlgorithm.Model.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CryptoAlgorithm.Model
{
    public class AESEncryption : IEncryption
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

            var encByte = ProcessEncrypt(data, password);
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
            var decByte = ProcessDecrypt(data, password);
            var lastByte = (int)decByte.LastOrDefault();
            decByte = decByte.Take(decByte.Length - lastByte).ToArray();
            return Encoding.UTF8.GetString(decByte).Trim();
        }

        private byte[] ProcessEncrypt(byte[] byteInput, string password)
        {
            if (password.Length != blockSize) throw new Exception("Password is not 128bit long");

            var result = new List<byte>();
            //We will divide the byteInput into Block of 16 chars
            var blockIteration = (int)Math.Ceiling((byteInput.Length * 1.0) / blockSize);
            var currentByteBlock = new byte[4, 4];
            var cbcBlock = new byte[4, 4];
            BlockFill(cbcBlock, 0);

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
                        currentByteBlock[j, i] = (byte)(cbcBlock[j, i] ^ byteInput[blockStart + i * 4 + j]);
                    }

                LogEngine.LogBytes(currentByteBlock, "Current Block");

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

                for (int i = 0; i < 4; i++)
                    for (int j = 0; j < 4; j++)
                    {
                        result.Add(currentByteBlock[j, i]);
                    }
                BlockCopy(currentByteBlock, cbcBlock);

            }

            addKey.Dispose();
            return result.ToArray();
        }

        private byte[] ProcessDecrypt(byte[] byteInput, string password)
        {
            if (password.Length != blockSize) throw new Exception("Password is not 128bit long");

            var result = new List<byte>();
            //We will divide the byteInput into Block of 16 chars
            var blockIteration = (int)Math.Ceiling((byteInput.Length * 1.0) / blockSize);
            var currentByteBlock = new byte[4, 4];
            var cbcBlock = new byte[4, 4];
            BlockFill(cbcBlock, 0);

            var substitute = new Substitution();
            var addKey = new AddKey(substitute, password);
            var mixColumn = new MixColumns();
            var shiftRow = new ShiftRow();

            for (var blockCount = 0; blockCount < blockIteration; blockCount++)
            {
                var blockStart = blockCount * blockSize;
                var cipherCBC = new byte[4, 4];
                //COnvert to a blockArray of 4x4
                for (int i = 0; i < 4; i++)
                    for (int j = 0; j < 4; j++)
                    {
                        currentByteBlock[j, i] = byteInput[blockStart + i * 4 + j];
                        cipherCBC[j, i] = currentByteBlock[j, i];
                    }

                LogEngine.LogBytes(currentByteBlock, "Current Block");

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


                for (int i = 0; i < 4; i++)
                    for (int j = 0; j < 4; j++)
                    {
                        result.Add((byte)(cbcBlock[j, i] ^ currentByteBlock[j, i]));
                    }
                //Copy this stage Cypher as next stage CBC
                BlockCopy(cipherCBC, cbcBlock);
            }
            addKey.Dispose();
            return result.ToArray();
        }

        private void BlockFill(byte[,] target,byte value)
        {
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                {
                    target[i, j] = value;
                }
        }
        private void BlockCopy(byte[,] source, byte[,] target)
        {
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                {
                    target[i, j] = source[i, j];
                }
        }
    }
}
