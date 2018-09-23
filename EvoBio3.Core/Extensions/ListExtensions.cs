using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace EvoBio3.Core.Extensions
{
	public static class ListExtensions
	{
		[DebuggerStepThrough]
		public static void Swap<T> ( this IList<T> list,
		                             int index1,
		                             int index2 )
		{
			if ( index1 == index2 )
				return;

			var temp = list[index1];
			list[index1] = list[index2];
			list[index2] = temp;
		}

		[DebuggerStepThrough]
		public static void Shuffle<T> ( this IList<T> list )
		{
			var n = list.Count;
			while ( n > 1 )
			{
				n--;
				var k = Utility.Srs.Next ( n + 1 );
				list.Swap ( k, n );
			}
		}

		[Pure]
		public static T AtPercentage<T> ( this IList<T> list,
		                                  double percentage )
		{
			var index = (int) Math.Round ( ( list.Count - 1d ) * percentage / 100d );
			return list[index];
		}

		[Pure]
		public static T AtPercentage<T> ( this IList<T> list,
		                                  double percentage,
		                                  T @default )
		{
			if ( percentage < 0 )
				return @default;

			var index = (int) Math.Round ( ( list.Count - 1d ) * percentage / 100d );
			return list[index];
		}
	}
}