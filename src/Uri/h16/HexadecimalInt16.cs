﻿using System.Globalization;
using Txt.ABNF;

namespace Uri.h16
{
    public class HexadecimalInt16 : Repetition
    {
        public HexadecimalInt16(Repetition repetition)
            : base(repetition)
        {
        }

        public byte[] GetBytes()
        {
            var bytes = new byte[2];
            int ix = 1;
            var hex = Text;
            for (int i = hex.Length; (i > 0) && (ix >= 0); i -= 2, ix--)
            {
                int subLength = i == 1 ? 1 : 2;
                var substr = hex.Substring(i - subLength, subLength);
                bytes[ix] = byte.Parse(substr, NumberStyles.HexNumber);
            }

            return bytes;
        }
    }
}