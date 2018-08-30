namespace EvoBio3.Core
{
	public class ConfidenceInterval
	{
		public double Low { get; set; }
		public double Mean { get; set; }
		public double High { get; set; }

		public ConfidenceInterval ( )
		{
		}

		public ConfidenceInterval ( double low,
		                            double mean,
		                            double high )
		{
			Low  = low;
			Mean = mean;
			High = high;
		}

		public ConfidenceInterval ( double mean,
		                            double range )
		{
			Low  = mean - range;
			Mean = mean;
			High = mean + range;
		}

		public override string ToString ( ) => $"[{Low:F4}, {Mean:F4}, {High:F4}]";
	}
}