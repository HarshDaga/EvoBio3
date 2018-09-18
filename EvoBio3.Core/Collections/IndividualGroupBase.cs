using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EvoBio3.Core.Enums;
using EvoBio3.Core.Extensions;
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

		public string ToTable ( )
		{
			return ToTable ( x => new
				{
					x.Id,
					Qg = $"{x.GeneticQuality:F4}",
					Qp = $"{x.PhenotypicQuality:F4}"
				}
			);
		}

		public string ToTable ( Func<TIndividual, object> selector )
		{
			var table = Individuals.ToTable ( selector );

			return $"{Type}\n{table}";
		}

		public IEnumerator<TIndividual> GetEnumerator ( ) => Individuals.GetEnumerator ( );

		IEnumerator IEnumerable.GetEnumerator ( ) => ( (IEnumerable) Individuals ).GetEnumerator ( );

		public override string ToString ( ) =>
			$"{Type}, {Count}, {nameof ( TotalFecundity )}: {TotalFecundity,8:F4}";
	}
}