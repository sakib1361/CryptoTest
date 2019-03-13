using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoAlgorithm.Engine
{
    class LogEngine
    {
        public static void LogBytes(byte[] data, string name)
        {
#if DEBUG
            Console.WriteLine("\n" + name);
            for (int col = 0; col < 4; col++)
            {
                Console.Write(string.Format(" {0}", data[col].ToString("X2")));
            }
            Console.WriteLine();
#endif
        }

        public static void LogBytes(byte[,] data, string name)
        {
#if DEBUG
            Console.WriteLine("\n" + name);
            for (int row = 0; row < 4; row++)
            {
                for (int col = 0; col < 4; col++)
                {
                    Console.Write(string.Format(" {0}", data[row, col].ToString("X2")));
                }
                Console.WriteLine();
            }
#endif
        }
    }
}
