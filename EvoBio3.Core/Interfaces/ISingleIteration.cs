using System.Collections.Generic;
using EvoBio3.Core.Enums;

namespace EvoBio3.Core.Interfaces
{
	public interface ISingleIteration<TIndividual, TGroup, TVariables>
		where TIndividual : class, IIndividual, new ( )
		where TGroup : IIndividualGroup<TIndividual>, new ( )
		where TVariables : IVariables
	{
		bool IsLoggingEnabled { get; }
		TVariables V { get; }
		TGroup[] AllGroups { get; set; }
		IList<TIndividual> AllIndividuals { get; set; }
		TGroup Both1Group { get; set; }
		TGroup Both2Group { get; set; }
		TGroup ResonationGroup { get; set; }
		TGroup NullGroup { get; set; }
		IList<TIndividual> Step1Rejects { get; }
		IList<TIndividual> Step2Rejects { get; }
		int TotalPerished { get; }
		IHeritabilitySummary Heritability { get; }
		Winner Winner { get; }
		int GenerationsPassed { get; }

		IAdjustmentRules<TIndividual, TGroup, TVariables,
			ISingleIteration<TIndividual, TGroup, TVariables>> AdjustmentRules { get; }

		IDictionary<IndividualType, List<int>> GenerationHistory { get; }

		void Init (
			TVariables v,
			IAdjustmentRules<TIndividual, TGroup, TVariables,
				ISingleIteration<TIndividual, TGroup, TVariables>> adjustmentRules = null,
			bool isLoggingEnabled = false );

		void CreateInitialPopulation ( );
		void Perish1 ( );
		void Perish2 ( );
		void CalculateFecundity ( );
		void CalculateAdjustedFecundity ( );
		List<TIndividual> GetParents ( );
		void ChooseParentsAndReproduce ( );
		void CalculateHeritability ( );
		void Run ( );
	}
}