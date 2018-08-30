using System;
using EvoBio3.Core.Enums;

namespace EvoBio3.Core.Interfaces
{
	public interface IIndividual : IEquatable<IIndividual>
	{
		Guid Guid { get; }
		IndividualType Type { get; }
		int Id { get; }
		int OffspringCount { get; set; }
		double AdjustedFecundity { get; set; }
		double Fecundity { get; set; }
		string Name { get; }
		string PaddedName { get; }

		bool Equals ( object obj );
		int GetHashCode ( );
		string ToDetailedString ( );
	}
}