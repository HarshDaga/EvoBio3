using System.Collections.Generic;
using EvoBio3.Core.Enums;

namespace EvoBio3.Core.Interfaces
{
	public interface ISimulation
	{
		Dictionary<int, int> GenerationsCount { get; }
		Dictionary<Winner, int> Wins { get; }
		IConfidenceIntervalStats ConfidenceIntervalStats { get; }

		void Run ( );
	}
}