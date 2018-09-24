using System.Linq;
using EvoBio3.Core;
using EvoBio3.Core.Enums;
using EvoBio3.Core.Extensions;

namespace EvoBio3.Versions
{
	public class AllowAllPerishVersion : SingleIterationBaseVersion
	{
		public bool AllPerished { get; private set; }

		public override void Perish1 ( )
		{
			AdjustmentRules.AdjustStep1 ( );

			Step1PerishCount = Utility.NextGaussianIntInRange ( V.MeanPerishStep1, V.SdPerishStep1,
			                                                    0, V.PopulationSize );

			if ( Step1PerishCount == V.PopulationSize )
			{
				AllPerished = true;
				return;
			}

			var survivorsCount = AllIndividuals.Count - Step1PerishCount;

			( Step1Survivors, Step1Rejects ) = AllIndividuals.ChooseBy ( survivorsCount, x => x.PhenotypicQuality );
			foreach ( var ind in Step1Rejects )
				ind.Perish ( );

			if ( IsLoggingEnabled )
			{
				Logger.Debug ( "\n\nPerish 1:\n" );
				Logger.Debug ( $"Amount to perish = {Step1PerishCount}" );
				Logger.Debug ( "Perished Individuals:" );
				Logger.Debug ( Step1Rejects
					               .OrderBy ( x => x.Type )
					               .ThenBy ( x => x.Id )
					               .ToTable ( x => new
						               {
							               _Type = x.Type,
							               x.Id,
							               Qp = $"{x.PhenotypicQuality:F4}"
						               }
					               )
				);
			}
		}

		public override void Perish2 ( )
		{
			if ( IsLoggingEnabled )
				Logger.Debug ( "\n\nPerish 2:\n" );

			if ( Step1PerishCount >= V.PopulationSize )
				return;

			AdjustmentRules.AdjustStep2 ( );

			Step2PerishCount = Utility.NextGaussianIntInRange ( V.MeanPerishStep2, V.SdPerishStep2,
			                                                    0, Step1Survivors.Count );

			if ( Step2PerishCount == Step1Survivors.Count )
			{
				AllPerished = true;
				return;
			}

			var step2SurvivorsCount = Step1Survivors.Count - Step2PerishCount;

			( Step2Survivors, Step2Rejects ) = Step1Survivors.ChooseBy ( step2SurvivorsCount, x => x.S );
			foreach ( var ind in Step2Rejects )
				ind.Perish ( );

			if ( IsLoggingEnabled )
			{
				Logger.Debug ( "\n" );
				Logger.Debug ( $"Amount to perish = {Step2PerishCount}" );
				Logger.Debug ( "Perished Individuals:" );
				Logger.Debug ( Step2Rejects
					               .OrderBy ( x => x.Type )
					               .ThenBy ( x => x.Id )
					               .ToTable ( x => new
						               {
							               _Type = x.Type,
							               x.Id,
							               Qp = $"{x.PhenotypicQuality:F4}",
							               S  = $"{x.S:F4}"
						               }
					               )
				);
			}
		}

		public override bool SimulateGeneration ( )
		{
			ResetLists ( );

			Perish1 ( );
			if ( AllPerished )
				return false;

			Perish2 ( );
			if ( AllPerished )
				return false;

			CalculateFecundity ( );
			CalculateAdjustedFecundity ( );
			ChooseParentsAndReproduce ( );
			AddGenerationHistory ( );

			return AllGroups.All ( x => x.Count != V.PopulationSize );
		}

		protected override void CalculateWinner ( )
		{
			if ( AllPerished )
			{
				Winner = Winner.Tie;
				return;
			}

			base.CalculateWinner ( );
		}
	}
}