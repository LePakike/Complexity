using System.Collections.Generic;
using System.Linq;
using System;

namespace Complexity.ApertureMetric
{
    public class LeafSequenceVariability
    {
        public double Calculate(Aperture aperture, double aav_norm, List<double> pos_left, List<double> pos_right)
        {
            int N = pos_right.Count;
            double[] pos_max = new double[] { Math.Abs(pos_left.Max() - pos_left.Min()),
                Math.Abs(pos_right.Max() - pos_right.Min()) };
            double[][] pos_dif = Algebra.ReturnDiffWithinApperature(aperture);
            List<double> added_max_left = new List<double>();
            List<double> added_max_right = new List<double>();
            for (int i = 0, j = pos_dif.Length; i < j; i++)
            {
                added_max_left.Add((pos_dif[i][0] + pos_max[0]) / (pos_max[0] * N));
                added_max_right.Add((pos_dif[i][1] + pos_max[1]) / (pos_max[1] * N));
            }
            double[] summed = new double[] { added_max_left.Sum(), added_max_right.Sum() };
            double LSV = 1;
            foreach (double val in summed)
            {
                LSV *= val;
            }
            List<double> fieldsize = new List<double>();
            foreach (LeafPair lp in aperture.LeafPairs)
            {
                if (lp.IsOutsideJaw())
                {
                    continue;
                }
                fieldsize.Add(lp.FieldSize());
            }
            double num = fieldsize.Sum();
            double AAV = DivisionOrDefault(num, aav_norm);
            return LSV * AAV;
        }

        public static double DivisionOrDefault(double a, double b)
        {
            return (b != 0.0) ? (a / b) : 0.0;
        }
    }
}
