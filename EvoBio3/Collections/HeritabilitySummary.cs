using EvoBio3.Core.Interfaces;

namespace EvoBio3.Collections
{
	public class HeritabilitySummary : IHeritabilitySummary
	{
		public double PhenotypicQuality
		{
			get => Values[0];
			set => Values[0] = value;
		}

		public double VariancePhenotypicQuality
		{
			get => Values[1];
			set => Values[1] = value;
		}

		public double CovariancePhenotypicQuality
		{
			get => Values[2];
			set => Values[2] = value;
		}

		public double GeneticQuality
		{
			get => Values[3];
			set => Values[3] = value;
		}

		public double VarianceGeneticQuality
		{
			get => Values[4];
			set => Values[4] = value;
		}

		public double CovarianceGeneticQuality
		{
			get => Values[5];
			set => Values[5] = value;
		}

		public double Reproduction
		{
			get => Values[6];
			set => Values[6] = value;
		}

		public double VarianceReproduction
		{
			get => Values[7];
			set => Values[7] = value;
		}

		public double CovarianceReproduction
		{
			get => Values[8];
			set => Values[8] = value;
		}

		public int ValueCount { get; }
		public double[] Values { get; }

		public HeritabilitySummary ( )
		{
			ValueCount = 9;
			Values     = new double[ValueCount];
		}

		public override string ToString ( ) =>
			$"Phenotypic Quality:                 {PhenotypicQuality,8:F8}\n" +
			$"Variance Phenotypic Quality:        {VariancePhenotypicQuality,8:F8}\n" +
			$"Covariance Phenotypic Quality:      {CovariancePhenotypicQuality,8:F8}\n\n" +
			$"Genetic Quality:                    {GeneticQuality,8:F8}\n" +
			$"Variance Genetic Quality:           {VarianceGeneticQuality,8:F8}\n" +
			$"Covariance Genetic Quality:         {CovarianceGeneticQuality,8:F8}\n\n" +
			$"Reproduction:                       {Reproduction,8:F8}\n" +
			$"Variance Reproduction:              {VarianceReproduction,8:F8}\n" +
			$"Covariance Reproduction:            {CovarianceReproduction,8:F8}";
	}
}