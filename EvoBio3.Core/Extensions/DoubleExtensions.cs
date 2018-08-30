using System;
using System.Diagnostics;

namespace EvoBio3.Core.Extensions
{
	public static class DoubleExtensions
	{
		[DebuggerStepThrough]
		public static double Square ( this double d ) =>
			d * d;

		[DebuggerStepThrough]
		public static double Limit ( this double d,
		                             double min,
		                             double max ) =>
			Math.Max ( min, Math.Min ( max, d ) );
	}
}