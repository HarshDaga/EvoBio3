using System.Collections.Generic;
using System.Diagnostics;

namespace EvoBio3.Core.Extensions
{
	public static class EnumerableExtensions
	{
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