using System.Linq;

namespace EvoBio3.AdjustmentRules.Abstractions
{
	public abstract class ReservateAndAdjustOnPercentile : ReservateOnPercentile
	{
		public override void CalculateBoth1Fecundity ( )
		{
			if ( Iteration.Step1Rejects.Count + Iteration.Step2Rejects.Count > V.PiC )
				return;

			foreach ( var ind in Iteration.Both1Group )
				if ( ind.PhenotypicQuality > Iteration.Both1Threshold )
					ind.Fecundity = 0;
				else
					ind.Fecundity = ind.PhenotypicQuality - V.Beta * V.C1;
		}

		public override void CalculateBoth2Fecundity ( )
		{
			if ( Iteration.Step1Rejects.Count + Iteration.Step2Rejects.Count > V.PiC )
				return;

			foreach ( var ind in Iteration.Both2Group )
				if ( ind.PhenotypicQuality > Iteration.Both2Threshold )
					ind.Fecundity = 0;
				else
					ind.Fecundity = ind.PhenotypicQuality - V.Beta * V.C2;
		}

		public override void CalculateResonationFecundity ( )
		{
			if ( Iteration.Step1Rejects.Count + Iteration.Step2Rejects.Count > V.PiC )
				return;

			foreach ( var ind in Iteration.ResonationGroup.Where (
				x => x.PhenotypicQuality > Iteration.ResonationThreshold ) )
				ind.Fecundity = 0;
		}
	}
}