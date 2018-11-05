using System.Linq;
using EvoBio3.Collections;
using EvoBio3.Core.Collections;
using EvoBio3.Core.Interfaces;

namespace EvoBio3.AdjustmentRules.Abstractions
{
	public abstract class AdjustOnPercentile :
		AdjustmentRulesBase<Individual, IndividualGroup, Variables,
			ISingleIteration<Individual, IndividualGroup, Variables>>
	{
		public override void CalculateBoth1Fecundity ( )
		{
			var cutoff = Iteration.Step1Rejects.Count < V.PiD
				? Iteration.Pb1Percentile
				: Iteration.PrB1Percentile;

			foreach ( var ind in Iteration.Both1Group.Where ( x => !x.IsPerished ) )
				if ( ind.PhenotypicQuality <= cutoff && TotalPerished < V.PiCB1 )
				{
					ind.Fecundity = 0;
					if ( IsLoggingEnabled )
						Logger.Debug (
							$"Resonation :: {ind.PaddedName} Qp {ind.PhenotypicQuality,8:F4} <= {cutoff,8:F4};" +
							$" Fecundity = {ind.Fecundity,8:F4}" );
				}
				else if ( ind.HasReserved &
				          ( ind.PhenotypicQuality > Iteration.Pb1Percentile || TotalPerished >= V.PiCB1 ) )
				{
					ind.Fecundity = ind.PhenotypicQuality - V.Beta * V.C1;
					if ( IsLoggingEnabled )
						Logger.Debug ( $"Reduction  :: {ind.PaddedName} Fecundity = {ind.Fecundity,8:F4}" );
				}
		}

		public override void CalculateBoth2Fecundity ( )
		{
			var cutoff = Iteration.Step1Rejects.Count < V.PiD
				? Iteration.Pb2Percentile
				: Iteration.PrB2Percentile;

			foreach ( var ind in Iteration.Both2Group.Where ( x => !x.IsPerished ) )
				if ( ind.PhenotypicQuality <= cutoff && TotalPerished < V.PiCB2 )
				{
					ind.Fecundity = 0;
					if ( IsLoggingEnabled )
						Logger.Debug (
							$"Resonation :: {ind.PaddedName} Qp {ind.PhenotypicQuality,8:F4} <= {cutoff,8:F4};" +
							$" Fecundity = {ind.Fecundity,8:F4}" );
				}
				else if ( ind.HasReserved &
				          ( ind.PhenotypicQuality > Iteration.Pb2Percentile || TotalPerished >= V.PiCB2 ) )
				{
					ind.Fecundity = ind.PhenotypicQuality - V.Beta * V.C2;
					if ( IsLoggingEnabled )
						Logger.Debug ( $"Reduction  :: {ind.PaddedName} Fecundity = {ind.Fecundity,8:F4}" );
				}
		}

		public override void CalculateResonationFecundity ( )
		{
			if ( Iteration.Step1Rejects.Count + Iteration.Step2Rejects.Count >= V.PiCR )
				return;

			foreach ( var ind in Iteration.ResonationGroup.Where (
				x => !x.IsPerished && x.PhenotypicQuality <= Iteration.PrPercentile ) )
			{
				ind.Fecundity = 0;
				if ( IsLoggingEnabled )
					Logger.Debug (
						$"Resonation :: {ind.PaddedName} Qp {ind.PhenotypicQuality,8:F4} <= {Iteration.PrPercentile,8:F4};" +
						$" Fecundity = {ind.Fecundity,8:F4}" );
			}
		}
	}
}