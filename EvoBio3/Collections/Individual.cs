using System;
using System.Collections.Generic;
using EvoBio3.Core.Collections;
using EvoBio3.Core.Enums;

namespace EvoBio3.Collections
{
	public class Individual : IndividualBase, IEquatable<Individual>
	{
		public double S;

		public Individual ( ) : base ( default, default )
		{
		}

		public Individual ( IndividualType type,
		                    int id ) : base ( type, id )
		{
		}

		public Individual (
			IndividualType type,
			int id,
			double geneticQuality,
			double phenotypicQuality
		) : base ( type, id )
		{
			GeneticQuality    = geneticQuality;
			PhenotypicQuality = phenotypicQuality;
			S                 = phenotypicQuality;
			Fecundity         = phenotypicQuality;
		}

		public bool Equals ( Individual other ) =>
			other != null && Id == other.Id && Type == other.Type;

		public Individual Reproduce (
			int id,
			double geneticQuality,
			double phenotypicQuality
		)
		{
			++OffspringCount;
			return new Individual ( Type, id, geneticQuality, phenotypicQuality );
		}

		public override bool Equals ( object obj ) =>
			Equals ( obj as Individual );

		public override int GetHashCode ( )
		{
			var hashCode = 1325953389;
			hashCode = hashCode * -1521134295 + Id.GetHashCode ( );
			hashCode = hashCode * -1521134295 + Type.GetHashCode ( );
			return hashCode;
		}

		public override string ToString ( ) =>
			$"{PaddedName} GeneticQuality: {GeneticQuality,8:F4} PhenotypicQuality: {PhenotypicQuality,8:F4}";

		public override string ToDetailedString ( ) =>
			$"{PaddedName}\n" +
			$"    GeneticQuality: {GeneticQuality,8:F4} PhenotypicQuality: {PhenotypicQuality,8:F4}" +
			$"    Fecundity = {Fecundity,8:F4} AdjustedFecundity = {AdjustedFecundity,8:F4}";

		public static bool operator == ( Individual individual1,
		                                 Individual individual2 ) =>
			EqualityComparer<Individual>.Default.Equals ( individual1, individual2 );

		public static bool operator != ( Individual individual1,
		                                 Individual individual2 ) =>
			!( individual1 == individual2 );
	}
}