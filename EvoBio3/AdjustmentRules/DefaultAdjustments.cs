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
		public override void AdjustBoth1Step2 ( )
		{
			if ( Iteration.Step1Rejects.Count >= V.PiD )
				return;

			foreach ( var ind in Iteration.Both1Group.Where ( x => x.PhenotypicQuality > V.Qt ) )
			{
				ind.S = Math.Max ( 0, ind.PhenotypicQuality - V.C1 );
				if ( IsLoggingEnabled )
					Logger.Debug (
						$"{ind.PaddedName} Qp {ind.PhenotypicQuality,8:F4} > {V.Qt,8:F4}; S ={ind.S,8:F4}" );
			}
		}

		public override void AdjustBoth2Step2 ( )
		{
			if ( Iteration.Step1Rejects.Count >= V.PiD )
				return;

			foreach ( var ind in Iteration.Both2Group.Where ( x => x.PhenotypicQuality > V.Qu ) )
			{
				ind.S = Math.Max ( 0, ind.PhenotypicQuality - V.C2 );
				if ( IsLoggingEnabled )
					Logger.Debug (
						$"{ind.PaddedName} Qp {ind.PhenotypicQuality,8:F4} > {V.Qu,8:F4}; S ={ind.S,8:F4}" );
			}
		}

		public override void AdjustStep2 ( )
		{
			AdjustBoth1Step2 ( );
			AdjustBoth2Step2 ( );
		}

		public override void CalculateBoth1Fecundity ( )
		{
			var perished = Iteration.Step1Rejects.Count + Iteration.Step2Rejects.Count;

			var cutoff = Iteration.Step1Rejects.Count < V.PiD ? V.Qb1 : V.Qrb1;

			foreach ( var ind in Iteration.Both1Group.Where ( x => !x.IsPerished ) )
				if ( ind.PhenotypicQuality <= cutoff && perished < V.PiCB1 )
				{
					ind.Fecundity = 0;
					if ( IsLoggingEnabled )
						Logger.Debug (
							$"{ind.PaddedName} Qp {ind.PhenotypicQuality,8:F4} <= {cutoff,8:F4};" +
							$" Fecundity = {ind.Fecundity,8:F4}" );
				}
				else if ( ind.PhenotypicQuality > V.Qt && perished >= V.PiCB1 )
				{
					ind.Fecundity = ind.PhenotypicQuality - V.Beta * V.C1;
					if ( IsLoggingEnabled )
						Logger.Debug (
							$"{ind.PaddedName} Qp {ind.PhenotypicQuality,8:F4} > {cutoff,8:F4};" +
							$" Fecundity = {ind.Fecundity,8:F4}" );
				}
		}

		public override void CalculateBoth2Fecundity ( )
		{
			var perished = Iteration.Step1Rejects.Count + Iteration.Step2Rejects.Count;

			var cutoff = Iteration.Step1Rejects.Count < V.PiD ? V.Qb2 : V.Qrb2;

			foreach ( var ind in Iteration.Both2Group.Where ( x => !x.IsPerished ) )
				if ( ind.PhenotypicQuality <= cutoff && perished < V.PiCB2 )
				{
					ind.Fecundity = 0;
					if ( IsLoggingEnabled )
						Logger.Debug (
							$"{ind.PaddedName} Qp {ind.PhenotypicQuality,8:F4} <= {cutoff,8:F4};" +
							$" Fecundity = {ind.Fecundity,8:F4}" );
				}
				else if ( ind.PhenotypicQuality > V.Qu && perished >= V.PiCB2 )
				{
					ind.Fecundity = ind.PhenotypicQuality - V.Beta * V.C2;
					if ( IsLoggingEnabled )
						Logger.Debug (
							$"{ind.PaddedName} Qp {ind.PhenotypicQuality,8:F4} > {cutoff,8:F4};" +
							$" Fecundity = {ind.Fecundity,8:F4}" );
				}
		}

		public override void CalculateResonationFecundity ( )
		{
			if ( Iteration.Step1Rejects.Count + Iteration.Step2Rejects.Count >= V.PiCR )
				return;

			foreach ( var ind in Iteration.ResonationGroup.Where ( x => !x.IsPerished && x.PhenotypicQuality <= V.Qr ) )
			{
				ind.Fecundity = 0;
				if ( IsLoggingEnabled )
					Logger.Debug (
						$"{ind.PaddedName} Qp {ind.PhenotypicQuality,8:F4} <= {V.Qr,8:F4};" +
						$" Fecundity = {ind.Fecundity,8:F4}" );
			}
		}
	}
}