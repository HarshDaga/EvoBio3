using EvoBio3.AdjustmentRules.Abstractions;

namespace EvoBio3.AdjustmentRules
{
	public class Version1Adjustments : ReserveAndAdjustOnPercentile
	{
		public override void AdjustStep2 ( )
		{
			Iteration.CalculateThresholds ( );
			AdjustCooperator1Step2 ( );
			AdjustCooperator2Step2 ( );
		}

		public override void CalculateFecundity ( )
		{
			Iteration.CalculateThresholds ( );
			base.CalculateFecundity ( );
		}
	}
}