using System.Diagnostics.Contracts;
using EvoBio3.Core.Interfaces;

namespace EvoBio3.Core.Collections
{
	public abstract class VariablesBase : IVariables
	{
		public int PopulationSize => Cooperator1Quantity + Cooperator2Quantity + ResonationQuantity + DefectorQuantity;
		public int Cooperator1Quantity { get; set; }
		public int Cooperator2Quantity { get; set; }
		public int ResonationQuantity { get; set; }
		public int DefectorQuantity { get; set; }
		public double SdQuality { get; set; }
		public double SdPheno { get; set; }
		public double MeanPerishStep1 { get; set; }
		public double SdPerishStep1 { get; set; }
		public double MeanPerishStep2 { get; set; }
		public double SdPerishStep2 { get; set; }
		public int Generations { get; set; }
		public int Runs { get; set; }
		public double Z { get; set; }
		public double ReservationQualityCutoffForCooperator1Version1 { get; set; }
		public double ReservationQualityCutoffForCooperator2Version1 { get; set; }
		public double ResonationQualityCutoffForResonationTypeVersion1 { get; set; }
		public double ResonationQualityCutoffForCooperator1WithNoReservationVersion1 { get; set; }
		public double ResonationQualityCutoffForCooperator2WithNoReservationVersion1 { get; set; }
		public double ResonationQualityCutoffForCooperator1WithReservationVersion1 { get; set; }
		public double ResonationQualityCutoffForCooperator2WithReservationVersion1 { get; set; }
		public bool IncludeConfidenceIntervals { get; set; }
		public bool ConfidenceIntervalsIncludeGenerationsFollowingFixation { get; set; }

		[Pure]
		IVariables IVariables.Clone ( ) => (VariablesBase) MemberwiseClone ( );

		public override string ToString ( ) =>
			$"{nameof ( Cooperator1Quantity )}: {Cooperator1Quantity}, " +
			$"{nameof ( Cooperator2Quantity )}: {Cooperator2Quantity}, " +
			$"{nameof ( ResonationQuantity )}: {ResonationQuantity}, " +
			$"{nameof ( DefectorQuantity )}: {DefectorQuantity}\n" +
			$"{nameof ( Generations )}: {Generations}, " +
			$"{nameof ( Runs )}: {Runs}\n" +
			$"{nameof ( SdQuality )}: {SdQuality}, " +
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