using System;
using System.Collections.Generic;
using System.Linq;

namespace Complexity.ApertureMetric
{
    static class Algebra
    {
        public static double Sum(int start, int end, Func<int, double> func)
        {
            return Enumerable.Range(start, end - start + 1).Sum(func);
        }

        public static double Mean(int start, int end, Func<int, double> func)
        {
            return Sum(start, end, func) / (end - start + 1);
        }

        public static double Distance(double x, double y)
        {
            return Math.Abs(x - y);
        }

        public static IEnumerable<int> Sequence(int n)
        {
            return Enumerable.Range(0, n);
        }
        public static double[][] ReturnDiff(IEnumerable<Aperture> apertures)
        {
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

                diff_in_positions[i] = first_position.Zip(second_position, (x, y) => x - y).ToArray();
            }
            return diff_in_positions;
        }
    }
}
