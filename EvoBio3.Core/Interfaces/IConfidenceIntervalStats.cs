using System.Collections.Generic;
using EvoBio3.Core.Enums;

namespace EvoBio3.Core.Interfaces
{
	public interface IConfidenceIntervalStats
	{
		int Generations { get; }
		int Iterations { get; }
		IDictionary<IndividualType, List<List<double>>> Stats { get; }

		List<Dictionary<IndividualType, ConfidenceInterval>> Summary { get; }
		double Z { get; }

		ConfidenceInterval this [ int generation,
		                          IndividualType type ] { get; }

		void Add ( IDictionary<IndividualType, List<int>> iterationResult );

		void Compute ( );

		string ToTable ( );
		void PrintToCsv ( string fileName );
		void PrintToFile ( string fileName );
		void PrintToConsole ( );
	}
}