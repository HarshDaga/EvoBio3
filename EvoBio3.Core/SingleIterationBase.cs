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

		public TGroup Both1Group
		{
			get => Population.Both1Group;
			set => Population.Both1Group = value;
		}

		public TGroup Both2Group
		{
			get => Population.Both2Group;
			set => Population.Both2Group = value;
		}

		public TGroup ResonationGroup
		{
			get => Population.ResonationGroup;
			set => Population.ResonationGroup = value;
		}

		public TGroup NullGroup
		{
			get => Population.NullGroup;
			set => Population.NullGroup = value;
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
		public double ResonationThreshold { get; protected set; }
		public double Both1Threshold { get; protected set; }
		public double Both2Threshold { get; protected set; }

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

			ResonationThreshold = values.AtPercentage ( V.Pr );
			Both1Threshold      = values.AtPercentage ( V.Pb1 );
			Both2Threshold      = values.AtPercentage ( V.Pb2 );

			if ( IsLoggingEnabled )
			{
				Logger.Debug ( "\nCalculate Thresholds:\n" );
				Logger.Debug ( $"Resonation Threshold @ {V.Pr / 100:P} = {ResonationThreshold:F4}" );
				Logger.Debug ( $"Both1 Threshold      @ {V.Pb1 / 100:P} = {Both1Threshold:F4}" );
				Logger.Debug ( $"Both2 Threshold      @ {V.Pb2 / 100:P} = {Both2Threshold:F4}" );
			}
		}

		public virtual void CreateInitialPopulation ( )
		{
			Population.Init ( V );

			if ( IsLoggingEnabled )
			{
				Logger.Debug ( "\n\nInitial Population:\n" );

				foreach ( var individual in AllIndividuals )
					Logger.Debug ( individual );
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
				[IndividualType.Both1]      = 0,
				[IndividualType.Both2]      = 0,
				[IndividualType.Resonation] = 0,
				[IndividualType.Null]       = 0
			};

			if ( IsLoggingEnabled )
			{
				Logger.Debug ( "\n\nChoose Parents And Reproduce\n" );
				foreach ( var group in AllGroups.Where ( x => x.Any ( ) ) )
				{
					Logger.Debug ( $"{group.Type,-10}" +
					               $" Mean Qg = {group.Average ( x => x.GeneticQuality ),8:F4}" +
					               $" Mean Qp = {group.Average ( x => x.PhenotypicQuality ),8:F4}" );
				}
			}

			Reproduce ( parents, offsprings, lastId );

			AllIndividuals = offsprings;

			Population.RebuildIndividualLists ( );
		}

		public abstract List<TIndividual> GetParents ( );

		public abstract void CalculateHeritability ( );

		public virtual void Run ( )
		{
			if ( IsLoggingEnabled )
				Logger.Debug ( $"{this}" );

			CreateInitialPopulation ( );
			AddGenerationHistory ( );
			for ( GenerationsPassed = 0; GenerationsPassed < V.Generations; ++GenerationsPassed )
			{
				ResetLists ( );
				Perish1 ( );
				Perish2 ( );
				CalculateFecundity ( );
				CalculateAdjustedFecundity ( );
				ChooseParentsAndReproduce ( );
				AddGenerationHistory ( );

				if ( AllGroups.Any ( x => x.Count == V.PopulationSize ) )
				{
					++GenerationsPassed;
					break;
				}
			}

			AfterLastGeneration ( );

			CalculateHeritability ( );
			CalculateWinner ( );

			if ( IsLoggingEnabled )
				Logger.Debug ( $"\n\nWinner: {Winner}" );
		}

		public virtual void AfterLastGeneration ( )
		{
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
					case IndividualType.Both1:
						Winner = Winner.Both1;
						break;
					case IndividualType.Both2:
						Winner = Winner.Both2;
						break;
					case IndividualType.Resonation:
						Winner = Winner.Resonation;
						break;
					case IndividualType.Null:
						Winner = Winner.Null;
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