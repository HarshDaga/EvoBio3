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
				Both1Quantity   = 10, Both2Quantity = 8, ResonationQuantity = 9, NullQuantity = 7,
				Generations     = 5, Iterations     = 1,
				SdGenetic       = 1, SdPheno        = 1,
				MeanPerishStep1 = 3, SdPerishStep1  = 1.5,
				MeanPerishStep2 = 4, SdPerishStep2  = 1.5,

				C1   = 1, C2     = 0.5,
				Beta = 0.2, Y    = 0.8, R = 0.15, Z = 1.96,
				PiD  = 30, PiC   = 60,
				B1   = 10, B2    = 15,
				Qr   = 8.10, Qb1 = 8.50, Qb2 = 8.50, Qt = 8, Qu = 8.25,
				Pr   = 10, Pb1   = 20, Pb2   = 25,

				IsConfidenceIntervalsRequested = false
			};

			var timer = Stopwatch.StartNew ( );

			var simulation = new Simulation<ConsiderAllGenerationsVersion, DefaultAdjustments> ( v );
			simulation.Run ( );
			Console.WriteLine ( simulation );

			Console.WriteLine ( timer.Elapsed );

			Console.ReadLine ( );
		}
	}
}