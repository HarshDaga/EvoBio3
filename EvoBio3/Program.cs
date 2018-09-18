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
				Both1Quantity   = 50, Both2Quantity  = 50, ResonationQuantity = 50, NullQuantity = 50,
				Generations     = 250, Iterations    = 10000,
				SdGenetic       = 3, SdPheno         = 0,
				MeanPerishStep1 = 100, SdPerishStep1 = 30,
				MeanPerishStep2 = 0, SdPerishStep2   = 0,

				C1   = 0.5, C2 = 0,
				Beta = 0, Y    = 1, R = 0.15, Z = 1.96,
				PiD  = 20, PiC = 30,
				B1   = 10, B2  = 15,
				Qr   = 8, Qb1  = 8.5, Qb2 = 0, Qt = 8, Qu = 0,
				Pr   = 10, Pb1 = 10, Pb2  = 25,

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