using System.Diagnostics.Contracts;
using EvoBio3.Core.Collections;

// ReSharper disable InconsistentNaming

namespace EvoBio3.Collections
{
	public class Variables : VariablesBase
	{
		public double C1 { get; set; }
		public double C2 { get; set; }
		public double Beta { get; set; }
		public double Y { get; set; }
		public double R { get; set; }
		public double PiD { get; set; }
		public double PiCR { get; set; }
		public double PiCB1 { get; set; }
		public double PiCB2 { get; set; }
		public double Qr { get; set; }
		public double Qb1 { get; set; }
		public double Qb2 { get; set; }
		public double Qt { get; set; }
		public double Qu { get; set; }

		public override string ToString ( ) =>
			$"{base.ToString ( )}\n" +
			$"{nameof ( C1 )}: {C1}, {nameof ( C2 )}: {C2}\n" +
			$"{nameof ( Beta )}: {Beta}, {nameof ( Y )}: {Y}, {nameof ( R )}: {R}\n" +
			$"{nameof ( PiD )}: {PiD}, {nameof ( PiCR )}: {PiCR}, {nameof ( PiCB1 )}: {PiCB1}, {nameof ( PiCB2 )}: {PiCB2}\n" +
			$"{nameof ( Qr )}: {Qr}, {nameof ( Qb1 )}: {Qb1}, {nameof ( Qb2 )}: {Qb2}, {nameof ( Qt )}: {Qt}, {nameof ( Qu )}: {Qu}\n" +
			$"{nameof ( B1 )}: {B1}, {nameof ( B2 )}: {B2}\n" +
			$"{nameof ( Pr )}: {Pr}, {nameof ( Pb1 )}: {Pb1}, {nameof ( Pb2 )}: {Pb2}";

		[Pure]
		public override VariablesBase Clone ( ) => (Variables) MemberwiseClone ( );
	}
}