using System.Collections.Generic;
using System.Linq;
using EvoBio3.Collections;
using EvoBio3.Core;
using EvoBio3.Core.Enums;
using EvoBio3.Core.Interfaces;
using MathNet.Numerics.Statistics;

namespace EvoBio3.Versions
{
	public class SingleIterationBaseVersion :
		SingleIterationBase<Individual, IndividualGroup, Population, Variables>
	{
		public int Step1PerishCount { get; protected set; }
		public int Step2PerishCount { get; protected set; }

		public SingleIterationBaseVersion ( )
		{
		}

		public SingleIterationBaseVersion (
			Variables v,
			bool isLoggingEnabled ) :
			base ( v, null, isLoggingEnabled )
		{
		}

		public SingleIterationBaseVersion (
			Variables v,
			IAdjustmentRules<Individual, IndividualGroup, Variables,
				ISingleIteration<Individual, IndividualGroup, Variables>> adjustmentRules,
			bool isLoggingEnabled ) :
			base ( v, adjustmentRules, isLoggingEnabled )
		{
		}

		public override void Perish1 ( )
		{
			Step1PerishCount = Utility.NextGaussianIntInRange ( V.MeanPerishStep1, V.SdPerishStep1,
			                                                    0, V.PopulationSize );

			( Step1Rejects, _ ) = Population.ChooseBy ( Step1PerishCount, x => x.PhenotypicQuality );

			if ( IsLoggingEnabled )
			{
				Logger.Debug ( "\n\nPerish 1:\n" );
				Logger.Debug ( $"Amount to perish = {Step1PerishCount}" );
				Logger.Debug ( "Perished Individuals:" );
				foreach ( var individual in Step1Rejects.OrderBy ( x => x.Type ).ThenBy ( x => x.Id ) )
					Logger.Debug ( individual );
			}
		}

		public override void Perish2 ( )
		{
			AdjustmentRules.AdjustStep2 ( );

			Step2PerishCount = Utility.NextGaussianIntInRange ( V.MeanPerishStep1, V.SdPerishStep1,
			                                                    0, V.PopulationSize - Step1PerishCount );

			( Step2Rejects, _ ) = Population.ChooseBy ( Step2PerishCount, x => x.S );

			if ( IsLoggingEnabled )
			{
				Logger.Debug ( "\n\nPerish 2:\n" );
				Logger.Debug ( $"Amount to perish = {Step2PerishCount}" );
				Logger.Debug ( "Perished Individuals:" );
				foreach ( var individual in Step2Rejects.OrderBy ( x => x.Type ).ThenBy ( x => x.Id ) )
					Logger.Debug ( individual );
			}
		}

		public override void CalculateFecundity ( )
		{
			foreach ( var ind in Step1Rejects.Concat ( Step2Rejects ) )
				ind.Fecundity = 0;

			AdjustmentRules.CalculateFecundity ( );
			foreach ( var group in AllGroups )
			{
				group.CalculateTotalFecundity ( );
				group.CalculateLostFecundity ( );
			}

			if ( IsLoggingEnabled )
			{
				Logger.Debug ( "\n\nCalculate Fecundity:\n" );
				foreach ( var individual in AllIndividuals.OrderBy ( x => x.Type ).ThenBy ( x => x.Id ) )
					Logger.Debug ( $"{individual} Fecundity = {individual.Fecundity,8:F4}" );
			}
		}

		public override void CalculateAdjustedFecundity ( )
		{
			var lostFecunditySum = AllGroups.Sum ( x => x.LostFecundity );
			var totalFecunditySum = AllGroups.Sum ( x => x.TotalFecundity );
			foreach ( var group in AllGroups )
			{
				var term2 = ( 1d - V.R ) * V.Y * lostFecunditySum / totalFecunditySum;
				var term1 = V.R * V.Y * group.LostFecundity / group.TotalFecundity;
				var multiplier = 1d + term1 + term2;

				foreach ( var individual in group )
					individual.AdjustedFecundity = individual.Fecundity * multiplier;
			}

			if ( IsLoggingEnabled )
			{
				Logger.Debug ( "\n\nCalculate Adjusted Fecundity:\n" );
				foreach ( var individual in AllIndividuals.OrderBy ( x => x.Type ).ThenBy ( x => x.Id ) )
					Logger.Debug ( individual.ToDetailedString ( ) );
			}
		}

		public override List<Individual> GetParents ( ) =>
			Population.RepetitiveChooseBy ( V.PopulationSize, x => x.AdjustedFecundity );

		protected override void Reproduce ( List<Individual> parents,
		                                    List<Individual> offsprings,
		                                    Dictionary<IndividualType, int> lastId )
		{
			var totalGenetic = parents.Sum ( x => x.GeneticQuality );
			foreach ( var parent in parents )
			{
				var z = parent.GeneticQuality * V.PopulationSize * 10 / totalGenetic;

				var geneticQuality = Utility.NextGaussianNonNegative ( z, V.SdGenetic );

				var offspring = parent.Reproduce (
					++lastId[parent.Type],
					geneticQuality,
					Utility.NextGaussianNonNegative ( geneticQuality, V.SdPheno )
				);

				offsprings.Add ( offspring );
				History.Add ( ( parent, offspring ) );
			}

			if ( IsLoggingEnabled )
			{
				Logger.Debug ( "\n\nReproduce:\n" );
				foreach ( var pair in History
					.TakeLast ( V.PopulationSize )
					.OrderBy ( x => x.parent.Type )
					.ThenBy ( x => x.parent.Id )
				)
					Logger.Debug ( $"{pair.parent.PaddedName} => {pair.offspring}" );
			}
		}

		// TODO: Verify
		public override void CalculateHeritability ( )
		{
			if ( GenerationsPassed <= 2 )
				return;

			var groups = History
				.GroupBy ( x => x.parent )
				.Select ( x => ( parent: x.Key, offsprings: x.Select ( y => y.offspring ).ToList ( ) ) )
				.Where ( x => x.offsprings.Any ( ) )
				.ToList ( );

			var covPhenoQuality = groups.Select ( x => x.parent.PhenotypicQuality )
				.PopulationCovariance ( groups.Select ( x => x.offsprings.Average ( y => y.PhenotypicQuality ) ) );
			var covGeneticQuality = groups.Select ( x => x.parent.GeneticQuality )
				.PopulationCovariance ( groups.Select ( x => x.offsprings.Average ( y => y.GeneticQuality ) ) );

			var varPhenoQuality = groups
				.Select ( x => x.parent.PhenotypicQuality )
				.PopulationVariance ( );
			var varGeneticQuality = groups
				.Select ( x => x.parent.GeneticQuality )
				.PopulationVariance ( );

			var pairs = History.Take ( History.Count - V.PopulationSize );
			groups = pairs
				.GroupBy ( x => x.parent )
				.Select ( x => ( parent: x.Key, offsprings: x.Select ( y => y.offspring ).ToList ( ) ) )
				.Where ( x => x.offsprings.Any ( ) )
				.ToList ( );
			var covReproduction = groups.Select ( x => (double) x.parent.OffspringCount )
				.PopulationCovariance ( groups.Select ( x => x.offsprings.Average ( y => y.OffspringCount ) ) );
			var varReproduction = groups
				.Select ( x => x.parent.PhenotypicQuality )
				.PopulationVariance ( );

			Heritability = new HeritabilitySummary
			{
				PhenotypicQuality           = covPhenoQuality / varPhenoQuality,
				VariancePhenotypicQuality   = varPhenoQuality,
				CovariancePhenotypicQuality = covPhenoQuality,
				GeneticQuality              = covGeneticQuality / varGeneticQuality,
				VarianceGeneticQuality      = varGeneticQuality,
				CovarianceGeneticQuality    = covGeneticQuality,
				Reproduction                = covReproduction / varReproduction,
				VarianceReproduction        = varReproduction,
				CovarianceReproduction      = covReproduction
			};
		}
	}
}