using EvoBio3.AdjustmentRules.Abstractions;

namespace EvoBio3.AdjustmentRules
{
	public class Version3Adjustments : ReserveAndAdjustOnPercentile
	{
		public override void AdjustStep2 ( )
		{
			Iteration.CalculateThresholds ( );
			AdjustCooperator1Step2 ( );
			AdjustCooperator2Step2 ( );
		}
	}
}