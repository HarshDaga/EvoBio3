namespace EvoBio3.Versions
{
	public class ConsiderAllGenerationsVersion : SingleIterationBaseVersion
	{
		public override void AfterLastGeneration ( )
		{
			for ( var i = GenerationsPassed; i < V.Generations; i++ )
				AddGenerationHistory ( );
		}
	}
}