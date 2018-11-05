using System.Collections.Generic;
using EvoBio3.Core.Enums;

namespace EvoBio3.Core.Interfaces
{
	public interface ISingleIteration<TIndividual, TGroup, TVariables>
		where TIndividual : class, IIndividual, new ( )
		where TGroup : IIndividualGroup<TIndividual>, new ( )
		where TVariables : IVariables
	{
		bool IsLoggingEnabled { get; set; }
		TVariables V { get; }
		TGroup[] AllGroups { get; }
		IList<TIndividual> AllIndividuals { get; }
		TGroup Both1Group { get; }
		TGroup Both2Group { get; }
		TGroup ResonationGroup { get; }
		TGroup NullGroup { get; }
		int Step1PerishCount { get; }
		int Step2PerishCount { get; }
		IList<TIndividual> Step1Rejects { get; }
		IList<TIndividual> Step2Rejects { get; }
		IList<TIndividual> Step1Survivors { get; }
		IList<TIndividual> Step2Survivors { get; }
		int TotalPerished { get; }
		IHeritabilitySummary Heritability { get; }
		Winner Winner { get; }
		int GenerationsPassed { get; }
		double B1Percentile { get; }
		double B2Percentile { get; }
		double PrB1Percentile { get; }
		double PrB2Percentile { get; }
		double PrPercentile { get; }
		double Pb1Percentile { get; }
		double Pb2Percentile { get; }

		IAdjustmentRules<TIndividual, TGroup, TVariables,
			ISingleIteration<TIndividual, TGroup, TVariables>> AdjustmentRules { get; }

		IDictionary<IndividualType, List<int>> GenerationHistory { get; }

		void Init (
			TVariables v,
			IAdjustmentRules<TIndividual, TGroup, TVariables,
				ISingleIteration<TIndividual, TGroup, TVariables>> adjustmentRules = null,
			bool isLoggingEnabled = false );

		void ResetLists ( );
		void CalculateThresholds ( );
		void CreateInitialPopulation ( );
		void Perish1 ( );
		void Perish2 ( );
		void CalculateFecundity ( );
		void CalculateAdjustedFecundity ( );
		List<TIndividual> GetParents ( );
		void ChooseParentsAndReproduce ( );
		void CalculateHeritability ( );

		bool SimulateGeneration ( );
		void Run ( );
	}
}