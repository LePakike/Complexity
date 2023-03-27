using System.Collections.Generic;
using System.Linq;
using Complexity.ApertureMetric;

namespace Complexity.EsapiApertureMetric
{
    // Subclass of ComplexityMetric that represents the edge metric
    public class AreaMetric : ComplexityMetric
    {
        // Returns the unweighted edge metrics of a list of apertures
        protected override double[] CalculatePerAperture(IEnumerable<Aperture> apertures)
        {
            ApertureMetric.AreaMetric metric = new ApertureMetric.AreaMetric();
            return (from aperture in apertures
                    select metric.Calculate(aperture)).ToArray();
        }
    }
}