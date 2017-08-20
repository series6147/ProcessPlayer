namespace ProcessPlayer.Data.Functions
{
	public class InternalValidationException : ValidationException
	{
		#region constructors

		public InternalValidationException()
			: base("", -1, -1)
		{
		}

		#endregion
	}
}
