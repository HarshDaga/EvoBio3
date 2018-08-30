using System.Linq;
using EvoBio3.Collections;
using EvoBio3.Core.Collections;
using EvoBio3.Core.Interfaces;

namespace EvoBio3.AdjustmentRules
{
	public class DefaultAdjustments :
		AdjustmentRulesBase<Individual, IndividualGroup, Variables,
			ISingleIteration<Individual, IndividualGroup, Variables>>
	{
		public override void AdjustBoth1Step2 ( )
		{
			if ( Iteration.Step1Rejects.Count >= V.PiD )
				return;

			foreach ( var ind in Iteration.Both1Group.Where ( x => x.PhenotypicQuality > V.Qt ) )
				ind.S -= V.C1;
		}

		public override void AdjustBoth2Step2 ( )
		{
			if ( Iteration.Step1Rejects.Count >= V.PiD )
				return;

			foreach ( var ind in Iteration.Both2Group.Where ( x => x.PhenotypicQuality > V.Qu ) )
				ind.S -= V.C2;
		}

		public override void AdjustStep2 ( )
		{
			AdjustBoth1Step2 ( );
			AdjustBoth2Step2 ( );
		}

		public override void CalculateBoth1Fecundity ( )
		{
			if ( Iteration.Step1Rejects.Count + Iteration.Step2Rejects.Count > V.PiC )
				return;

			foreach ( var ind in Iteration.Both1Group )
				if ( ind.PhenotypicQuality <= V.Qb1 )
					ind.Fecundity = 0;
				else
					ind.Fecundity = ind.PhenotypicQuality - V.Beta * V.C1;
		}

		public override void CalculateBoth2Fecundity ( )
		{
			if ( Iteration.Step1Rejects.Count + Iteration.Step2Rejects.Count > V.PiC )
				return;

			foreach ( var ind in Iteration.Both2Group )
				if ( ind.PhenotypicQuality <= V.Qb2 )
					ind.Fecundity = 0;
				else
					ind.Fecundity = ind.PhenotypicQuality - V.Beta * V.C2;
		}

		public override void CalculateResonationFecundity ( )
		{
			if ( Iteration.Step1Rejects.Count + Iteration.Step2Rejects.Count > V.PiC )
				return;

			foreach ( var ind in Iteration.ResonationGroup.Where ( x => x.PhenotypicQuality <= V.Qr ) )
				ind.Fecundity = 0;
		}
	}
}