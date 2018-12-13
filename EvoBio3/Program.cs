using System;
using System.Diagnostics;
using EvoBio3.AdjustmentRules;
using EvoBio3.Collections;
using EvoBio3.Versions;

namespace EvoBio3
{
	public class Program
	{
		public static void Main ( )
		{
			Console.Title = "Evo Bio 3";

			var v = new Variables
			{
				Cooperator1Quantity = 0, Cooperator2Quantity = 0, ResonationQuantity = 100, DefectorQuantity = 100,
				Generations         = 250, Runs              = 1000,
				SdQuality           = 1, SdPheno             = 0,
				MeanPerishStep1     = 0.0, SdPerishStep1     = 0.0,
				MeanPerishStep2     = 0, SdPerishStep2       = 0,

				ReservationCostForCooperator1                                  = 0.5,
				ReservationCostForCooperator2                                  = 0,
				Beta                                                           = 0.2,
				Y                                                              = .8, R = .15, Z = 1.96,
				ReservationConditionsCutoff                                    = 70,
				ResonationConditionsCutoffForResonationType                    = 0,
				ResonationConditionsCutoffForCooperator1                       = 0,
				ResonationConditionsCutoffForCooperator2                       = 0,
				ReservationQualityCutoffForCooperator1Version1                 = 10,
				ReservationQualityCutoffForCooperator2Version1                 = 15,
				ResonationQualityCutoffForResonationTypeVersion0               = 8,
				ResonationQualityCutoffForCooperator1WithNoReservationVersion0 = 8,
				ResonationQualityCutoffForCooperator2WithNoReservationVersion0 = 8,
				ResonationQualityCutoffForCooperator1WithReservationVersion0   = 0,
				ResonationQualityCutoffForCooperator2WithReservationVersion0   = 0,
				ReservationQualityCutoffForCooperator1Version0                 = 0,
				ReservationQualityCutoffForCooperator2Version0                 = 0,
				ResonationQualityCutoffForResonationTypeVersion1               = -1,
				ResonationQualityCutoffForCooperator1WithNoReservationVersion1 = -1,
				ResonationQualityCutoffForCooperator2WithNoReservationVersion1 = -1,
				ResonationQualityCutoffForCooperator1WithReservationVersion1   = 20,
				ResonationQualityCutoffForCooperator2WithReservationVersion1   = 25,

				ConfidenceIntervalsIncludeGenerationsFollowingFixation         = true,
				IncludeConfidenceIntervals = true
			};

			var timer = Stopwatch.StartNew ( );

			var simulation = new Simulation<AllowAllPerishVersion, Version3Adjustments> ( v );
			simulation.Run ( );
			Console.WriteLine ( simulation );

			Console.WriteLine ( timer.Elapsed );

			Console.ReadLine ( );
		}
	}
}