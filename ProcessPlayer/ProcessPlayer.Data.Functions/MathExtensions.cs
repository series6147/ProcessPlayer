using System;

namespace ProcessPlayer.Data.Functions
{
    public static class MathExtensions
    {
        #region private static methods

        private static int GCD(int a, int b)
        {
            while (true)
            {
                a = a % b;

                if (a == 0)
                    return b;

                b = b % a;

                if (b == 0)
                    return a;
            }
        }

        #endregion

        #region public static methods

        public static double abs(double number)
        {
            return System.Math.Abs(number);
        }

        public static double acos(double number)
        {
            return System.Math.Acos(number);
        }

        public static double acosh(double number)
        {
            if (number < 1)
                throw new ArithmeticException("range exception");

            return System.Math.Log(number + System.Math.Sqrt(number * number - 1));
        }

        public static double asin(double number)
        {
            return System.Math.Asin(number);
        }

        public static double asinh(double number)
        {
            double x;
            int sign;

            if (number == 0)
                return number;

            if (number < 0)
            {
                sign = -1;
                x = -number;
            }
            else
            {
                sign = 1;
                x = number;
            }

            return sign * System.Math.Log(x + System.Math.Sqrt(x * x + 1));
        }

        public static double atan(double number)
        {
            return System.Math.Atan(number);
        }

        public static double atan2(double x, double y)
        {
            return System.Math.Atan2(y, x);
        }

        public static double atanh(double number)
        {
            if (number > 1.0 || number < -1.0)
                throw new ArithmeticException("range exception");

            return 0.5d * System.Math.Log((1.0 + number) / (1.0 - number));
        }

        public static double ceiling(double number, double precision)
        {
            if (precision == 0)
                return 0;
            return System.Math.Ceiling(number / precision) * precision;
        }

        public static double combin(int number, int numberChosen)
        {
            return fact(number) / (fact(numberChosen) * fact(number - numberChosen));
        }

        public static double cos(double number)
        {
            return System.Math.Cos(number);
        }

        public static double cosh(double number)
        {
            return System.Math.Cosh(number);
        }

        public static double degrees(double number)
        {
            return (180d * number) / System.Math.PI;
        }

        public static int even(int number)
        {
            if (number % 2 == 0)
                return number;

            if (number < 0)
                return number - 1;

            return number + 1;
        }

        public static double exp(double number)
        {
            return System.Math.Exp(number);
        }

        public static double fact(double number)
        {
            double fact = 1;

            for (int i = 2; i <= number; i++)
                fact *= i;

            return fact;
        }

        public static double factDouble(int number)
        {
            double fact = 1;

            for (int i = number % 2 == 0 ? 2 : 3; i <= number; i += 2)
                fact *= i;

            return fact;
        }

        public static double floor(double number, double precision)
        {
            if (precision == 0)
                return 0;

            return System.Math.Floor(number / precision) * precision;
        }

        public static int gcd(params int[] numbers)
        {
            int gcd = numbers[0];

            for (int i = 1; i < numbers.Length; i++)
                gcd = GCD(gcd, numbers[i]);

            return gcd;
        }

        public static bool isNumeric(string value)
        {
            double d;

            return double.TryParse(value, out d);
        }

        public static int lcm(params int[] numbers)
        {
            for (int i = 1; ; i++)
            {
                bool lcm = true;

                for (int j = 0; j < numbers.Length; j++)
                    if (!(lcm &= (i % numbers[j] == 0)))
                        break;

                if (lcm)
                    return i;
            }
        }

        public static double ln(double number)
        {
            return System.Math.Log(number);
        }

        public static double log(double number, double newbase)
        {
            return System.Math.Log(number, newbase);
        }

        public static double log10(double number)
        {
            return System.Math.Log10(number);
        }

        public static double toDouble(object value)
        {
            try
            {
                return value == null || value == DBNull.Value
                    ? default(double)
                    : value is double
                        ? (double)value
                        : Convert.ToDouble(value);
            }
            catch
            {
                return default(double);
            }
        }

        public static int toInt(double number)
        {
            return Convert.ToInt32(System.Math.Floor(number));
        }

        public static int toInt(object value)
        {
            try
            {
                return value == null || value == DBNull.Value
                    ? default(int)
                    : value is int
                        ? (int)value
                        : Convert.ToInt32(value);
            }
            catch
            {
                return default(int);
            }
        }

        #endregion
    }
}
