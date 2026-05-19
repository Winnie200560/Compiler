using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace comp
{
    public struct TextPosition
    {
        private uint _lineNumber;
        private byte _charNumber;

        public TextPosition(uint lineNumber, byte charNumber)
        {
            _lineNumber = lineNumber;
            _charNumber = charNumber;
        }

        public uint LineNumber
        {
            get
            {
                return _lineNumber;
            }
            set
            {
                _lineNumber = value;
            }
        }

        public byte CharNumber
        {
            get
            {
                return _charNumber;
            }
            set
            {
                _charNumber = value;
            }
        }
    }
}
