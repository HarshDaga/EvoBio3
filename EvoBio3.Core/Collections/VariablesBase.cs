using System.Diagnostics.Contracts;
using EvoBio3.Core.Interfaces;

namespace EvoBio3.Core.Collections
{
	public abstract class VariablesBase : IVariables
	{
		public int PopulationSize => Both1Quantity + Both2Quantity + ResonationQuantity + NullQuantity;
		public int Both1Quantity { get; set; }
		public int Both2Quantity { get; set; }
		public int ResonationQuantity { get; set; }
		public int NullQuantity { get; set; }
		public double SdGenetic { get; set; }
		public double SdPheno { get; set; }
		public double MeanPerishStep1 { get; set; }
		public double SdPerishStep1 { get; set; }
		public double MeanPerishStep2 { get; set; }
		public double SdPerishStep2 { get; set; }
		public int Generations { get; set; }
		public int Iterations { get; set; }
		public double Z { get; set; }
		public double B1 { get; set; }
		public double B2 { get; set; }
		public double Pr { get; set; }
		public double Prb1 { get; set; }
		public double Prb2 { get; set; }
		public double Pb1 { get; set; }
		public double Pb2 { get; set; }
		public bool IsConfidenceIntervalsRequested { get; set; }
		public bool ConsiderAllGenerations { get; set; }

		[Pure]
		IVariables IVariables.Clone ( ) => (VariablesBase) MemberwiseClone ( );

		public override string ToString ( ) =>
			$"{nameof ( Both1Quantity )}: {Both1Quantity}, " +
			$"{nameof ( Both2Quantity )}: {Both2Quantity}, " +
			$"{nameof ( ResonationQuantity )}: {ResonationQuantity}, " +
			$"{nameof ( NullQuantity )}: {NullQuantity}\n" +
			$"{nameof ( Generations )}: {Generations}, " +
			$"{nameof ( Iterations )}: {Iterations}\n" +
			$"{nameof ( SdGenetic )}: {SdGenetic}, " +
			$"{nameof ( SdPheno )}: {SdPheno}\n" +
			$"{nameof ( MeanPerishStep1 )}: {MeanPerishStep1}, " +
			$"{nameof ( SdPerishStep1 )}: {SdPerishStep1}\n" +
			$"{nameof ( MeanPerishStep2 )}: {MeanPerishStep2}, " +
			$"{nameof ( SdPerishStep2 )}: {SdPerishStep2}, " +
			$"{nameof ( Z )}: {Z}\n";

		[Pure]
		public virtual VariablesBase Clone ( ) => (VariablesBase) MemberwiseClone ( );
	}
}