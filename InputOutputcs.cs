using comp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Comp
{

    class InputOutput
    {
        const byte ErrMax = 9;

        private static StreamReader _reader;
        private static string _line;
        private static int _lastInLine;
        private static List<Err> _errors;
        private static uint _errCount;
        private static TextPosition _positionNow;
        private static char _ch;

        static InputOutput()
        {
            _lastInLine = 0;
            _errCount = 0;
            _positionNow = new TextPosition();
            _errors = new List<Err>();
        }

        public static char Ch
        {
            get
            {
                return _ch;
            }
            private set
            {
                _ch = value;
            }
        }

        public static TextPosition PositionNow
        {
            get
            {
                return _positionNow;
            }
            private set
            {
                _positionNow = value;
            }
        }

        public static List<Err> Errors
        {
            get
            {
                return _errors;
            }
        }

        public static void NextCh()
        {
            if (_positionNow.CharNumber == _lastInLine)
            {
                ListThisLine();
                ErrorGenerator.GenerateRandomErrors(_positionNow,_line,AddError);
                if (_errors.Count > 0)
                {
                    ListErrors();
                }
                ReadNextLine();
                _positionNow.LineNumber = _positionNow.LineNumber + 1;
                _positionNow.CharNumber = 0;
            }
            else
            {
                ++_positionNow.CharNumber;
                Ch = _line[_positionNow.CharNumber];
            }
        }

        public static void ListThisLine()
        {
            Console.WriteLine(_line);
        }

        public static void ReadNextLine()
        {
            if (!_reader.EndOfStream)
            {
                _line = _reader.ReadLine();
                _errors = new List<Err>();

                if (_line.Length > 0)
                {
                    _lastInLine = (byte)(_line.Length - 1);
                }
                else
                {
                    _lastInLine = 0;
                }
            }
            else
            {
                End();
            }
        }

        public static void End()
        {
            Console.WriteLine($"Компиляция завершена: : ошибок — {_errCount}!");
            Environment.Exit(0);
        }

        public static void ListErrors()
        {
            int pos = 6 - $"{_positionNow.LineNumber} ".Length;
            string s = "";
            foreach (Err item in _errors)
            {
                ++_errCount;
                s = "**";
                if (_errCount < 10)
                {
                    s += "0";
                }
                s += $"{_errCount}**";
                while (s.Length - 1 < pos + item.ErrorPosition.CharNumber)
                {
                    s += " ";
                }
                s += $"^ ошибка {item.ErrorCode}: {ErrorTable.Error[item.ErrorCode]}";
                Console.WriteLine(s);
            }
        }

        public static void AddError(byte errorCode, TextPosition position)
        {
            Err e;
            if (_errors.Count <= ErrMax)
            {
                e = new Err(position, errorCode);
                _errors.Add(e);
            }
        }

        public static void ReadFile(string fileName)
        {
            if (!File.Exists(fileName))
            {
                Console.WriteLine("Файл не найден!");
                return;
            }
            _reader = new StreamReader(fileName);
            _errors = new List<Err>();
            if (!_reader.EndOfStream)
            {
                _line = _reader.ReadLine();
                _positionNow.LineNumber = 1;
                _positionNow.CharNumber = 0;
                _lastInLine = (byte)(_line.Length - 1);
                Ch = _line[0];
            }
            else
            {
                Console.WriteLine("Пустой файл");
                End();
            }
        }

    }
}