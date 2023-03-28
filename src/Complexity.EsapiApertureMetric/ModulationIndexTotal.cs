using System.Collections.Generic;
using System.Linq;
using Complexity.ApertureMetric;
using VMS.TPS.Common.Model.API;

namespace Complexity.EsapiApertureMetric
{
    // Subclass of ComplexityMetric that represents the edge metric
    public class ModulationIndexTotal : ComplexityMetric
    {
        Beam this_beam;
        double[] meterset_weights;
        double[] cumulative_metersets;
        // Returns the unweighted edge metrics of a list of apertures
        public override double CalculateForBeam(Patient patient, PlanSetup plan, Beam beam)
        {
            this_beam = beam;
            double[] cumulative_metersets = GetMetersets(this_beam);
            meterset_weights = GetWeights(this_beam);
            return WeightedSum(meterset_weights, GetMetrics(patient, plan, beam));
        }
        protected override double[] CalculatePerAperture(IEnumerable<Aperture> apertures)
        {
            get_mlc_speed(apertures);
            ApertureMetric.AreaMetric metric = new ApertureMetric.AreaMetric();
            return (from aperture in apertures
                    select metric.Calculate(aperture)).ToArray();
        }
        private void get_gantry_angle(IEnumerable<Aperture> apertures)
        {
            List<double> gantry_angles = new List<double>();
            foreach (Aperture aperture in apertures)
            {
                gantry_angles.Add(aperture.GantryAngle);
            }
        }
        private void get_mlc_speed(IEnumerable<Aperture> apertures)
        {
            /// Need to find the difference in locations between each leaf pair, then divide it by the meterset
            List<List<double>> raveled_apperture_leaf_positions = new List<List<double>>();

            foreach (Aperture aperture in apertures)
            {
                List<double> leaf_positions = new List<double>();
                foreach (LeafPair lp in aperture.LeafPairs)
                {
                    leaf_positions.Add(lp.Left);
                    leaf_positions.Add(lp.Right);
                }
                raveled_apperture_leaf_positions.Add(leaf_positions);
            }

            double[][] diff_in_positions = new double[raveled_apperture_leaf_positions.Count - 1][];
            for (int i = 0, j = raveled_apperture_leaf_positions.Count - 1; i < j; i++)
            {
                List<double> first_position = raveled_apperture_leaf_positions[i];
                List<double> second_position = raveled_apperture_leaf_positions[i + 1];

                diff_in_positions[i] = first_position.Zip(second_position, (x, y) => System.Math.Abs(x - y)).ToArray();
            }

            double[][] mlc_speed = new double[diff_in_positions.Length][];
            for (int i = 0, j = mlc_speed.Length; i < j; i++)
            {
                List<double> first_position = raveled_apperture_leaf_positions[i];
                List<double> second_position = raveled_apperture_leaf_positions[i + 1];
                mlc_speed[i] = diff_in_positions[i].Select(x => x / meterset_weights[i]).ToArray();
            }
        }
    }
}