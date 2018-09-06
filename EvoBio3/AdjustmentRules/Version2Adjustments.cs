using EvoBio3.AdjustmentRules.Abstractions;

namespace EvoBio3.AdjustmentRules
{
	public class Version2Adjustments : ReservateAndAdjustOnPercentile
	{
		public override void AdjustStep1 ( )
		{
			Iteration.CalculateThresholds ( );
		}
	}
}