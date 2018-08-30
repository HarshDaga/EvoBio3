﻿namespace EvoBio3.Core.Interfaces
{
	public interface IVariables
	{
		int PopulationSize { get; }
		int Both1Quantity { get; set; }
		int Both2Quantity { get; set; }
		int ResonationQuantity { get; set; }
		int NullQuantity { get; set; }
		double SdGenetic { get; set; }
		double SdPheno { get; set; }
		double MeanPerishStep1 { get; set; }
		double SdPerishStep1 { get; set; }
		double MeanPerishStep2 { get; set; }
		double SdPerishStep2 { get; set; }
		int Generations { get; set; }
		int Iterations { get; set; }
		double Z { get; set; }
		bool IsConfidenceIntervalsRequested { get; set; }

		IVariables Clone ( );
		string ToString ( );
	}
}