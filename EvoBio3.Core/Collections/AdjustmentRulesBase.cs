using EvoBio3.Core.Interfaces;

namespace EvoBio3.Core.Collections
{
	public abstract class AdjustmentRulesBase<TIndividual, TGroup, TVariables, TIteration> :
		IAdjustmentRules<TIndividual, TGroup, TVariables, TIteration>
		where TIndividual : class, IIndividual, new ( )
		where TGroup : IIndividualGroup<TIndividual>, new ( )
		where TVariables : IVariables
		where TIteration : class, ISingleIteration<TIndividual, TGroup, TVariables>
	{
		public TIteration Iteration { get; set; }
		public TVariables V => Iteration.V;

		protected AdjustmentRulesBase ( TIteration iteration )
		{
			Iteration = iteration;
		}

		protected AdjustmentRulesBase ( )
		{
		}

		public virtual void Reset ( )
		{
			Iteration = null;
		}

		public virtual void AdjustBoth1Step2 ( )
		{
		}

		public virtual void AdjustBoth2Step2 ( )
		{
		}

		public virtual void AdjustResonationStep2 ( )
		{
		}

		public virtual void AdjustStep2 ( )
		{
			AdjustBoth1Step2 ( );
			AdjustBoth2Step2 ( );
			AdjustResonationStep2 ( );
		}

		public virtual void CalculateBoth1Fecundity ( )
		{
		}

		public virtual void CalculateBoth2Fecundity ( )
		{
		}

		public virtual void CalculateResonationFecundity ( )
		{
		}

		public virtual void CalculateFecundity ( )
		{
			CalculateBoth1Fecundity ( );
			CalculateBoth2Fecundity ( );
			CalculateResonationFecundity ( );
		}
	}
}