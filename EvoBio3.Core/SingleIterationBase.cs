using System.Collections.Generic;
using System.Linq;
using EvoBio3.Core.Enums;
using EvoBio3.Core.Extensions;
using EvoBio3.Core.Interfaces;
using NLog;

namespace EvoBio3.Core
{
	public abstract class SingleIterationBase<TIndividual, TGroup, TPopulation, TVariables> :
		ISingleIteration<TIndividual, TGroup, TVariables>
		where TIndividual : class, IIndividual, new ( )
		where TGroup : IIndividualGroup<TIndividual>, new ( )
		where TPopulation : IPopulation<TIndividual, TGroup, TVariables>, new ( )
		where TVariables : IVariables
	{
		// ReSharper disable once StaticMemberInGenericType
		protected static Logger Logger { get; set; } = LogManager.GetCurrentClassLogger ( );
		public bool IsLoggingEnabled { get; set; }

		public IList<TIndividual> AllIndividuals
		{
			get => Population.AllIndividuals;
			set => Population.AllIndividuals = value;
		}

		public TGroup Cooperator1Group
		{
			get => Population.Cooperator1Group;
			set => Population.Cooperator1Group = value;
		}

		public TGroup Cooperator2Group
		{
			get => Population.Cooperator2Group;
			set => Population.Cooperator2Group = value;
		}

		public TGroup ResonationGroup
		{
			get => Population.ResonationGroup;
			set => Population.ResonationGroup = value;
		}

		public TGroup DefectorGroup
		{
			get => Population.DefectorGroup;
			set => Population.DefectorGroup = value;
		}

		public int Step1PerishCount { get; protected set; }
		public int Step2PerishCount { get; protected set; }
		public IList<TIndividual> Step1Rejects { get; protected set; }
		public IList<TIndividual> Step2Rejects { get; protected set; }
		public IList<TIndividual> Step1Survivors { get; protected set; }
		public IList<TIndividual> Step2Survivors { get; protected set; }
		public int TotalPerished => Step1PerishCount + Step2PerishCount;

		public TGroup[] AllGroups
		{
			get => Population.AllGroups;
			set => Population.AllGroups = value;
		}

		public TVariables V { get; protected set; }


		public IAdjustmentRules<TIndividual, TGroup, TVariables,
			ISingleIteration<TIndividual, TGroup, TVariables>> AdjustmentRules { get; protected set; }

		public double PerishedPercent { get; protected set; }
		public int GenerationsPassed { get; protected set; }
		public double ReservationQualityCutoffForCooperator1Version1 { get; protected set; }
		public double ReservationQualityCutoffForCooperator2Version1 { get; protected set; }
		public double ResonationQualityCutoffForResonationTypeVersion1 { get; protected set; }
		public double ResonationQualityCutoffForCooperator1WithNoReservationVersion1 { get; protected set; }
		public double ResonationQualityCutoffForCooperator2WithNoReservationVersion1 { get; protected set; }
		public double ResonationQualityCutoffForCooperator1WithReservationVersion1 { get; protected set; }
		public double ResonationQualityCutoffForCooperator2WithReservationVersion1 { get; protected set; }

		public IHeritabilitySummary Heritability { get; set; }
		public Winner Winner { get; set; }

		public IDictionary<IndividualType, List<int>> GenerationHistory { get; protected set; }

		public List<(TIndividual parent, TIndividual offspring)> History;

		public TPopulation Population;

		protected SingleIterationBase ( )
		{
		}

		protected SingleIterationBase (
			TVariables v,
			IAdjustmentRules<TIndividual, TGroup, TVariables,
				ISingleIteration<TIndividual, TGroup, TVariables>> adjustmentRules = null,
			bool isLoggingEnabled = false )
		{
			Init ( v, adjustmentRules, isLoggingEnabled );
		}

		protected SingleIterationBase ( TVariables v,
		                                bool isLoggingEnabled = false ) :
			this ( v, null, isLoggingEnabled )
		{
		}

		public void Init (
			TVariables v,
			IAdjustmentRules<TIndividual, TGroup, TVariables,
				ISingleIteration<TIndividual, TGroup, TVariables>> adjustmentRules = null,
			bool isLoggingEnabled = false )
		{
			AdjustmentRules  = adjustmentRules;
			IsLoggingEnabled = isLoggingEnabled;
			V                = v;
			Population       = new TPopulation ( );
			History =
				new List<(TIndividual parent, TIndividual offspring)> ( V.PopulationSize * V.Generations );

			GenerationHistory = new Dictionary<IndividualType, List<int>> ( );
			foreach ( var type in EnumsNET.Enums.GetValues<IndividualType> ( ) )
				GenerationHistory[type] = new List<int> ( V.Generations );

			if ( AdjustmentRules != null )
				AdjustmentRules.Iteration = this;

			if ( IsLoggingEnabled )
				Logger.Debug ( $"\n\n{V}\n" );
		}

		public virtual void ResetLists ( )
		{
			Step1Rejects   = new List<TIndividual> ( );
			Step2Rejects   = new List<TIndividual> ( );
			Step1Survivors = new List<TIndividual> ( );
			Step2Survivors = new List<TIndividual> ( );
		}

		public virtual void CalculateThresholds ( )
		{
			var values = AllIndividuals
				.Except ( Step1Rejects )
				.Except ( Step2Rejects )
				.Select ( x => x.PhenotypicQuality )
				.OrderBy ( x => x )
				.ToList ( );

			ReservationQualityCutoffForCooperator1Version1 =
				values.AtPercentile ( V.ReservationQualityCutoffForCooperator1Version1 );
			ReservationQualityCutoffForCooperator2Version1 =
				values.AtPercentile ( V.ReservationQualityCutoffForCooperator2Version1 );
			ResonationQualityCutoffForResonationTypeVersion1 =
				values.AtPercentile ( V.ResonationQualityCutoffForResonationTypeVersion1, 0 );
			ResonationQualityCutoffForCooperator1WithNoReservationVersion1 =
				values.AtPercentile ( V.ResonationQualityCutoffForCooperator1WithNoReservationVersion1, 0 );
			ResonationQualityCutoffForCooperator2WithNoReservationVersion1 =
				values.AtPercentile ( V.ResonationQualityCutoffForCooperator2WithNoReservationVersion1, 0 );
			ResonationQualityCutoffForCooperator1WithReservationVersion1 =
				values.AtPercentile ( V.ResonationQualityCutoffForCooperator1WithReservationVersion1, 0 );
			ResonationQualityCutoffForCooperator2WithReservationVersion1 =
				values.AtPercentile ( V.ResonationQualityCutoffForCooperator2WithReservationVersion1, 0 );

			if ( IsLoggingEnabled )
			{
				Logger.Debug ( "\nCalculate Thresholds:\n" );
				Logger.Debug (
					$"Cooperator1 Reservation Threshold      @ {V.ReservationQualityCutoffForCooperator1Version1 / 100:P} = {ReservationQualityCutoffForCooperator1Version1:F4}" );
				Logger.Debug (
					$"Cooperator2 Reservation Threshold      @ {V.ReservationQualityCutoffForCooperator2Version1 / 100:P} = {ReservationQualityCutoffForCooperator2Version1:F4}" );
				Logger.Debug (
					$"Cooperator1 Resonation Threshold       @ {V.ResonationQualityCutoffForCooperator1WithNoReservationVersion1 / 100:P} = {ResonationQualityCutoffForCooperator1WithNoReservationVersion1:F4}" );
				Logger.Debug (
					$"Cooperator2 Resonation Threshold       @ {V.ResonationQualityCutoffForCooperator2WithNoReservationVersion1 / 100:P} = {ResonationQualityCutoffForCooperator2WithNoReservationVersion1:F4}" );
				Logger.Debug (
					$"Resonation Threshold             @ {V.ResonationQualityCutoffForResonationTypeVersion1 / 100:P} = {ResonationQualityCutoffForResonationTypeVersion1:F4}" );
				Logger.Debug (
					$"Cooperator1 Threshold                  @ {V.ResonationQualityCutoffForCooperator1WithReservationVersion1 / 100:P} = {ResonationQualityCutoffForCooperator1WithReservationVersion1:F4}" );
				Logger.Debug (
					$"Cooperator2 Threshold                  @ {V.ResonationQualityCutoffForCooperator2WithReservationVersion1 / 100:P} = {ResonationQualityCutoffForCooperator2WithReservationVersion1:F4}" );
			}
		}

		public virtual void CreateInitialPopulation ( )
		{
			Population.Init ( V );

			if ( IsLoggingEnabled )
			{
				Logger.Debug ( "\n\nInitial Population:\n" );

				foreach ( var group in AllGroups )
					Logger.Debug ( group.ToTable ( ) );
			}
		}

		public abstract void Perish1 ( );

		public abstract void Perish2 ( );

		public abstract void CalculateFecundity ( );

		public abstract void CalculateAdjustedFecundity ( );

		public virtual void ChooseParentsAndReproduce ( )
		{
			var parents = GetParents ( );
			var offsprings = new List<TIndividual> ( V.PopulationSize );
			var lastId = new Dictionary<IndividualType, int>
			{
				[IndividualType.Cooperator1] = 0,
				[IndividualType.Cooperator2] = 0,
				[IndividualType.Resonation]  = 0,
				[IndividualType.Defector]    = 0
			};

			if ( IsLoggingEnabled )
			{
				Logger.Debug ( "\n\nChoose Parents And Reproduce\n" );
				foreach ( var group in parents.GroupBy ( x => x.Type ).Where ( x => x.Any ( ) ) )
					Logger.Debug ( $"{group.Key,-10}" +
					               $" Mean Qg = {group.Average ( x => x.GeneticQuality ),8:F4}" +
					               $" Mean Qp = {group.Average ( x => x.PhenotypicQuality ),8:F4}" );
			}

			Reproduce ( parents, offsprings, lastId );

			AllIndividuals = offsprings;

			Population.RebuildIndividualLists ( );
		}

		public abstract List<TIndividual> GetParents ( );

		public abstract void CalculateHeritability ( );

		public virtual bool SimulateGeneration ( )
		{
			ResetLists ( );
			Perish1 ( );
			Perish2 ( );
			CalculateFecundity ( );
			CalculateAdjustedFecundity ( );
			ChooseParentsAndReproduce ( );
			AddGenerationHistory ( );

			return AllGroups.All ( x => x.Count != V.PopulationSize );
		}

		public virtual void Run ( )
		{
			if ( IsLoggingEnabled )
				Logger.Debug ( $"{this}" );

			CreateInitialPopulation ( );
			AddGenerationHistory ( );

			for ( GenerationsPassed = 0; GenerationsPassed < V.Generations; ++GenerationsPassed )
			{
				if ( IsLoggingEnabled )
					Logger.Debug ( $"\nGeneration #{GenerationsPassed + 1}\n\n" );
				if ( !SimulateGeneration ( ) )
				{
					++GenerationsPassed;
					break;
				}
			}

			if ( V.ConfidenceIntervalsIncludeGenerationsFollowingFixation )
				for ( var i = GenerationsPassed; i < V.Generations; i++ )
					AddGenerationHistory ( );

			CalculateHeritability ( );
			CalculateWinner ( );

			if ( IsLoggingEnabled )
				Logger.Debug ( $"\n\nWinner: {Winner}" );
		}

		protected void AddGenerationHistory ( )
		{
			foreach ( var group in AllGroups )
				GenerationHistory[group.Type].Add ( group.Count );
		}

		protected abstract void Reproduce ( List<TIndividual> lastId,
		                                    List<TIndividual> offsprings,
		                                    Dictionary<IndividualType, int> dictionary );

		protected virtual void CalculateWinner ( )
		{
			AllGroups = AllGroups.OrderByDescending ( x => x.Count ).ToArray ( );
			if ( AllGroups[0].Count > AllGroups[1].Count )
			{
				switch ( AllGroups[0].Type )
				{
					case IndividualType.Cooperator1:
						Winner = Winner.Cooperator1;
						break;
					case IndividualType.Cooperator2:
						Winner = Winner.Cooperator2;
						break;
					case IndividualType.Resonation:
						Winner = Winner.Resonation;
						break;
					case IndividualType.Defector:
						Winner = Winner.Defector;
						break;
				}

				if ( AllGroups[0].Count == V.PopulationSize )
					Winner |= Winner.Fix;
			}
			else
			{
				Winner = Winner.Tie;
			}
		}

		public override string ToString ( ) =>
			$"{GetType ( ).Name} with {AdjustmentRules.GetType ( ).Name}";
	}
}