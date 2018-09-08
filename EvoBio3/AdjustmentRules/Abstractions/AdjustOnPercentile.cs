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
			if ( Iteration.Step1Rejects.Count + Iteration.Step2Rejects.Count > V.PiC )
				return;

			foreach ( var ind in Iteration.Both1Group )
				if ( ind.PhenotypicQuality <= Iteration.Both1Threshold )
				{
					ind.Fecundity = 0;
					if ( IsLoggingEnabled )
					{
						Logger.Debug (
							$"{ind.PaddedName} Qp {ind.PhenotypicQuality,8:F4} <= {Iteration.Both1Threshold,8:F4};" +
							$" Fecundity = {ind.Fecundity,8:F4}" );
					}
				}
				else
				{
					ind.Fecundity = ind.PhenotypicQuality - V.Beta * V.C1;
					if ( IsLoggingEnabled )
					{
						Logger.Debug (
							$"{ind.PaddedName} Qp {ind.PhenotypicQuality,8:F4} > {Iteration.Both1Threshold,8:F4};" +
							$" Fecundity = {ind.Fecundity,8:F4}" );
					}
				}
		}

		public override void CalculateBoth2Fecundity ( )
		{
			if ( Iteration.Step1Rejects.Count + Iteration.Step2Rejects.Count > V.PiC )
				return;

			foreach ( var ind in Iteration.Both2Group )
				if ( ind.PhenotypicQuality <= Iteration.Both2Threshold )
				{
					ind.Fecundity = 0;
					if ( IsLoggingEnabled )
					{
						Logger.Debug (
							$"{ind.PaddedName} Qp {ind.PhenotypicQuality,8:F4} <= {Iteration.Both2Threshold,8:F4};" +
							$" Fecundity = {ind.Fecundity,8:F4}" );
					}
				}
				else
				{
					ind.Fecundity = ind.PhenotypicQuality - V.Beta * V.C2;
					if ( IsLoggingEnabled )
					{
						Logger.Debug (
							$"{ind.PaddedName} Qp {ind.PhenotypicQuality,8:F4} > {Iteration.Both2Threshold,8:F4};" +
							$" Fecundity = {ind.Fecundity,8:F4}" );
					}
				}
		}

		public override void CalculateResonationFecundity ( )
		{
			if ( Iteration.Step1Rejects.Count + Iteration.Step2Rejects.Count > V.PiC )
				return;

			foreach ( var ind in Iteration.ResonationGroup.Where (
				x => x.PhenotypicQuality <= Iteration.ResonationThreshold ) )
			{
				ind.Fecundity = 0;
				if ( IsLoggingEnabled )
				{
					Logger.Debug (
						$"{ind.PaddedName} Qp {ind.PhenotypicQuality,8:F4} <= {Iteration.ResonationThreshold,8:F4};" +
						$" Fecundity = {ind.Fecundity,8:F4}" );
				}
			}
		}
	}
}