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
		void AdjustBoth1Step2 ( );
		void AdjustBoth2Step2 ( );
		void AdjustResonationStep2 ( );
		void AdjustStep2 ( );

		void CalculateBoth1Fecundity ( );
		void CalculateBoth2Fecundity ( );
		void CalculateResonationFecundity ( );
		void CalculateFecundity ( );
	}
}