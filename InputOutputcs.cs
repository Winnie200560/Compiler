using System;
using System.Collections.Generic;
using System.IO;

namespace Comp
{
    struct TextPosition
    {
        public uint lineNumber;
        public byte charNumber;

        public TextPosition(uint ln = 0, byte c = 0)
        {
            lineNumber = ln;
            charNumber = c;
        }
    }
    struct Err
    {
        public TextPosition errorPosition;
        public byte errorCode;

        public Err(TextPosition errorPosition, byte errorCode)
        {
            this.errorPosition = errorPosition;
            this.errorCode = errorCode;
        }
    }

    class InputOutput
    {
        const byte ERRMAX = 9;
        public static char Ch { get; set; }
        public static TextPosition positionNow = new TextPosition();
        static string line;
        static byte lastInLine = 0;
        public static List<Err> err;
        static StreamReader File { get; set; }
        static uint errCount = 0;

        static public void NextCh()
        {
            if (positionNow.charNumber == lastInLine)
            {
                ListThisLine();
                GenerateRandomErrors();
                if (err.Count > 0)
                {
                    ListErrors();
                }
                ReadNextLine();
                positionNow.lineNumber = positionNow.lineNumber + 1;
                positionNow.charNumber = 0;
            }
            else
            {
                ++positionNow.charNumber;
            }
            if (line != null && positionNow.charNumber < line.Length)
            {
                Ch = line[positionNow.charNumber];
            }
        }
        private static void ListThisLine()
        {
            Console.WriteLine(line);
        }

        private static void ReadNextLine()
        {
            if (!File.EndOfStream)
            {
                line = File.ReadLine();
                err = new List<Err>();

                if (line.Length > 0)
                {
                    lastInLine = (byte)(line.Length - 1);
                }
                else
                {
                    lastInLine = 0;
                }
            }
            else
            {
                End();
            }
        }

        static void End()
        {
            Console.WriteLine($"Компиляция завершена: : ошибок — {errCount}!");
            Environment.Exit(0);
        }

        static void ListErrors()
        {
            int pos = 6 - $"{positionNow.lineNumber} ".Length;
            string s = "";
            foreach (Err item in err)
            {
                ++errCount;
                s = "**";
                if (errCount < 10)
                {
                    s += "0";
                }
                s += $"{errCount}**";
                while (s.Length - 1 < pos + item.errorPosition.charNumber)
                {
                    s += " ";
                }
                s += $"^ ошибка {item.errorCode}: {errorTable[item.errorCode]}";
                Console.WriteLine(s);
            }
        }

        static public void Error(byte errorCode, TextPosition position)
        {
            Err e;
            if (err.Count <= ERRMAX)
            {
                e = new Err(position, errorCode);
                err.Add(e);
            }
        }

        static public void ReadFile(string fileName)
        {
            File = new StreamReader(fileName);
            err = new List<Err>();
            if (!File.EndOfStream)
            {
                line = File.ReadLine();
                positionNow.lineNumber = 1;
                positionNow.charNumber = 0;
                lastInLine = (byte)(line.Length - 1);
                Ch = line[0];
            }
            else
            {
                Console.WriteLine("Пустой файл");
                End();
            }
        }

        static Random rnd = new Random();

        static public void GenerateRandomErrors()
        {
            if (rnd.Next(0, 100) > 20)
            {
                return;
            }

            int count = rnd.Next(1, 3);
            for (int i = 0; i < count; i++)
            {
                TextPosition p = new TextPosition();
                p.lineNumber = positionNow.lineNumber;

                if (line.Length > 1)
                {
                    p.charNumber = (byte)rnd.Next(1, line.Length);
                }
                else
                {
                    p.charNumber = 0;
                }
                byte errorCode = (byte)rnd.Next(201, 206);
                Error(errorCode, p);
            }
        }

        static Dictionary<byte, string> errorTable = new Dictionary<byte, string>()
        {
                {201, "неизвестный символ"},
                {202, "ожидался символ"},
                {203, "слишком большое число"},
                {204, "не закрытая скобка"},
                {205, "неверный идентификатор"}
        };
    }
}