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
		public override void CalculateCooperator1Fecundity ( )
		{
			var cutoff = Iteration.Step1Rejects.Count < V.ReservationConditionsCutoff
				? Iteration.ResonationQualityCutoffForCooperator1WithReservationVersion1
				: Iteration.ResonationQualityCutoffForCooperator1WithNoReservationVersion1;

			foreach ( var ind in Iteration.Cooperator1Group.Where ( x => !x.IsPerished ) )
				if ( ind.PhenotypicQuality <= cutoff && TotalPerished < V.ResonationConditionsCutoffForCooperator1 )
				{
					ind.Fecundity = 0;
					if ( IsLoggingEnabled )
						Logger.Debug (
							$"Resonation :: {ind.PaddedName} Qp {ind.PhenotypicQuality,8:F4} <= {cutoff,8:F4};" +
							$" Fecundity = {ind.Fecundity,8:F4}" );
				}
				else if ( ind.HasReserved &
				          ( ind.PhenotypicQuality >
				            Iteration.ResonationQualityCutoffForCooperator1WithReservationVersion1 ||
				            TotalPerished >= V.ResonationConditionsCutoffForCooperator1 ) )
				{
					ind.Fecundity = ind.PhenotypicQuality - V.Beta * V.ReservationCostForCooperator1;
					if ( IsLoggingEnabled )
						Logger.Debug ( $"Reduction  :: {ind.PaddedName} Fecundity = {ind.Fecundity,8:F4}" );
				}
		}

		public override void CalculateCooperator2Fecundity ( )
		{
			var cutoff = Iteration.Step1Rejects.Count < V.ReservationConditionsCutoff
				? Iteration.ResonationQualityCutoffForCooperator2WithReservationVersion1
				: Iteration.ResonationQualityCutoffForCooperator2WithNoReservationVersion1;

			foreach ( var ind in Iteration.Cooperator2Group.Where ( x => !x.IsPerished ) )
				if ( ind.PhenotypicQuality <= cutoff && TotalPerished < V.ResonationConditionsCutoffForCooperator2 )
				{
					ind.Fecundity = 0;
					if ( IsLoggingEnabled )
						Logger.Debug (
							$"Resonation :: {ind.PaddedName} Qp {ind.PhenotypicQuality,8:F4} <= {cutoff,8:F4};" +
							$" Fecundity = {ind.Fecundity,8:F4}" );
				}
				else if ( ind.HasReserved &
				          ( ind.PhenotypicQuality >
				            Iteration.ResonationQualityCutoffForCooperator2WithReservationVersion1 ||
				            TotalPerished >= V.ResonationConditionsCutoffForCooperator2 ) )
				{
					ind.Fecundity = ind.PhenotypicQuality - V.Beta * V.ReservationCostForCooperator2;
					if ( IsLoggingEnabled )
						Logger.Debug ( $"Reduction  :: {ind.PaddedName} Fecundity = {ind.Fecundity,8:F4}" );
				}
		}

		public override void CalculateResonationFecundity ( )
		{
			if ( Iteration.Step1Rejects.Count + Iteration.Step2Rejects.Count >=
			     V.ResonationConditionsCutoffForResonationType )
				return;

			foreach ( var ind in Iteration.ResonationGroup.Where (
				x => !x.IsPerished &&
				     x.PhenotypicQuality <= Iteration.ResonationQualityCutoffForResonationTypeVersion1 ) )
			{
				ind.Fecundity = 0;
				if ( IsLoggingEnabled )
					Logger.Debug (
						$"Resonation :: {ind.PaddedName} Qp {ind.PhenotypicQuality,8:F4} <= {Iteration.ResonationQualityCutoffForResonationTypeVersion1,8:F4};" +
						$" Fecundity = {ind.Fecundity,8:F4}" );
			}
		}
	}
}