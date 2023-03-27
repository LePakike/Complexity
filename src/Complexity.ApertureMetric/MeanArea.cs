namespace Complexity.ApertureMetric
{
    public class AreaMetric
    {
        public double Calculate(Aperture aperture)
        {
            return aperture.Area();
        }

        public static double DivisionOrDefault(double a, double b)
        {
            return (b != 0.0) ? (a / b) : 0.0;
        }
    }
}