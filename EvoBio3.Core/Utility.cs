using System;
using System.Collections.Generic;
using System.Diagnostics;
using MathNet.Numerics.Distributions;
using MathNet.Numerics.Random;
using MathNet.Numerics.Statistics;
using static System.Math;

namespace EvoBio3.Core
{
	public static class Utility
	{
		public static readonly Random Rand;
		public static readonly SystemRandomSource Srs;

		public static double NextDouble => Srs.NextDouble ( );

		static Utility ( )
		{
			Rand = new Random ( Environment.TickCount );
			Srs  = new SystemRandomSource ( Rand.Next ( ), true );
		}

		[DebuggerStepThrough]
		public static double NextGaussian ( double mean,
		                                    double sd ) =>
			Normal.Sample ( mean, sd );

		[DebuggerStepThrough]
		public static double NextGaussianNonNegative ( double mean,
		                                               double sd )
		{
			var randomGaussianValue = Normal.Sample ( mean, sd );
			return Max ( 0, randomGaussianValue );
		}

		[DebuggerStepThrough]
		public static double NextGaussianInRange ( double mean,
		                                           double sd,
		                                           double min,
		                                           double max )
		{
			var randomGaussianValue = Normal.Sample ( mean, sd );
			return Max ( Min ( randomGaussianValue, max ), min );
		}

		[DebuggerStepThrough]
		public static int NextGaussianIntInRange ( double mean,
		                                           double sd,
		                                           double min,
		                                           double max )
		{
			var randomGaussianValue = Normal.Sample ( mean, sd );
			var result = Max ( Min ( randomGaussianValue, max ), min );

			return (int) Round ( result );
		}

		// ReSharper disable once InconsistentNaming
		[DebuggerStepThrough]
		public static double NextGaussianCV ( double mean,
		                                      double cv )
		{
			var normalDist = Normal.WithMeanStdDev ( mean, cv * mean );
			var randomGaussianValue = normalDist.Sample ( );
			return randomGaussianValue;
		}

		[DebuggerStepThrough]
		public static double[] NextGaussianSymbols ( double mean,
		                                             double sd,
		                                             int n )
		{
			var normalDist = new Normal ( mean, sd );
			var samples = new double[n];
			normalDist.Samples ( samples );
			return samples;
		}

		[DebuggerStepThrough]
		public static double[] NextGaussianNonNegativeSymbols ( double mean,
		                                                        double sd,
		                                                        int n )
		{
			var samples = new double[n];
			Normal.Samples ( samples, mean, sd );
			for ( var i = 0; i < n; i++ )
				if ( samples[i] < 0 )
					samples[i] = 0;

			return samples;
		}

		[DebuggerStepThrough]
		public static double NextGaussian ( double mean,
		                                    double sd,
		                                    int precision )
		{
			var randNormal = NextGaussian ( mean, sd );

			return Round ( randNormal, precision );
		}

		public static ConfidenceInterval CalculateConfidenceInterval (
			IList<double> list,
			double z )
		{
			var (mean, sd) = list.MeanStandardDeviation ( );
			var stdError = sd / Sqrt ( list.Count );

			return new ConfidenceInterval ( mean, z * stdError );
		}
	}
}