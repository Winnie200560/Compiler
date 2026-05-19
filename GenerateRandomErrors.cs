using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace comp
{
    public static class ErrorGenerator
    {
        public static void GenerateRandomErrors (TextPosition positionNow, string line, Action<byte, TextPosition> AddError)
        {
            Random rnd = new Random();
            if (rnd.Next(0, 100) > 20)
            {
                return;
            }

            int count = rnd.Next(1, 3);
            for (int i = 0; i < count; i++)
            {
                TextPosition p = new TextPosition();
                p.LineNumber = positionNow.LineNumber;
                if (line.Length > 1)
                {
                    p.CharNumber = (byte)rnd.Next(1, line.Length);
                }
                else
                {
                    p.CharNumber = 0;
                }
                byte errorCode = (byte)rnd.Next(201, 206);
                AddError(errorCode, p);
            }
        }
    }
}


