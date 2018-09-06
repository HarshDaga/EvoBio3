using System;
using EvoBio3.Core.Enums;
using EvoBio3.Core.Interfaces;

namespace EvoBio3.Core.Collections
{
	public abstract class IndividualBase : IIndividual, IEquatable<IndividualBase>
	{
		public int Id { get; }
		public Guid Guid { get; } = Guid.NewGuid ( );
		public IndividualType Type { get; }
		public double GeneticQuality { get; protected set; }
		public double PhenotypicQuality { get; protected set; }
		public double Fecundity { get; set; }
		public double AdjustedFecundity { get; set; }

		public int OffspringCount { get; set; }

		public string Name => $"{Type}_{Id}";
		public string PaddedName => $"{Name,-15}";

		protected IndividualBase ( IndividualType type,
		                           int id )
		{
			Type = type;
			Id   = id;
		}

		bool IEquatable<IndividualBase>.Equals ( IndividualBase other ) => Equals ( other );

		public bool Equals ( IIndividual other )
		{
			if ( ReferenceEquals ( null, other ) ) return false;
			if ( ReferenceEquals ( this, other ) ) return true;
			return Guid.Equals ( other.Guid );
		}

		public override bool Equals ( object obj )
		{
			if ( ReferenceEquals ( null, obj ) ) return false;
			if ( ReferenceEquals ( this, obj ) ) return true;
			if ( obj.GetType ( ) != GetType ( ) ) return false;
			return Equals ( (IndividualBase) obj );
		}

		public override int GetHashCode ( ) => Guid.GetHashCode ( );

		public abstract string ToDetailedString ( );

		public static bool operator == ( IndividualBase left,
		                                 IndividualBase right ) => Equals ( left, right );

		public static bool operator != ( IndividualBase left,
		                                 IndividualBase right ) => !Equals ( left, right );
	}
}