﻿using System;
using System.Linq;

namespace EvoBio3.AdjustmentRules.Abstractions
{
	public abstract class ReservateAndAdjustOnPercentile : AdjustOnPercentile
	{
		public override void AdjustBoth1Step2 ( )
		{
			if ( Iteration.Step1Rejects.Count >= V.PiD )
				return;

			foreach ( var ind in Iteration.Both1Group.Where (
				x => x.PhenotypicQuality > Iteration.Both1ReservationThreshold ) )
			{
				ind.S = Math.Max ( 0, ind.S - V.C1 );
				if ( IsLoggingEnabled )
					Logger.Debug (
						$"{ind.PaddedName} Qp {ind.PhenotypicQuality,8:F4} > {Iteration.Both1ReservationThreshold,8:F4}; S ={ind.S,8:F4}" );
			}
		}

		public override void AdjustBoth2Step2 ( )
		{
			if ( Iteration.Step1Rejects.Count >= V.PiD )
				return;

			foreach ( var ind in Iteration.Both2Group.Where (
				x => x.PhenotypicQuality > Iteration.Both2ReservationThreshold ) )
			{
				ind.S = Math.Max ( 0, ind.S - V.C2 );
				if ( IsLoggingEnabled )
					Logger.Debug (
						$"{ind.PaddedName} Qp {ind.PhenotypicQuality,8:F4} > {Iteration.Both2ReservationThreshold,8:F4}; S ={ind.S,8:F4}" );
			}
		}
	}
}