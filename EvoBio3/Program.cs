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
				Both1Quantity   = 100, Both2Quantity = 100, ResonationQuantity = 100, NullQuantity = 100,
				Generations     = 500, Iterations    = 10000,
				SdGenetic       = 1, SdPheno         = 1,
				MeanPerishStep1 = 30, SdPerishStep1  = 15,
				MeanPerishStep2 = 30, SdPerishStep2  = 15,

				C1   = 0.5, C2 = 0.5,
				Beta = 0.2, Y  = 0.8, R = 0.15, Z = 1.96,
				PiD  = 30, PiC = 60,
				B1   = 100, B2 = 100,
				Qr   = 0, Qb1  = 0, Qb2 = 0, Qt = 800, Qu = 800,
				Pr   = 0, Pb1  = 0, Pb2 = 0,

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