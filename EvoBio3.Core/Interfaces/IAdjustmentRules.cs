namespace EvoBio3.Core.Interfaces
{
	public interface IAdjustmentRules<TIndividual, TGroup, TVariables, in TIteration>
		where TIndividual : class, IIndividual, new ( )
		where TGroup : IIndividualGroup<TIndividual>, new ( )
		where TVariables : IVariables
		where TIteration : ISingleIteration<TIndividual, TGroup, TVariables>
	{
		TIteration Iteration { set; }

		void Reset ( );
		void AdjustStep1 ( );
		void AdjustCooperator1Step2 ( );
		void AdjustCooperator2Step2 ( );
		void AdjustResonationStep2 ( );
		void AdjustStep2 ( );

		void CalculateCooperator1Fecundity ( );
		void CalculateCooperator2Fecundity ( );
		void CalculateResonationFecundity ( );
		void CalculateFecundity ( );
	}
}