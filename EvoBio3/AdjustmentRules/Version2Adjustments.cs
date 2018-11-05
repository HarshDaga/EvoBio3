using EvoBio3.AdjustmentRules.Abstractions;

namespace EvoBio3.AdjustmentRules
{
	public class Version2Adjustments : ReserveAndAdjustOnPercentile
	{
		public override void AdjustStep1 ( )
		{
			Iteration.CalculateThresholds ( );
		}
	}
}