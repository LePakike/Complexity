using System.Collections.Generic;
using System.Linq;
using Complexity.ApertureMetric;

namespace Complexity.EsapiApertureMetric
{
    // Subclass of ComplexityMetric that represents the edge metric
    public class ModulationComplexityScore : ComplexityMetric
    {
        // Returns the unweighted edge metrics of a list of apertures
        protected override double[] CalculatePerAperture(IEnumerable<Aperture> apertures)
        {
            double aav_norm = 0;
            foreach (Aperture aperature in apertures)
            {
                List<double> pos_left = new List<double>();
                List<double> pos_right = new List<double>();
                List<double[]> leaf_pair_positions = new List<double[]>();

                foreach (LeafPair lp in aperature.LeafPairs)
                {
                    if (!lp.IsOutsideJaw())
                    {
                        pos_left.Add(lp.Left);
                        pos_right.Add(lp.Right);
                        leaf_pair_positions.Add(new double[] { lp.Left, lp.Right });
                    }
                }
                aav_norm += System.Math.Abs(pos_left.Max() - pos_right.Max());
            }
            ApertureMetric.LeafSequenceVariability metric = new ApertureMetric.LeafSequenceVariability();
            return (from aperture in apertures
                    select metric.Calculate(aperture, aav_norm)).ToArray();
        }
    }
}