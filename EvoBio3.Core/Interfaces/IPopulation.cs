using System;
using System.Collections.Generic;

namespace EvoBio3.Core.Interfaces
{
	public interface IPopulation<TIndividual, TGroup, in TVariables>
		where TIndividual : class, IIndividual
		where TGroup : IIndividualGroup<TIndividual>
		where TVariables : IVariables
	{
		TGroup[] AllGroups { get; set; }
		IList<TIndividual> AllIndividuals { get; set; }
		TGroup Cooperator1Group { get; set; }
		TGroup Cooperator2Group { get; set; }
		TGroup ResonationGroup { get; set; }
		TGroup DefectorGroup { get; set; }

		void Init ( TVariables variables );

		(List<TIndividual> chosen, List<TIndividual> rejected) ChooseBy ( int amount,
		                                                                  Func<TIndividual, double> selector );

		void RebuildAllIndividualsList ( );
		void RebuildIndividualLists ( );
	}
}