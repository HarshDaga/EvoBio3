using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EvoBio3.Core.Enums;
using EvoBio3.Core.Interfaces;

namespace EvoBio3.Core.Collections
{
	public abstract class IndividualGroupBase<TIndividual> :
		IIndividualGroup<TIndividual>
		where TIndividual : class, IIndividual
	{
		public IndividualType Type { get; }
		public List<TIndividual> Individuals { get; set; }
		public double LostFecundity { get; set; }
		public double TotalAdjusted { get; set; }
		public double TotalFecundity { get; set; }

		public int Count => Individuals.Count;

		protected IndividualGroupBase ( IndividualType type,
		                                int count )
		{
			Type        = type;
			Individuals = new List<TIndividual> ( count );
		}

		public void RebuildFromAll ( IEnumerable<TIndividual> allIndividuals ) =>
			Individuals = allIndividuals.Where ( x => x.Type == Type ).ToList ( );

		public double CalculateTotalFecundity ( ) =>
			TotalFecundity = Individuals.Sum ( x => x.Fecundity );

		public abstract double CalculateLostFecundity ( );

		public IEnumerator<TIndividual> GetEnumerator ( ) => Individuals.GetEnumerator ( );

		IEnumerator IEnumerable.GetEnumerator ( ) => ( (IEnumerable) Individuals ).GetEnumerator ( );
	}
}