using System.Diagnostics.Contracts;
using EvoBio3.Core.Collections;

// ReSharper disable InconsistentNaming

namespace EvoBio3.Collections
{
	public class Variables : VariablesBase
	{
		public double ReservationCostForCooperator1 { get; set; }
		public double ReservationCostForCooperator2 { get; set; }
		public double Beta { get; set; }
		public double Y { get; set; }
		public double R { get; set; }
		public int ReservationConditionsCutoff { get; set; }
		public int ResonationConditionsCutoffForResonationType { get; set; }
		public int ResonationConditionsCutoffForCooperator1 { get; set; }
		public int ResonationConditionsCutoffForCooperator2 { get; set; }
		public double ResonationQualityCutoffForResonationTypeVersion0 { get; set; }
		public double ResonationQualityCutoffForCooperator1WithNoReservationVersion0 { get; set; }
		public double ResonationQualityCutoffForCooperator2WithNoReservationVersion0 { get; set; }
		public double ResonationQualityCutoffForCooperator1WithReservationVersion0 { get; set; }
		public double ResonationQualityCutoffForCooperator2WithReservationVersion0 { get; set; }
		public double ReservationQualityCutoffForCooperator1Version0 { get; set; }
		public double ReservationQualityCutoffForCooperator2Version0 { get; set; }

		public override string ToString ( ) =>
			$"{base.ToString ( )}\n" +
			$"{nameof ( ReservationCostForCooperator1 )}: {ReservationCostForCooperator1}\n" +
			$"{nameof ( ReservationCostForCooperator2 )}: {ReservationCostForCooperator2}\n" +
			$"{nameof ( Beta )}: {Beta}, {nameof ( Y )}: {Y}, {nameof ( R )}: {R}\n" +
			$"{nameof ( ReservationConditionsCutoff )}: {ReservationConditionsCutoff}\n" +
			$"{nameof ( ResonationConditionsCutoffForResonationType )}: {ResonationConditionsCutoffForResonationType}\n" +
			$"{nameof ( ResonationConditionsCutoffForCooperator1 )}: {ResonationConditionsCutoffForCooperator1}\n" +
			$"{nameof ( ResonationConditionsCutoffForCooperator2 )}: {ResonationConditionsCutoffForCooperator2}\n" +
			$"{nameof ( ResonationQualityCutoffForResonationTypeVersion0 )}: {ResonationQualityCutoffForResonationTypeVersion0}\n" +
			$"{nameof ( ResonationQualityCutoffForCooperator1WithNoReservationVersion0 )}: {ResonationQualityCutoffForCooperator1WithNoReservationVersion0}\n" +
			$"{nameof ( ResonationQualityCutoffForCooperator2WithNoReservationVersion0 )}: {ResonationQualityCutoffForCooperator2WithNoReservationVersion0}\n" +
			$"{nameof ( ResonationQualityCutoffForCooperator1WithReservationVersion0 )}: {ResonationQualityCutoffForCooperator1WithReservationVersion0}\n" +
			$"{nameof ( ResonationQualityCutoffForCooperator2WithReservationVersion0 )}: {ResonationQualityCutoffForCooperator2WithReservationVersion0}\n" +
			$"{nameof ( ReservationQualityCutoffForCooperator1Version0 )}: {ReservationQualityCutoffForCooperator1Version0}\n" +
			$"{nameof ( ReservationQualityCutoffForCooperator2Version0 )}: {ReservationQualityCutoffForCooperator2Version0}\n" +
			$"{nameof ( ReservationQualityCutoffForCooperator1Version1 )}: {ReservationQualityCutoffForCooperator1Version1}\n" +
			$"{nameof ( ReservationQualityCutoffForCooperator2Version1 )}: {ReservationQualityCutoffForCooperator2Version1}\n" +
			$"{nameof ( ResonationQualityCutoffForResonationTypeVersion1 )}: {ResonationQualityCutoffForResonationTypeVersion1}\n" +
			$"{nameof ( ResonationQualityCutoffForCooperator1WithNoReservationVersion1 )}: {ResonationQualityCutoffForCooperator1WithNoReservationVersion1}\n" +
			$"{nameof ( ResonationQualityCutoffForCooperator2WithNoReservationVersion1 )}: {ResonationQualityCutoffForCooperator2WithNoReservationVersion1}\n" +
			$"{nameof ( ResonationQualityCutoffForCooperator1WithReservationVersion1 )}: {ResonationQualityCutoffForCooperator1WithReservationVersion1}\n" +
			$"{nameof ( ResonationQualityCutoffForCooperator2WithReservationVersion1 )}: {ResonationQualityCutoffForCooperator2WithReservationVersion1}";

		[Pure]
		public override VariablesBase Clone ( ) => (Variables) MemberwiseClone ( );
	}
}