using System;

namespace ProcessPlayer.Data.Common.Utils
{
    public static class CRC16CCITT
    {
        #region private constants

        private const ushort poly = 4129;

        #endregion

        #region private variables

        private static readonly ushort[] _crc16Table = new ushort[256];

        #endregion

        #region public methods

        public static ushort ComputeChecksum(byte[] bytes, ushort initialValue)
        {
            ushort crc = initialValue;

            for (int i = 0; i < bytes.Length; ++i)
                crc = (ushort)((crc << 8) ^ _crc16Table[((crc >> 8) ^ (0xff & bytes[i]))]);

            return crc;
        }

        public static byte[] ComputeChecksumBytes(byte[] bytes, ushort initialValue)
        {
            ushort crc = ComputeChecksum(bytes, initialValue);

            return BitConverter.GetBytes(crc);
        }

        #endregion

        #region constructors

        static CRC16CCITT()
        {
            ushort a, tmp;

            for (int i = 0; i < _crc16Table.Length; ++i)
            {
                a = (ushort)(i << 8);
                tmp = 0;

                for (int j = 0; j < 8; ++j)
                {
                    if (((tmp ^ a) & 0x8000) != 0)
                        tmp = (ushort)((tmp << 1) ^ poly);
                    else
                        tmp <<= 1;

                    a <<= 1;
                }

                _crc16Table[i] = tmp;
            }
        }

        #endregion
    }

    public enum InitialCrcValue { Zeros, NonZero1 = 0xffff, NonZero2 = 0x1D0F }
}
