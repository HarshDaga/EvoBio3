namespace EvoBio3.Core.Interfaces
{
	public interface IVariables
	{
		int PopulationSize { get; }
		int Cooperator1Quantity { get; set; }
		int Cooperator2Quantity { get; set; }
		int ResonationQuantity { get; set; }
		int DefectorQuantity { get; set; }
		double SdQuality { get; set; }
		double SdPheno { get; set; }
		double MeanPerishStep1 { get; set; }
		double SdPerishStep1 { get; set; }
		double MeanPerishStep2 { get; set; }
		double SdPerishStep2 { get; set; }
		int Generations { get; set; }
		int Runs { get; set; }
		double Z { get; set; }
		double ReservationQualityCutoffForCooperator1Version1 { get; set; }
		double ReservationQualityCutoffForCooperator2Version1 { get; set; }
		double ResonationQualityCutoffForResonationTypeVersion1 { get; set; }
		double ResonationQualityCutoffForCooperator1WithNoReservationVersion1 { get; set; }
		double ResonationQualityCutoffForCooperator2WithNoReservationVersion1 { get; set; }
		double ResonationQualityCutoffForCooperator1WithReservationVersion1 { get; set; }
		double ResonationQualityCutoffForCooperator2WithReservationVersion1 { get; set; }
		bool IncludeConfidenceIntervals { get; set; }
		bool ConfidenceIntervalsIncludeGenerationsFollowingFixation { get; set; }

		IVariables Clone ( );
		string ToString ( );
	}
}