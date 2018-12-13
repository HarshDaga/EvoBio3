using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using Alba.CsConsoleFormat;
using CsvHelper;
using EvoBio3.Core.Enums;
using EvoBio3.Core.Interfaces;
using MoreLinq;
using static EnumsNET.Enums;

namespace EvoBio3.Core.Collections
{
	[SuppressMessage ( "ReSharper", "UnusedAutoPropertyAccessor.Local" )]
	[SuppressMessage ( "ReSharper", "MemberCanBePrivate.Local" )]
	public class ConfidenceIntervalStats : IConfidenceIntervalStats
	{
		public static IEnumerable<IndividualType> IndividualTypes => GetValues<IndividualType> ( );

		public int Generations { get; }
		public int Iterations { get; }
		public IDictionary<IndividualType, List<List<double>>> Stats { get; }
		public List<Dictionary<IndividualType, ConfidenceInterval>> Summary { get; private set; }
		public double Z { get; }

		public ConfidenceInterval this [ int generation,
		                                 IndividualType type ]
		{
			get
			{
				if ( Summary.Count < generation )
					return default;
				if ( Summary[generation].TryGetValue ( type, out var result ) )
					return result;
				return default;
			}
		}

		public ConfidenceIntervalStats ( int generations,
		                                 int iterations,
		                                 double z )
		{
			Generations = generations + 1;
			Iterations  = iterations;
			Z           = z;
			Stats       = new Dictionary<IndividualType, List<List<double>>> ( );

			foreach ( var type in IndividualTypes )
			{
				Stats[type] = new List<List<double>> ( Generations );
				for ( var i = 0; i < Generations; i++ )
					Stats[type].Add ( new List<double> ( iterations ) );
			}
		}

		public void Add ( IDictionary<IndividualType, List<int>> iterationResult )
		{
			foreach ( var kp in iterationResult )
			{
				var type = kp.Key;
				var genCounts = kp.Value;

				for ( var gen = 0; gen < genCounts.Count; gen++ )
					Stats[type][gen].Add ( genCounts[gen] );
			}
		}

		public void Compute ( )
		{
			Summary = new List<Dictionary<IndividualType, ConfidenceInterval>> ( Generations );

			for ( var i = 0; i < Generations; i++ )
				Summary.Add (
					new Dictionary<IndividualType, ConfidenceInterval> ( GetMemberCount<IndividualType> ( ) ) );

			foreach ( var stat in Stats )
			{
				var type = stat.Key;
				var intervals = stat.Value.Select ( x => Utility.CalculateConfidenceInterval ( x, Z ) ).ToList ( );
				for ( var i = 0; i < intervals.Count; i++ )
					Summary[i][type] = intervals[i];
			}
		}

		public string ToTable ( )
		{
			var doc = CreateDocument ( );

			var sw = new StringWriter ( );
			ConsoleRenderer.RenderDocumentToText ( doc, new TextRenderTarget ( sw ),
			                                       Rect.FromBounds ( 0, 0, 100000, 100000 ) );

			return sw.GetStringBuilder ( ).ToString ( );
		}

		public void PrintToCsv ( string fileName )
		{
			using ( var csv = new CsvWriter ( File.CreateText ( fileName ) ) )
			{
				csv.WriteRecords ( Summary.Select ( ( x,
				                                      i ) => new CsvRecord ( i, x ) ) );
			}
		}

		public void PrintToFile ( string fileName )
		{
			File.WriteAllText ( fileName, ToTable ( ) );
		}

		public void PrintToConsole ( )
		{
			var doc = CreateDocument ( );

			ConsoleRenderer.RenderDocument ( doc );
		}

		private Document CreateDocument ( ) => new Document
		{
			Children =
			{
				new Grid
				{
					Stroke      = LineThickness.Double,
					StrokeColor = ConsoleColor.Green,
					Columns =
					{
						new Column {Width = GridLength.Auto},
						IndividualTypes.Select ( x => new Column {Width = GridLength.Auto} )
					},
					Children =
					{
						new Cell ( "Generation" ),
						IndividualTypes.Select ( x => new Cell ( x ) ),
						Summary.Select ( ( x,
						                   i ) =>
							                 x.Values
								                 .Select ( y => new Cell ( $"{y:F}" ) )
								                 .Insert ( new[] {new Cell ( i )}, 0 ) )
					}
				}
			}
		};

		private class CsvRecord
		{
			public int Generation { get; }
			public double Cooperator1High { get; }
			public double Cooperator1Low { get; }
			public double Cooperator1Mean { get; }
			public double Cooperator2High { get; }
			public double Cooperator2Low { get; }
			public double Cooperator2Mean { get; }
			public double ResonationHigh { get; }
			public double ResonationLow { get; }
			public double ResonationMean { get; }
			public double DefectorHigh { get; }
			public double DefectorLow { get; }
			public double DefectorMean { get; }

			public CsvRecord ( int generation,
			                   IDictionary<IndividualType, ConfidenceInterval> record )
			{
				Generation      = generation;
				Cooperator1Low  = record[IndividualType.Cooperator1].Low;
				Cooperator1Mean = record[IndividualType.Cooperator1].Mean;
				Cooperator1High = record[IndividualType.Cooperator1].High;
				Cooperator2Low  = record[IndividualType.Cooperator2].Low;
				Cooperator2Mean = record[IndividualType.Cooperator2].Mean;
				Cooperator2High = record[IndividualType.Cooperator2].High;
				ResonationLow   = record[IndividualType.Resonation].Low;
				ResonationMean  = record[IndividualType.Resonation].Mean;
				ResonationHigh  = record[IndividualType.Resonation].High;
				DefectorLow     = record[IndividualType.Defector].Low;
				DefectorMean    = record[IndividualType.Defector].Mean;
				DefectorHigh    = record[IndividualType.Defector].High;
			}
		}
	}
}