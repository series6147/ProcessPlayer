using System.IO;

namespace ProcessPlayer.Data.Expressions
{
    public class PegByteParser : PegBaseParser
    {
        #region protected variables

        protected byte[] _src;

        #endregion

        #region private methods

        private void LogOutMsg(string errKind, string msg)
        {
            _errOut.WriteLine("<{0}>{1}: {2}", _pos, errKind, msg);
            _errOut.Flush();
        }

        #endregion

        #region public methods

        public bool Bit(int bitNo, byte toMatch)
        {
            if (_pos < _srcLen && ((_src[_pos] >> (bitNo - 1)) & 1) == toMatch)
            {
                _pos++;

                return true;
            }

            return false;
        }

        public bool Bits(int lowBitNo, int highBitNo, byte toMatch)
        {
            if (_pos < _srcLen && ((_src[_pos] >> (lowBitNo - 1)) & ((1 << highBitNo) - 1)) == toMatch)
            {
                _pos++;

                return true;
            }

            return false;
        }

        public bool Bits(int lowBitNo, int highBitNo, ByteSetData toMatch)
        {
            if (_pos < _srcLen)
            {
                var value = (byte)((_src[_pos] >> (lowBitNo - 1)) & ((1 << highBitNo) - 1));

                _pos++;

                return toMatch.Matches(value);
            }

            return false;
        }

        public bool BitsInto(int lowBitNo, int highBitNo, out int into)
        {
            if (_pos < _srcLen)
            {
                into = (_src[_pos] >> (lowBitNo - 1)) & ((1 << highBitNo) - 1);

                _pos++;

                return true;
            }

            into = 0;

            return false;
        }
        public bool BitsInto(int lowBitNo, int highBitNo, ByteSetData toMatch, out int into)
        {
            if (_pos < _srcLen)
            {
                var value = (byte)((_src[_pos] >> (lowBitNo - 1)) & ((1 << highBitNo) - 1));

                _pos++;

                into = value;

                return toMatch.Matches(value);
            }

            into = 0;

            return false;
        }

        public bool Char(byte c1)
        {
            if (_pos < _srcLen && _src[_pos] == c1)
            {
                _pos++;

                return true;
            }

            return false;
        }
        public bool Char(byte c1, byte c2)
        {
            if (_pos + 1 < _srcLen && _src[_pos] == c1 && _src[_pos + 1] == c2)
            {
                _pos += 2;

                return true;
            }

            return false;
        }
        public bool Char(byte c1, byte c2, byte c3)
        {
            if (_pos + 2 < _srcLen && _src[_pos] == c1 && _src[_pos + 1] == c2 && _src[_pos + 2] == c3)
            {
                _pos += 3;

                return true;
            }

            return false;
        }
        public bool Char(byte c1, byte c2, byte c3, byte c4)
        {
            if (_pos + 3 < _srcLen && _src[_pos] == c1 && _src[_pos + 1] == c2 && _src[_pos + 2] == c3 && _src[_pos + 3] == c4)
            {
                _pos += 4;

                return true;
            }

            return false;
        }
        public bool Char(byte c1, byte c2, byte c3, byte c4, byte c5)
        {
            if (_pos + 4 < _srcLen && _src[_pos] == c1 && _src[_pos + 1] == c2 && _src[_pos + 2] == c3 && _src[_pos + 3] == c4 && _src[_pos + 4] == c5)
            {
                _pos += 5;

                return true;
            }

            return false;
        }
        public bool Char(byte c1, byte c2, byte c3, byte c4, byte c5, byte c6)
        {
            if (_pos + 5 < _srcLen && _src[_pos] == c1 && _src[_pos + 1] == c2 && _src[_pos + 2] == c3 && _src[_pos + 3] == c4 && _src[_pos + 4] == c5 && _src[_pos + 5] == c6)
            {
                _pos += 6;

                return true;
            }

            return false;
        }
        public bool Char(byte c1, byte c2, byte c3, byte c4, byte c5, byte c6, byte c7)
        {
            if (_pos + 6 < _srcLen && _src[_pos] == c1 && _src[_pos + 1] == c2 && _src[_pos + 2] == c3 && _src[_pos + 3] == c4 && _src[_pos + 4] == c5 && _src[_pos + 5] == c6 && _src[_pos + 6] == c7)
            {
                _pos += 7;

                return true;
            }

            return false;
        }
        public bool Char(byte c1, byte c2, byte c3, byte c4, byte c5, byte c6, byte c7, byte c8)
        {
            if (_pos + 7 < _srcLen && _src[_pos] == c1 && _src[_pos + 1] == c2 && _src[_pos + 2] == c3 && _src[_pos + 3] == c4 && _src[_pos + 4] == c5 && _src[_pos + 5] == c6 && _src[_pos + 6] == c7 && _src[_pos + 7] == c8)
            {
                _pos += 8;

                return true;
            }

            return false;
        }
        public bool Char(byte[] s)
        {
            int length = s.Length;

            if (_pos + length > _srcLen)
                return false;

            for (int i = 0; i < length; ++i)
                if (s[i] != _src[_pos + i])
                    return false;

            _pos += length;

            return true;
        }

        public void Construct(byte[] src, TextWriter errOut)
        {
            Construct(errOut);

            SetSource(src);
        }

        public virtual bool SyntaxError(string sMsg)
        {

            LogOutMsg("Syntax error", sMsg);

            throw new PegException("", "Syntax error", sMsg, 0, 0);
        }

        public byte[] GetSource()
        {
            return _src;
        }

        public bool IChar(byte c1)
        {
            if (_pos < _srcLen && ToUpper(_src[_pos]) == c1)
            {
                _pos++;

                return true;
            }

            return false;
        }
        public bool IChar(byte c1, byte c2)
        {
            if (_pos + 1 < _srcLen && ToUpper(_src[_pos]) == ToUpper(c1) && ToUpper(_src[_pos + 1]) == ToUpper(c2))
            {
                _pos += 2;

                return true;
            }

            return false;
        }
        public bool IChar(byte c1, byte c2, byte c3)
        {
            if (_pos + 2 < _srcLen && ToUpper(_src[_pos]) == ToUpper(c1) && ToUpper(_src[_pos + 1]) == ToUpper(c2) && ToUpper(_src[_pos + 2]) == ToUpper(c3))
            {
                _pos += 3;

                return true;
            }

            return false;
        }
        public bool IChar(byte c1, byte c2, byte c3, byte c4)
        {
            if (_pos + 3 < _srcLen && ToUpper(_src[_pos]) == ToUpper(c1) && ToUpper(_src[_pos + 1]) == ToUpper(c2) && ToUpper(_src[_pos + 2]) == ToUpper(c3) && ToUpper(_src[_pos + 3]) == ToUpper(c4))
            {
                _pos += 4;

                return true;
            }

            return false;
        }
        public bool IChar(byte c1, byte c2, byte c3, byte c4, byte c5)
        {
            if (_pos + 4 < _srcLen && ToUpper(_src[_pos]) == ToUpper(c1) && ToUpper(_src[_pos + 1]) == ToUpper(c2) && ToUpper(_src[_pos + 2]) == ToUpper(c3) && ToUpper(_src[_pos + 3]) == ToUpper(c4) && ToUpper(_src[_pos + 4]) == ToUpper(c5))
            {
                _pos += 5;

                return true;
            }

            return false;
        }
        public bool IChar(byte c1, byte c2, byte c3, byte c4, byte c5, byte c6)
        {
            if (_pos + 5 < _srcLen && ToUpper(_src[_pos]) == ToUpper(c1) && ToUpper(_src[_pos + 1]) == ToUpper(c2) && ToUpper(_src[_pos + 2]) == ToUpper(c3) && ToUpper(_src[_pos + 3]) == ToUpper(c4) && ToUpper(_src[_pos + 4]) == ToUpper(c5) && ToUpper(_src[_pos + 5]) == ToUpper(c6))
            {
                _pos += 6;

                return true;
            }

            return false;
        }
        public bool IChar(byte c1, byte c2, byte c3, byte c4, byte c5, byte c6, byte c7)
        {
            if (_pos + 6 < _srcLen && ToUpper(_src[_pos]) == ToUpper(c1) && ToUpper(_src[_pos + 1]) == ToUpper(c2) && ToUpper(_src[_pos + 2]) == ToUpper(c3) && ToUpper(_src[_pos + 3]) == ToUpper(c4) && ToUpper(_src[_pos + 4]) == ToUpper(c5) && ToUpper(_src[_pos + 5]) == ToUpper(c6) && ToUpper(_src[_pos + 6]) == ToUpper(c7))
            {
                _pos += 7;

                return true;
            }

            return false;
        }
        public bool IChar(byte[] s)
        {
            int length = s.Length;

            if (_pos + length > _srcLen)
                return false;

            for (int i = 0; i < length; ++i)
                if (s[i] != ToUpper(_src[_pos + i]))
                    return false;

            _pos += length;

            return true;
        }

        public bool In(byte c0, byte c1)
        {
            if (_pos < _srcLen && _src[_pos] >= c0 && _src[_pos] <= c1)
            {
                _pos++;

                return true;
            }

            return false;
        }
        public bool In(byte c0, byte c1, byte c2, byte c3)
        {
            if (_pos < _srcLen)
            {
                byte c = _src[_pos];

                if (c >= c0 && c <= c1 || c >= c2 && c <= c3)
                {
                    _pos++;

                    return true;
                }
            }

            return false;
        }
        public bool In(byte c0, byte c1, byte c2, byte c3, byte c4, byte c5)
        {
            if (_pos < _srcLen)
            {
                byte c = _src[_pos];

                if (c >= c0 && c <= c1 || c >= c2 && c <= c3 || c >= c4 && c <= c5)
                {
                    _pos++;

                    return true;
                }
            }

            return false;
        }
        public bool In(byte c0, byte c1, byte c2, byte c3, byte c4, byte c5, byte c6, byte c7)
        {
            if (_pos < _srcLen)
            {
                byte c = _src[_pos];

                if (c >= c0 && c <= c1 || c >= c2 && c <= c3 || c >= c4 && c <= c5 || c >= c6 && c <= c7)
                {
                    _pos++;

                    return true;
                }
            }

            return false;
        }
        public bool In(byte[] s)
        {
            if (_pos < _srcLen)
            {
                byte c = _src[_pos];

                for (int i = 0; i < s.Length - 1; i += 2)
                {
                    if (c >= s[i] && c <= s[i + 1])
                    {
                        _pos++;

                        return true;
                    }
                }
            }

            return false;
        }

        public bool Into(delMatcher toMatch, out byte[] into)
        {
            int pos = _pos;

            if (toMatch())
            {
                int nLen = _pos - pos;

                into = new byte[nLen];

                for (int i = 0; i < nLen; ++i)
                    into[i] = _src[i + pos];

                return true;
            }
            
            @into = null;

            return false;
        }

        public bool Into(delMatcher toMatch, out double into)
        {
            byte[] s;

            into = 0.0;

            if (!Into(toMatch, out s))
                return false;

            System.Text.Encoding encoding = System.Text.Encoding.UTF8;

            string asString = encoding.GetString(s, 0, s.Length);

            if (!System.Double.TryParse(asString, out into))
                return false;
            return true;
        }
        public bool Into(delMatcher toMatch, out int into)
        {
            byte[] s;

            into = 0;

            if (!Into(toMatch, out s))
                return false;

            into = 0;

            for (int i = 0; i < s.Length; ++i)
            {
                into <<= 8;
                into |= s[i];
            }

            return true;
        }

        public bool Into(delMatcher toMatch, out PegBegEnd begEnd)
        {
            bool matches = toMatch();

            begEnd.posBeg = _pos;
            begEnd.posEnd = _pos;

            return matches;
        }

        public bool IntoBits(int lowBitNo, int highBitNo, out int val)
        {
            return BitsInto(lowBitNo, highBitNo, out val);
        }

        public bool IntoBits(int lowBitNo, int highBitNo, ByteSetData toMatch, out int val)
        {
            return BitsInto(lowBitNo, highBitNo, out val);
        }

        public bool NotBit(int bitNo, byte toMatch)
        {
            return !(_pos < _srcLen && ((_src[_pos] >> (bitNo - 1)) & 1) == toMatch);
        }

        public bool NotBits(int lowBitNo, int highBitNo, byte toMatch)
        {
            return !(_pos < _srcLen && ((_src[_pos] >> (lowBitNo - 1)) & ((1 << highBitNo) - 1)) == toMatch);
        }

        public bool NotIn(byte[] s)
        {
            if (_pos < _srcLen)
            {
                byte c = _src[_pos];

                for (int i = 0; i < s.Length - 1; i += 2)
                    if (c >= s[i] && c <= s[i + 1])
                        return false;

                _pos++;

                return true;
            }

            return false;
        }

        public bool NotOneOf(byte[] s)
        {
            if (_pos < _srcLen)
            {
                byte c = _src[_pos];

                for (int i = 0; i < s.Length; ++i)
                    if (c == s[i])
                        return false;

                return true;
            }

            return false;
        }

        public bool OneOf(byte c0, byte c1)
        {
            if (_pos < _srcLen && (_src[_pos] == c0 || _src[_pos] == c1))
            {
                _pos++;

                return true;
            }

            return false;
        }

        public bool OneOf(byte c0, byte c1, byte c2)
        {
            if (_pos < _srcLen)
            {
                byte c = _src[_pos];

                if (c == c0 || c == c1 || c == c2)
                {
                    _pos++;

                    return true;
                }
            }

            return false;
        }

        public bool OneOf(byte c0, byte c1, byte c2, byte c3)
        {
            if (_pos < _srcLen)
            {
                byte c = _src[_pos];

                if (c == c0 || c == c1 || c == c2 || c == c3)
                {
                    _pos++;

                    return true;
                }
            }

            return false;
        }

        public bool OneOf(byte c0, byte c1, byte c2, byte c3, byte c4)
        {
            if (_pos < _srcLen)
            {
                byte c = _src[_pos];

                if (c == c0 || c == c1 || c == c2 || c == c3 || c == c4)
                {
                    _pos++;

                    return true;
                }
            }

            return false;
        }

        public bool OneOf(byte c0, byte c1, byte c2, byte c3, byte c4, byte c5)
        {
            if (_pos < _srcLen)
            {
                byte c = _src[_pos];

                if (c == c0 || c == c1 || c == c2 || c == c3 || c == c4 || c == c5)
                {
                    _pos++;

                    return true;
                }
            }
            return false;
        }

        public bool OneOf(byte c0, byte c1, byte c2, byte c3, byte c4, byte c5, byte c6)
        {
            if (_pos < _srcLen)
            {
                byte c = _src[_pos];

                if (c == c0 || c == c1 || c == c2 || c == c3 || c == c4 || c == c5 || c == c6)
                {
                    _pos++;

                    return true;
                }
            }

            return false;
        }

        public bool OneOf(byte c0, byte c1, byte c2, byte c3, byte c4, byte c5, byte c6, byte c7)
        {
            if (_pos < _srcLen)
            {
                byte c = _src[_pos];

                if (c == c0 || c == c1 || c == c2 || c == c3 || c == c4 || c == c5 || c == c6 || c == c7)
                {
                    _pos++;

                    return true;
                }
            }

            return false;
        }

        public bool OneOf(byte[] s)
        {
            if (_pos < _srcLen)
            {
                byte c = _src[_pos];

                for (int i = 0; i < s.Length; ++i)
                    if (c == s[i])
                    {
                        _pos++;

                        return true;
                    }
            }

            return false;
        }

        public bool OneOf(ByteSetData bset)
        {
            if (_pos < _srcLen && bset.Matches(_src[_pos]))
            {
                _pos++;

                return true;
            }

            return false;
        }

        public bool PeekBit(int bitNo, byte toMatch)
        {
            return _pos < _srcLen && ((_src[_pos] >> (bitNo - 1)) & 1) == toMatch;
        }

        public bool PeekBits(int lowBitNo, int highBitNo, byte toMatch)
        {
            return _pos < _srcLen && ((_src[_pos] >> (lowBitNo - 1)) & ((1 << highBitNo) - 1)) == toMatch;
        }

        public void SetSource(byte[] src)
        {
            if (src == null)
                src = new byte[0];

            _src = src;

            _srcLen = src.Length;

            ResetLineStarts();
        }

        public static byte ToUpper(byte c)
        {
            if (c >= 97 && c <= 122)
                return (byte)(c - 32);
            return c;
        }

        public bool Warning(string msg)
        {
            LogOutMsg("Warning", msg);

            return true;
        }

        #endregion

        #region constructors

        public PegByteParser()
            : this(null)
        {
        }
        public PegByteParser(byte[] src)
            : base(null)
        {
            SetSource(src);
        }
        public PegByteParser(byte[] src, TextWriter errOut)
            : base(errOut)
        {
            SetSource(src);
        }
        #endregion
    }
}
