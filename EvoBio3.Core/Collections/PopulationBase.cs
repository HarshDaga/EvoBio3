using System;
using System.Collections.Generic;
using System.Linq;
using EvoBio3.Core.Extensions;
using EvoBio3.Core.Interfaces;

namespace EvoBio3.Core.Collections
{
	public abstract class PopulationBase<TIndividual, TGroup, TVariables> :
		IPopulation<TIndividual, TGroup, TVariables>
		where TIndividual : class, IIndividual
		where TGroup : IIndividualGroup<TIndividual>
		where TVariables : IVariables
	{
		public IList<TIndividual> AllIndividuals { get; set; }

		public TGroup Both1Group { get; set; }
		public TGroup ResonationGroup { get; set; }
		public TGroup Both2Group { get; set; }
		public TGroup NullGroup { get; set; }
		public TGroup[] AllGroups { get; set; }

		public void RebuildAllIndividualsList ( )
		{
			AllIndividuals = AllGroups.SelectMany ( x => x.Individuals ).ToList ( );
		}

		public void RebuildIndividualLists ( )
		{
			foreach ( var individualGroup in AllGroups )
				individualGroup.RebuildFromAll ( AllIndividuals );
		}

		public void Init ( TVariables variables )
		{
			Create ( variables );

			AllGroups = new[] {Both1Group, Both2Group, ResonationGroup, NullGroup};

			RebuildAllIndividualsList ( );
		}

		public (List<TIndividual> chosen, List<TIndividual> rejected) ChooseBy ( int amount,
		                                                                         Func<TIndividual, double> selector )
		{
			var backup = AllIndividuals.ToList ( );
			var cumulative = backup
				.Select ( selector )
				.CumulativeSum ( )
				.ToList ( );
			var total = cumulative.Last ( );

			var chosenIndividuals = new List<TIndividual> ( amount );

			for ( var i = 0; i < amount; i++ )
			{
				var target = Utility.NextDouble * total;
				var index = cumulative.BinarySearch ( target );
				if ( index < 0 )
					index = Math.Min ( ~index, backup.Count - 1 );

				var chosen = backup[index];
				chosenIndividuals.Add ( chosen );
				backup.RemoveAt ( index );
				cumulative.RemoveAt ( index );

				for ( var j = index; j < cumulative.Count; j++ )
					cumulative[j] -= selector ( chosen );
				total -= selector ( chosen );
			}

			return ( chosenIndividuals, backup );
		}

		public List<TIndividual> RepetitiveChooseBy ( int amount,
		                                              Func<TIndividual, double> selector )
		{
			var cumulative = AllIndividuals.Select ( selector ).CumulativeSum ( ).ToList ( );
			var total = cumulative.Last ( );

			var parents = new List<TIndividual> ( amount );
			for ( var i = 0; i < amount; i++ )
			{
				var targetAllocation = Utility.NextDouble * total;
				var index = cumulative.BinarySearch ( targetAllocation );
				if ( index < 0 )
					index = ~index;

				parents.Add ( AllIndividuals[index] );
			}

			return parents;
		}

		protected abstract void Create ( TVariables variables );
	}
}