using System;
using System.Collections.Generic;
using EvoBio3.Core.Enums;

namespace EvoBio3.Core.Interfaces
{
	public interface IIndividualGroup<TIndividual> : IEnumerable<TIndividual>
		where TIndividual : class, IIndividual
	{
		IndividualType Type { get; }
		int Count { get; }
		List<TIndividual> Individuals { get; set; }
		double TotalFecundity { get; set; }
		double LostFecundity { get; set; }
		double TotalAdjusted { get; set; }

		double CalculateLostFecundity ( );
		double CalculateTotalFecundity ( );
		void RebuildFromAll ( IEnumerable<TIndividual> allIndividuals );
		string ToTable ( );
		string ToTable ( Func<TIndividual, object> selector );
	}
}