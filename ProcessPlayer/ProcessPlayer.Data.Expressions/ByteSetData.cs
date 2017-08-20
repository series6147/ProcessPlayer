using System.Collections;

namespace ProcessPlayer.Data.Expressions
{
	public sealed class ByteSetData
	{
		#region private variables

		private readonly BitArray _charSet;
		private readonly bool _negated;

		#endregion

		#region public methods

		public bool Matches(byte c)
		{
			var bMatches = c < _charSet.Length && _charSet[c];

			if (_negated)
				return !bMatches;

			return bMatches;
		}

		#endregion

		#region constructors

		public ByteSetData(BitArray b)
			: this(b, false)
		{
		}

		public ByteSetData(BitArray b, bool negated)
		{
			_charSet = new BitArray(b);

			_negated = negated;
		}

		public ByteSetData(RangeChars[] r, byte[] c)
			: this(r, c, false)
		{
		}

		public ByteSetData(RangeChars[] r, byte[] c, bool negated)
		{
			int max = 0;

			if (r != null)
				foreach (RangeChars val in r)
					if (val.high > max)
						max = val.high;

			if (c != null)
				foreach (int val in c)
					if (val > max)
						max = val;

			_charSet = new BitArray(max + 1, false);

			if (r != null)
			{
				foreach (RangeChars val in r)
					for (int i = val.low; i <= val.high; ++i)
						_charSet[i] = true;
			}

			if (c != null)
				foreach (int val in c)
					_charSet[val] = true;

			_negated = negated;
		}

		#endregion
	}
}
