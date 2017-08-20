namespace ProcessPlayer.Data.Functions
{
	public static class LogExtensions
	{
		#region public static methods

		public static bool and(params bool[] logicals)
		{
			bool res = true;

			foreach (var logical in logicals)
				if (!(res &= logical))
					break;

			return res;
		}

		public static object iif(bool logicalTest, object valueIfTrue, object valueIfFalse)
		{
			return logicalTest ? valueIfTrue : valueIfFalse;
		}

		public static bool not(bool logical)
		{
			return !logical;
		}

		public static bool or(params bool[] logicals)
		{
			bool res = false;

			foreach (var logical in logicals)
				if (res |= logical)
					break;

			return res;
		}

		#endregion
	}
}
