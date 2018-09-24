using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Tababular;

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
			var sum = 0d;
			foreach ( var item in sequence )
			{
				sum += item;
				yield return sum;
			}
		}

		[DebuggerStepThrough]
		public static IEnumerable<int> CumulativeSum ( this IEnumerable<int> sequence )
		{
			var sum = 0;
			foreach ( var item in sequence )
			{
				sum += item;
				yield return sum;
			}
		}

		public static string ToTable<T> ( this IEnumerable<T> enumerable,
		                                  Func<T, object> selector,
		                                  int maxWidth = 80 )
		{
			var hints = new Hints {MaxTableWidth = maxWidth};
			var formatter = new TableFormatter ( hints );

			return formatter.FormatObjects ( enumerable.Select ( selector ) );
		}

		public static string Join<T> ( this IEnumerable<T> enumerable,
		                               string separator ) =>
			string.Join ( separator, enumerable );
	}
}