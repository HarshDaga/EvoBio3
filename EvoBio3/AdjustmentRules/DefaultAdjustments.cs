using System;
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
		public override void AdjustCooperator1Step2 ( )
		{
			if ( Iteration.Step1Rejects.Count >= V.ReservationConditionsCutoff )
				return;

			foreach ( var ind in Iteration.Cooperator1Group.Where (
				x => x.PhenotypicQuality > V.ReservationQualityCutoffForCooperator1Version0 ) )
			{
				ind.HasReserved = true;
				ind.S           = Math.Max ( 0, ind.PhenotypicQuality - V.ReservationCostForCooperator1 );
				if ( IsLoggingEnabled )
					Logger.Debug (
						$"{ind.PaddedName} Qp {ind.PhenotypicQuality,8:F4} > {V.ReservationQualityCutoffForCooperator1Version0,8:F4}; S ={ind.S,8:F4}" );
			}
		}

		public override void AdjustCooperator2Step2 ( )
		{
			if ( Iteration.Step1Rejects.Count >= V.ReservationConditionsCutoff )
				return;

			foreach ( var ind in Iteration.Cooperator2Group.Where (
				x => x.PhenotypicQuality > V.ReservationQualityCutoffForCooperator2Version0 ) )
			{
				ind.HasReserved = true;
				ind.S           = Math.Max ( 0, ind.PhenotypicQuality - V.ReservationCostForCooperator2 );
				if ( IsLoggingEnabled )
					Logger.Debug (
						$"{ind.PaddedName} Qp {ind.PhenotypicQuality,8:F4} > {V.ReservationQualityCutoffForCooperator2Version0,8:F4}; S ={ind.S,8:F4}" );
			}
		}

		public override void AdjustStep2 ( )
		{
			AdjustCooperator1Step2 ( );
			AdjustCooperator2Step2 ( );
		}

		public override void CalculateCooperator1Fecundity ( )
		{
			var cutoff = Iteration.Step1Rejects.Count < V.ReservationConditionsCutoff
				? V.ResonationQualityCutoffForCooperator1WithReservationVersion0
				: V.ResonationQualityCutoffForCooperator1WithNoReservationVersion0;

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
				          ( ind.PhenotypicQuality > V.ResonationQualityCutoffForCooperator1WithReservationVersion0 ||
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
				? V.ResonationQualityCutoffForCooperator2WithReservationVersion0
				: V.ResonationQualityCutoffForCooperator2WithNoReservationVersion0;

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
				          ( ind.PhenotypicQuality > V.ResonationQualityCutoffForCooperator2WithReservationVersion0 ||
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
				x => !x.IsPerished && x.PhenotypicQuality <= V.ResonationQualityCutoffForResonationTypeVersion0 ) )
			{
				ind.Fecundity = 0;
				if ( IsLoggingEnabled )
					Logger.Debug (
						$"Resonation :: {ind.PaddedName} Qp {ind.PhenotypicQuality,8:F4} <= {V.ResonationQualityCutoffForResonationTypeVersion0,8:F4};" +
						$" Fecundity = {ind.Fecundity,8:F4}" );
			}
		}
	}
}