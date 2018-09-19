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
				Both1Quantity   = 0, Both2Quantity    = 0, ResonationQuantity = 100, NullQuantity = 100,
				Generations     = 250, Iterations     = 10000,
				SdGenetic       = 1, SdPheno          = 0,
				MeanPerishStep1 = 50.0, SdPerishStep1 = 50.0,
				MeanPerishStep2 = 0, SdPerishStep2    = 0,

				C1   = 0, C2   = 0,
				Beta = 0, Y    = .8, R = 0.15, Z = 1.96,
				PiD  = 70, PiC = 0,
				B1   = 10, B2  = 15,
				Qr   = 8, Qb1  = 0, Qb2  = 0, Qt = 0, Qu = 0,
				Pr   = 35, Pb1 = 10, Pb2 = 25,

				IsConfidenceIntervalsRequested = false
			};

			var timer = Stopwatch.StartNew ( );

			var simulation = new Simulation<SingleIterationBaseVersion, Version3Adjustments> ( v );
			simulation.Run ( );
			Console.WriteLine ( simulation );

			Console.WriteLine ( timer.Elapsed );

			Console.ReadLine ( );
		}
	}
}