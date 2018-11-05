﻿using EvoBio3.Core.Interfaces;
using NLog;

namespace EvoBio3.Core.Collections
{
	public abstract class AdjustmentRulesBase<TIndividual, TGroup, TVariables, TIteration> :
		IAdjustmentRules<TIndividual, TGroup, TVariables, TIteration>
		where TIndividual : class, IIndividual, new ( )
		where TGroup : IIndividualGroup<TIndividual>, new ( )
		where TVariables : IVariables
		where TIteration : class, ISingleIteration<TIndividual, TGroup, TVariables>
	{
		// ReSharper disable once StaticMemberInGenericType
		protected static Logger Logger { get; set; } = LogManager.GetCurrentClassLogger ( );
		public TIteration Iteration { get; set; }
		public TVariables V => Iteration.V;
		public int TotalPerished => Iteration.Step1Rejects.Count + Iteration.Step2Rejects.Count;
		public bool IsLoggingEnabled => Iteration.IsLoggingEnabled;

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

		public virtual void AdjustStep1 ( )
		{
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