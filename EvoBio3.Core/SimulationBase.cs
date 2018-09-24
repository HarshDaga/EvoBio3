using System;
using System.Collections.Generic;
using System.Linq;
using EvoBio3.Core.Collections;
using EvoBio3.Core.Enums;
using EvoBio3.Core.Extensions;
using EvoBio3.Core.Interfaces;
using MathNet.Numerics.Statistics;
using MoreLinq;
using ShellProgressBar;

namespace EvoBio3.Core
{
	public abstract class SimulationBase<TIndividual, TGroup, TIteration,
	                                     THeritability, TVariables, TAdjustmentRules> :
		ISimulation
		where TIndividual : class, IIndividual, new ( )
		where TGroup : IIndividualGroup<TIndividual>, new ( )
		where TVariables : IVariables, new ( )
		where TIteration : ISingleIteration<TIndividual, TGroup, TVariables>, new ( )
		where THeritability : IHeritabilitySummary, new ( )
		where TAdjustmentRules : IAdjustmentRules<TIndividual, TGroup, TVariables,
			ISingleIteration<TIndividual, TGroup, TVariables>>, new ( )
	{
		public const int NumberWidth = 8;

		public Dictionary<Winner, int> Wins { get; }
		public Dictionary<int, int> GenerationsCount { get; }
		public THeritability HeritabilityMean { get; }
		public THeritability HeritabilitySd { get; }
		public List<IHeritabilitySummary> HeritabilitySummaries { get; }
		public IConfidenceIntervalStats ConfidenceIntervalStats { get; }

		public readonly int DegreeOfParallelism = Environment.ProcessorCount * 2;
		protected readonly object SyncLock = new object ( );

		public readonly TVariables V;

		protected SimulationBase ( TVariables v )
		{
			var adjustmentRules = new TAdjustmentRules ( );
			V = v;
			Wins = new Dictionary<Winner, int>
			{
				[Winner.Both1]                   = 0,
				[Winner.Both2]                   = 0,
				[Winner.Resonation]              = 0,
				[Winner.Null]                    = 0,
				[Winner.Both1 | Winner.Fix]      = 0,
				[Winner.Both2 | Winner.Fix]      = 0,
				[Winner.Resonation | Winner.Fix] = 0,
				[Winner.Null | Winner.Fix]       = 0,
				[Winner.Tie]                     = 0
			};
			GenerationsCount = Enumerable
				.Range ( 1, V.Generations )
				.ToDictionary ( x => x, x => 0 );

			HeritabilitySummaries = new List<IHeritabilitySummary> ( V.Iterations );
			HeritabilityMean      = new THeritability ( );
			HeritabilitySd        = new THeritability ( );

			if ( V.IsConfidenceIntervalsRequested )
				ConfidenceIntervalStats = new ConfidenceIntervalStats ( V.Generations, V.Iterations, V.Z );

			var generations = V.Generations;
			var iterations = V.Iterations;
			V.Generations = 5;
			V.Iterations  = 5;

			var iteration = new TIteration ( );
			iteration.Init ( V, adjustmentRules, true );
			iteration.Run ( );

			V.Generations = generations;
			V.Iterations  = iterations;
		}

		public virtual void Run ( )
		{
			var options = ProgressBarOptions.Default;
			options.EnableTaskBarProgress = true;

			using ( var pbar = new ProgressBar ( V.Iterations, "Simulating", options ) )
			{
				ParallelEnumerable
					.Range ( 0, V.Iterations )
					.ForAll ( i =>
						{
							var adjustmentRules = new TAdjustmentRules ( );
							var iteration = new TIteration ( );
							iteration.Init ( V, adjustmentRules );
							iteration.Run ( );

							lock ( SyncLock )
							{
								++Wins[iteration.Winner];
								++GenerationsCount[iteration.GenerationsPassed];
								ConfidenceIntervalStats?.Add ( iteration.GenerationHistory );
							}

							if ( iteration.GenerationsPassed > 2 )
								HeritabilitySummaries.Add ( iteration.Heritability );

							// ReSharper disable once AccessToDisposedClosure
							pbar.Tick ( );
						}
					);
			}

			for ( var i = 0; i < HeritabilityMean.ValueCount; i++ )
			{
				var index = i;
				( HeritabilityMean.Values[index], HeritabilitySd.Values[index] ) =
					HeritabilitySummaries.Select ( x => x.Values[index] ).MeanStandardDeviation ( );
			}

			if ( V.IsConfidenceIntervalsRequested )
				PrintConfidenceIntervals ( "ConfidenceIntervals.txt" );
		}

		public abstract void PrintConfidenceIntervals ( string fileName );

		public override string ToString ( )
		{
			var result = $"\n\n{V}\n\n";
			result += string.Join ( "\n", Wins.Select ( x => $"{x.Key,16} = {x.Value}" ) );
			result += $"\n\n\nHeritability Mean:\n{HeritabilityMean}\n";
			result += $"\nHeritability Standard Deviation:\n{HeritabilitySd}\n";

			var max = GenerationsCount.Values.Max ( );
			result += "\nGenerations Count:\n";
			var count = GenerationsCount.Values
				.CumulativeSum ( )
				.ToList ( )
				.TakeUntil ( x => x >= V.Iterations )
				.Count ( );
			result += string.Join (
				"\n",
				GenerationsCount
					.Take ( count )
					.Select ( pair => $"{pair.Key,-3} {pair.Value,-5} " +
					                  $"{new string ( '█', pair.Value * 100 / max )}" ) );

			return result;
		}
	}
}