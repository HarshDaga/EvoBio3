using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace EvoBio3.Core.Extensions
{
	public static class EnumerableExtensions
	{
		public static (List<T> chosen, List<T> rejected) ChooseBy<T> ( this IEnumerable<T> allIndividuals,
		                                                               int amount,
		                                                               Func<T, double> selector )
		{
			var backup = allIndividuals.ToList ( );
			var cumulative = backup
				.Select ( selector )
				.CumulativeSum ( )
				.ToList ( );
			var total = cumulative.Last ( );

			var chosenIndividuals = new List<T> ( amount );

			for ( var i = 0; i < amount; i++ )
			{
				var target = Utility.NextDouble * total;
				var index = cumulative.BinarySearch ( target );
				if ( index < 0 )
					index = Math.Min ( ~index, backup.Count - 1 );

				var chosen = backup[index];
				chosenIndividuals.Add ( chosen );
				backup.RemoveAt ( index );
				cumulative.RemoveAt ( index );

				for ( var j = index; j < cumulative.Count; j++ )
					cumulative[j] -= selector ( chosen );
				total -= selector ( chosen );
			}

			return ( chosenIndividuals, backup );
		}

		[DebuggerStepThrough]
		public static IEnumerable<double> CumulativeSum ( this IEnumerable<double> sequence )
		{
			double sum = 0;
			foreach ( var item in sequence )
			{
				sum += item;
				yield return sum;
			}
		}
	}
}