using System.Linq;
using EvoBio3.Collections;
using EvoBio3.Core.Collections;
using EvoBio3.Core.Interfaces;

namespace EvoBio3.AdjustmentRules.Abstractions
{
	public abstract class ReservateOnPercentile :
		AdjustmentRulesBase<Individual, IndividualGroup, Variables,
			ISingleIteration<Individual, IndividualGroup, Variables>>
	{
		public override void AdjustBoth1Step2 ( )
		{
			if ( Iteration.Step1Rejects.Count >= V.PiD )
				return;

			foreach ( var ind in Iteration.Both1Group.Where ( x => x.PhenotypicQuality > Iteration.Both1Threshold ) )
				ind.S -= V.C1;
		}

		public override void AdjustBoth2Step2 ( )
		{
			if ( Iteration.Step1Rejects.Count >= V.PiD )
				return;

			foreach ( var ind in Iteration.Both2Group.Where ( x => x.PhenotypicQuality > Iteration.Both2Threshold ) )
				ind.S -= V.C2;
		}
	}
}