using System.IO;
using System.Linq;
using EvoBio3.Collections;
using EvoBio3.Core;
using EvoBio3.Core.Interfaces;

namespace EvoBio3
{
	public class Simulation<TIteration, TAdjustmentRules> :
		SimulationBase<Individual, IndividualGroup, TIteration,
			HeritabilitySummary, Variables, TAdjustmentRules>
		where TIteration : SingleIterationBase<Individual, IndividualGroup, Population, Variables>, new ( )
		where TAdjustmentRules : IAdjustmentRules<Individual, IndividualGroup, Variables,
			ISingleIteration<Individual, IndividualGroup, Variables>>, new ( )
	{
		public Simulation ( Variables v ) : base ( v )
		{
		}

		public override void PrintConfidenceIntervals ( string fileName )
		{
			ConfidenceIntervalStats.Compute ( );
			ConfidenceIntervalStats.PrintToCsv ( "ConfidenceIntervals.csv" );
			var ci = ConfidenceIntervalStats.ToTable ( );

			var properties = new[]
			{
				"Genetic Quality",
				"Variance Genetic Quality",
				"Covariance Genetic Quality",
				"Phenotypic Quality",
				"Variance Phenotypic Quality",
				"Covariance Phenotypic Quality",
				"Reproduction",
				"Variance Reproduction",
				"Covariance Reproduction"
			};

			ci += "\n\n\nHeritability Confidence Intervals:\n\n";

			for ( var i = 0; i < properties.Length; i++ )
			{
				var property = properties[i];
				var interval = Utility.CalculateConfidenceInterval (
					HeritabilitySummaries.Select ( x => x.Values[i] ).ToList ( ),
					V.Z );
				ci += $"{property,-33}: {interval}\n";
			}

			var lines = ci.Split ( '\n' ).Select ( x => x.TrimEnd ( ) );
			File.WriteAllLines ( fileName, lines );
		}
	}
}