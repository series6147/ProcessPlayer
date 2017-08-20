using System;

namespace ProcessPlayer.Data.Functions
{
	public class ValidationException : Exception
	{
		#region properties

		public int Line { get; private set; }
		public int Column { get; private set; }

		#endregion

		#region overriden methods

		public override string ToString()
		{
			return string.Format("({1},{2}): {0}", Message, Line, Column);
		}

		#endregion

		#region constructors

		public ValidationException(Exception e, int lineNo, int colNo)
			: base(e.Message, e)
		{
			Column = colNo;
			Line = lineNo;
		}
		public ValidationException(string msg, int lineNo, int colNo)
			: base(msg)
		{
			Column = colNo;
			Line = lineNo;
		}

		#endregion
	}
}
