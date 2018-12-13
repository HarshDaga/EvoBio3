using System.Linq;
using EvoBio3.Core;
using EvoBio3.Core.Collections;
using EvoBio3.Core.Enums;
using EvoBio3.Core.Interfaces;

namespace EvoBio3.Collections
{
	public class IndividualGroup : IndividualGroupBase<Individual>
	{
		public double TotalPhenoQuality { get; protected set; }

		public IndividualGroup ( ) : base ( default, default )
		{
		}

		public IndividualGroup ( IndividualType type,
		                         int count,
		                         IVariables v ) : base ( type, count )
		{
			var geneticQualities = Utility.NextGaussianNonNegativeSymbols ( 10,
			                                                                v.SdQuality,
			                                                                count );
			for ( var i = 0; i < count; i++ )
			{
				var individual = new Individual (
					type, i + 1,
					geneticQualities[i],
					Utility.NextGaussianNonNegative ( geneticQualities[i], v.SdPheno )
				);

				Individuals.Add ( individual );
			}
		}

		public override double CalculateLostFecundity ( )
		{
			TotalPhenoQuality = Individuals.Sum ( x => x.PhenotypicQuality );

			return LostFecundity = TotalPhenoQuality - TotalFecundity;
		}

		public override string ToString ( ) =>
			$"{Type}, {Count}, {nameof ( TotalPhenoQuality )}: {TotalPhenoQuality,8:F4}, {nameof ( TotalFecundity )}: {TotalFecundity,8:F4}";
	}
}