using EvoBio3.AdjustmentRules.Abstractions;

namespace EvoBio3.AdjustmentRules
{
	public class Version3Adjustments : ReservateAndAdjustOnPercentile
	{
		public override void AdjustStep2 ( )
		{
			Iteration.CalculateThresholds ( );
			AdjustBoth1Step2 ( );
			AdjustBoth2Step2 ( );
		}
	}
}