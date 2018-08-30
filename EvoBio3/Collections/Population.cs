using EvoBio3.Core.Collections;
using EvoBio3.Core.Enums;

namespace EvoBio3.Collections
{
	public class Population : PopulationBase<Individual, IndividualGroup, Variables>
	{
		protected override void Create ( Variables v )
		{
			Both1Group      = new IndividualGroup ( IndividualType.Both1, v.Both1Quantity, v );
			Both2Group      = new IndividualGroup ( IndividualType.Both2, v.Both2Quantity, v );
			ResonationGroup = new IndividualGroup ( IndividualType.Resonation, v.ResonationQuantity, v );
			NullGroup       = new IndividualGroup ( IndividualType.Null, v.NullQuantity, v );
		}
	}
}