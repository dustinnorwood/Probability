using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProbabilityEngine
{
    public static class Probability
    {

        public static int GetCdfBin(this double[] x, double observation)
        {
            double[] y = x.ToCdf();
            for (int k = 0; k < x.Length; k++)
            {
                if (observation < y[k])
                    return k;
            }

            return x.Length - 1;
        }

        public static double GetCdfValue(this double[] x, double observation)
        {
            return x[x.GetCdfBin(observation)];
        }

        public static double[] Flatten(this double[] x, Predicate<double> pred)
        {
            double[] flat = new double[x.Length];
            for (int i = 0; i < flat.Length; i++)
                flat[i] = pred(x[i]) ? 1 : 0;

            return flat;
        }

        public static double[] ToPmf(this double[] x)
        {
            double sum = x.Sum();
            if (sum > 0)
                return x.Divide(sum);
            else return x;
        }

        public static double[] ToCdf(this double[] x)
        {
            double[] y = x.ToPmf();
            for (int k = 0; k < y.Length - 1; k++)
                y[k + 1] += y[k];

            return y;
        }

        public static double[] Given(this double[] x, double[] condition, Predicate<double> pred)
        {
            double[] y = new double[x.Length];
            for (int i = 0; i < x.Length; i++)
            {
                if (pred(condition[i]))
                {
                    y[i] = x[i];
                }
            }

            return y;
        }

        public static double[] Given(this double[] x, double[] condition, Predicate<double> pred, out double[] givenCondition)
        {
            double[] y = new double[x.Length];
            double[] ycond = new double[x.Length];
            for (int i = 0; i < x.Length; i++)
            {
                if (pred(condition[i]))
                {
                    y[i] = x[i];
                    ycond[i] = condition[i];
                }
            }
            givenCondition = ycond;
            return y;
        }

        public static double[] GivenPmf(this double[] x, double[] condition, Predicate<double> pred)
        {
            return x.Given(condition, pred).ToPmf();
        }

        public static double[] GivenPmf(this double[] x, double[] condition, Predicate<double> pred, out double[] givenCondition)
        {
            return x.Given(condition, pred, out givenCondition).ToPmf();
        }


        public static double AutoCorrelation(this double[] x)
        {
            return x.Correlation(x);
        }

        public static double Correlation(this double[] x, double[] y)
        {
            return x.Covariance(y) / (x.StandardDeviation() * y.StandardDeviation());
        }

        public static double Covariance(this double[] x, double[] y)
        {
            return E(x.Subtract(E(x)).Multiply(y.Subtract(E(y))));
        }

        public static double E(IEnumerable<double> list)
        {
            return list.Average();
        }

        public static double AverageNonZero(this double[] list)
        {
            double sum = 0, count = 0;
            for (int k = 0; k < list.Length; k++)
            {
                if (list[k] != 0)
                {
                    sum += list[k];
                    count++;
                }
            }

            if (count > 0)
                return sum / count;
            else
                return 0;
        }

        public static double[] Add(this double[] list1, double[] list2)
        {
            double[] ret = new double[list1.Length];
            for (int i = 0; i < ret.Length; i++)
            {
                if (i < list2.Length)
                    ret[i] = list1[i] + list2[i];
                else
                    ret[i] = list1[i];
            }

            return ret;
        }

        public static double[] Subtract(this double[] list1, double[] list2)
        {
            double[] ret = new double[list1.Length];
            for (int i = 0; i < ret.Length; i++)
            {
                if (i < list2.Length)
                    ret[i] = list1[i] - list2[i];
                else
                    ret[i] = list1[i];
            }

            return ret;
        }

        public static double[] Multiply(this double[] list1, double[] list2)
        {
            double[] ret = new double[list1.Length];
            for (int i = 0; i < ret.Length; i++)
            {
                if (i < list2.Length)
                    ret[i] = list1[i] * list2[i];
                else
                    ret[i] = 0;
            }

            return ret;
        }

        public static double[] Divide(this double[] list1, double[] list2)
        {
            double[] ret = new double[list1.Length];
            for (int i = 0; i < ret.Length; i++)
            {
                if (i < list2.Length)
                {
                    if (list2[i] != 0)
                        ret[i] = list1[i] / list2[i];
                    else ret[i] = 0;
                }
                else
                    ret[i] = list1[i];
            }

            return ret;
        }

        public static double[] Add(this double[] list1, double list2)
        {
            double[] ret = new double[list1.Length];
            for (int i = 0; i < ret.Length; i++)
            {
                ret[i] = list1[i] + list2;
            }

            return ret;
        }

        public static double[] Subtract(this double[] list1, double list2)
        {
            double[] ret = new double[list1.Length];
            for (int i = 0; i < ret.Length; i++)
            {
                ret[i] = list1[i] - list2;
            }

            return ret;
        }

        public static double[] Multiply(this double[] list1, double list2)
        {
            double[] ret = new double[list1.Length];
            for (int i = 0; i < ret.Length; i++)
            {
                ret[i] = list1[i] * list2;
            }

            return ret;
        }

        public static double[] Divide(this double[] list1, double list2)
        {
            double[] ret = new double[list1.Length];
            if (list2 != 0)
            {
                for (int i = 0; i < ret.Length; i++)
                {
                    ret[i] = list1[i] / list2;
                }
            }

            return ret;
        }

        public static double Average(this IEnumerable<double> list)
        {
            return list.Sum() / list.Count();
        }

        public static double Variance(this IEnumerable<double> list)
        {
            IEnumerator<double> e = list.GetEnumerator();
            double ave = list.Average();
            double numerator = 0.0;
            int denominator = 0;
            while (e.MoveNext())
            {
                numerator += (e.Current - ave) * (e.Current - ave);
                denominator++;
            }
            return numerator / denominator;
        }

        public static double StandardDeviation(this IEnumerable<double> list)
        {
            return Math.Sqrt(list.Variance());
        }

        public static double MaxValue<T>(this IEnumerable<Tuple<T, double>> list)
        {
            double max = 0;
            foreach (Tuple<T, double> l in list)
            {
                if (l.Item2 > max)
                    max = l.Item2;
            }
            return max;
        }
        public static double Sum<T>(this IEnumerable<Tuple<T, double>> list)
        {
            double sum = 0.0;
            foreach (Tuple<T, double> l in list)
                sum += l.Item2;

            return sum;
        }

        public static double Average<T>(this IEnumerable<Tuple<T, double>> list)
        {
            return list.Sum() / list.Count();
        }

        public static double Variance<T>(this IEnumerable<Tuple<T, double>> list)
        {
            IEnumerator<Tuple<T, double>> e = list.GetEnumerator();
            double ave = list.Average();
            double numerator = 0.0;
            int denominator = 0;
            while (e.MoveNext())
            {
                numerator += (e.Current.Item2 - ave) * (e.Current.Item2 - ave);
                denominator++;
            }
            return numerator / denominator;
        }

        public static double StandardDeviation<T>(this IEnumerable<Tuple<T, double>> list)
        {
            return Math.Sqrt(list.Variance());
        }
    }
}
