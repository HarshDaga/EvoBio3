using EvoBio3.Core.Collections;
using EvoBio3.Core.Enums;

namespace EvoBio3.Collections
{
	public class Population : PopulationBase<Individual, IndividualGroup, Variables>
	{
		protected override void Create ( Variables v )
		{
			Cooperator1Group = new IndividualGroup ( IndividualType.Cooperator1, v.Cooperator1Quantity, v );
			Cooperator2Group = new IndividualGroup ( IndividualType.Cooperator2, v.Cooperator2Quantity, v );
			ResonationGroup  = new IndividualGroup ( IndividualType.Resonation, v.ResonationQuantity, v );
			DefectorGroup    = new IndividualGroup ( IndividualType.Defector, v.DefectorQuantity, v );
		}
	}
}