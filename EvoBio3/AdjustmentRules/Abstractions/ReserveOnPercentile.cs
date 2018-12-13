using System;
using System.Linq;
using EvoBio3.Collections;
using EvoBio3.Core.Collections;
using EvoBio3.Core.Interfaces;

namespace EvoBio3.AdjustmentRules.Abstractions
{
	public abstract class ReserveOnPercentile :
		AdjustmentRulesBase<Individual, IndividualGroup, Variables,
			ISingleIteration<Individual, IndividualGroup, Variables>>
	{
		public override void AdjustCooperator1Step2 ( )
		{
			if ( Iteration.Step1Rejects.Count >= V.ReservationConditionsCutoff )
				return;

			foreach ( var ind in Iteration.Cooperator1Group.Where (
				x => x.PhenotypicQuality > Iteration.ReservationQualityCutoffForCooperator1Version1 ) )
			{
				ind.HasReserved = true;
				ind.S           = Math.Max ( 0, ind.S - V.ReservationCostForCooperator1 );
				if ( IsLoggingEnabled )
					Logger.Debug (
						$"{ind.PaddedName} Qp {ind.PhenotypicQuality,8:F4} > {Iteration.ReservationQualityCutoffForCooperator1Version1,8:F4}; S ={ind.S,8:F4}" );
			}
		}

		public override void AdjustCooperator2Step2 ( )
		{
			if ( Iteration.Step1Rejects.Count >= V.ReservationConditionsCutoff )
				return;

			foreach ( var ind in Iteration.Cooperator2Group.Where (
				x => x.PhenotypicQuality > Iteration.ReservationQualityCutoffForCooperator2Version1 ) )
			{
				ind.HasReserved = true;
				ind.S           = Math.Max ( 0, ind.S - V.ReservationCostForCooperator2 );
				if ( IsLoggingEnabled )
					Logger.Debug (
						$"{ind.PaddedName} Qp {ind.PhenotypicQuality,8:F4} > {Iteration.ReservationQualityCutoffForCooperator2Version1,8:F4}; S ={ind.S,8:F4}" );
			}
		}
	}
}