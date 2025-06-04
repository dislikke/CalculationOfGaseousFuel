using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public class QuadraticCoefficients
    {
        public double a;
        public double b;
        public double c;
    }

    public class CoefficientsForQn
    {
        public QuadraticCoefficients ForQnGreaterThan12500;
        public QuadraticCoefficients ForQnLessOrEqual12500;
    }

    public class CoefficientsSet
    {
        public CoefficientsForQn T0 { get; set; }
        public CoefficientsForQn T0b { get; set; }
        public CoefficientsForQn Ta { get; set; }
        public CoefficientsForQn Tab { get; set; }
    }




    public static class CoefficientsLibrary
    {
        public static Dictionary<int, CoefficientsSet> GetPredefinedCoefficients()
        {
            return new Dictionary<int, CoefficientsSet>
         {
             { 0, new CoefficientsSet {
                 T0 = new CoefficientsForQn {
                     ForQnLessOrEqual12500 = new QuadraticCoefficients { a = 0.0004, b = 1.1099, c = 94.677 },
                     ForQnGreaterThan12500 = new QuadraticCoefficients { a = 0.0003, b = 1.1732, c = 48.842 }
                 },
                 Ta = new CoefficientsForQn {
                     ForQnLessOrEqual12500 = new QuadraticCoefficients { a = 0.0004, b = 1.1099, c = 94.677 },
                     ForQnGreaterThan12500 = new QuadraticCoefficients { a = 0.0003, b = 1.1732, c = 48.842 }
                 },
                 T0b = new CoefficientsForQn {
                     ForQnLessOrEqual12500 = new QuadraticCoefficients { a = 0.0001, b = 1.5059, c = -15.985 },
                     ForQnGreaterThan12500 = new QuadraticCoefficients { a = 9E-05, b = 1.5148, c = -47.944 }
                 },
                 Tab = new CoefficientsForQn {
                     ForQnLessOrEqual12500 = new QuadraticCoefficients { a = 0.0001, b = 1.5059, c = -15.985 },
                     ForQnGreaterThan12500 = new QuadraticCoefficients { a = 9E-05, b = 1.5148, c = -47.944 }
                 }
             }},
             { 20, new CoefficientsSet {
                 T0 = new CoefficientsForQn {
                     ForQnLessOrEqual12500 = new QuadraticCoefficients { a = 0.0004, b = 0.7324, c = 348.13 },
                     ForQnGreaterThan12500 = new QuadraticCoefficients { a = 0.0003, b = 0.8786, c = 267.26 }
                 },
                 Ta = new CoefficientsForQn {
                     ForQnLessOrEqual12500 = new QuadraticCoefficients { a = 0.0004, b = 0.7324, c = 348.13 },
                     ForQnGreaterThan12500 = new QuadraticCoefficients { a = 0.0003, b = 0.8786, c = 267.26 }
                 },
                 T0b = new CoefficientsForQn {
                     ForQnLessOrEqual12500 = new QuadraticCoefficients { a = 5E-05, b = 1.6514, c = -127.34 },
                     ForQnGreaterThan12500 = new QuadraticCoefficients { a = 8E-05, b = 1.506, c = -55.431 }
                 },
                 Tab = new CoefficientsForQn {
                     ForQnLessOrEqual12500 = new QuadraticCoefficients { a = 5E-05, b = 1.6514, c = -127.34 },
                     ForQnGreaterThan12500 = new QuadraticCoefficients { a = 8E-05, b = 1.506, c = -55.431 }
                 }
             }},
             { 40, new CoefficientsSet {
                 T0 = new CoefficientsForQn {
                     ForQnLessOrEqual12500 = new QuadraticCoefficients { a = 0.0003, b = 1.0053, c = 196.13 },
                     ForQnGreaterThan12500 = new QuadraticCoefficients { a = 0.0003, b = 1.042, c = 173.92 }
                 },
                 Ta = new CoefficientsForQn {
                     ForQnLessOrEqual12500 = new QuadraticCoefficients { a = 0.0003, b = 1.0053, c = 196.13 },
                     ForQnGreaterThan12500 = new QuadraticCoefficients { a = 0.0003, b = 1.042, c = 173.92 }
                 },
                 T0b = new CoefficientsForQn {
                     ForQnLessOrEqual12500 = new QuadraticCoefficients { a = 3E-05, b = 1.6632, c = -148.88 },
                     ForQnGreaterThan12500 = new QuadraticCoefficients { a = 6E-05, b = 1.5319, c = -82.884 }
                 },
                 Tab = new CoefficientsForQn {
                     ForQnLessOrEqual12500 = new QuadraticCoefficients { a = 3E-05, b = 1.6632, c = -148.88 },
                     ForQnGreaterThan12500 = new QuadraticCoefficients { a = 6E-05, b = 1.5319, c = -82.884 }
                 }
             }},
             { 60, new CoefficientsSet {
                 T0 = new CoefficientsForQn {
                     ForQnLessOrEqual12500 = new QuadraticCoefficients { a = 0.0002, b = 1.0624, c = 160.12 },
                     ForQnGreaterThan12500 = new QuadraticCoefficients { a = 0.0002, b = 1.2683, c = 40.561 }
                 },
                 Ta = new CoefficientsForQn {
                     ForQnLessOrEqual12500 = new QuadraticCoefficients { a = 0.0002, b = 1.0624, c = 160.12 },
                     ForQnGreaterThan12500 = new QuadraticCoefficients { a = 0.0002, b = 1.2683, c = 40.561 }
                 },
                 T0b = new CoefficientsForQn {
                     ForQnLessOrEqual12500 = new QuadraticCoefficients { a = 8E-05, b = 1.4516, c = -42.547 },
                     ForQnGreaterThan12500 = new QuadraticCoefficients { a = 5E-05, b = 1.5144, c = -87.848 }
                 },
                 Tab = new CoefficientsForQn {
                     ForQnLessOrEqual12500 = new QuadraticCoefficients { a = 8E-05, b = 1.4516, c = -42.547 },
                     ForQnGreaterThan12500 = new QuadraticCoefficients { a = 5E-05, b = 1.5144, c = -87.848 }
                 }
             }},
             { 80, new CoefficientsSet {
                 T0 = new CoefficientsForQn {
                     ForQnLessOrEqual12500 = new QuadraticCoefficients { a = 0.0001, b = 1.2512, c = 45.372 },
                     ForQnGreaterThan12500 = new QuadraticCoefficients { a = 0.0001, b = 1.3554, c = -17.965 }
                 },
                 Ta = new CoefficientsForQn {
                     ForQnLessOrEqual12500 = new QuadraticCoefficients { a = 0.0001, b = 1.2512, c = 45.372 },
                     ForQnGreaterThan12500 = new QuadraticCoefficients { a = 0.0001, b = 1.3554, c = -17.965 }
                 },
                 T0b = new CoefficientsForQn {
                     ForQnLessOrEqual12500 = new QuadraticCoefficients { a = 8E-05, b = 1.4041, c = -33.996 },
                     ForQnGreaterThan12500 = new QuadraticCoefficients { a = 4E-05, b = 1.5188, c = -103.03 }
                 },
                 Tab = new CoefficientsForQn {
                     ForQnLessOrEqual12500 = new QuadraticCoefficients { a = 8E-05, b = 1.4041, c = -33.996 },
                     ForQnGreaterThan12500 = new QuadraticCoefficients { a = 4E-05, b = 1.5188, c = -103.03 }
                 }
             }},
             { 100, new CoefficientsSet {
                 T0 = new CoefficientsForQn {
                     ForQnLessOrEqual12500 = new QuadraticCoefficients { a = 8E-05, b = 1.3387, c = 1.6874 },
                     ForQnGreaterThan12500 = new QuadraticCoefficients { a = 6E-05, b = 1.4125, c = -54.78 }
                 },
                 Ta = new CoefficientsForQn {
                     ForQnLessOrEqual12500 = new QuadraticCoefficients { a = 8E-05, b = 1.3387, c = 1.6874 },
                     ForQnGreaterThan12500 = new QuadraticCoefficients { a = 6E-05, b = 1.4125, c = -54.78 }
                 },
                 T0b = new CoefficientsForQn {
                     ForQnLessOrEqual12500 = new QuadraticCoefficients { a = 8E-05, b = 1.3387, c = 1.6874 },
                     ForQnGreaterThan12500 = new QuadraticCoefficients { a = 6E-05, b = 1.4125, c = -54.78 }
                 },
                 Tab = new CoefficientsForQn {
                     ForQnLessOrEqual12500 = new QuadraticCoefficients { a = 8E-05, b = 1.3387, c = 1.6874 },
                     ForQnGreaterThan12500 = new QuadraticCoefficients { a = 6E-05, b = 1.4125, c = -54.78 }
                 }
             }}
         };
        }
    }



    // Класс для расчёта теоретической температуры по квадратному уравнению
    public static class TemperatureSolver
    {
        public static double SolveQuadratic(double a, double b, double c)
        {
            double discriminant = b * b - 4 * a * c;

            if (discriminant < 0)
                throw new ArgumentException("Нет действительных решений (отрицательный дискриминант).");

            double sqrtD = Math.Sqrt(discriminant);

            double x1 = (-b + sqrtD) / (2 * a);
            double x2 = (-b - sqrtD) / (2 * a);

            if (x1 >= 0 && x2 >= 0)
                return Math.Min(x1, x2);
            else if (x1 >= 0)
                return x1;
            else if (x2 >= 0)
                return x2;
            else
                throw new ArgumentException("Оба корня отрицательны — некорректные параметры.");
        }
    }

    public static class TemperatureCalculator
    {
        public static QuadraticCoefficients SelectCoefficientsByQn(CoefficientsForQn coeffsForQn, double Qn)
        {
            return Qn > 12500 ? coeffsForQn.ForQnGreaterThan12500 : coeffsForQn.ForQnLessOrEqual12500;
        }


        public static double CalculateTemperature(
            double VL,
            double F19,
            double Qn,
            Func<CoefficientsSet, CoefficientsForQn> selector,
            Dictionary<int, CoefficientsSet> data)
        {
            var keys = data.Keys.OrderBy(k => k).ToArray();

            // Проверяем граничные условия
            if (VL < keys.First())
                throw new ArgumentException($"VL ({VL}) меньше минимального допустимого значения ({keys.First()})");
            if (VL > keys.Last())
                throw new ArgumentException($"VL ({VL}) больше максимального допустимого значения ({keys.Last()})");

            // Если VL точно совпадает с одним из ключей
            if (data.ContainsKey((int)Math.Round(VL)))
            {
                var coeffsForQn = selector(data[(int)Math.Round(VL)]);
                var coeffs = SelectCoefficientsByQn(coeffsForQn, Qn);
                return TemperatureSolver.SolveQuadratic(coeffs.a, coeffs.b, coeffs.c - F19);
            }

            // Находим ближайшие значения VL
            int leftKey = -1, rightKey = -1;
            for (int i = 0; i < keys.Length - 1; i++)
            {
                if (VL > keys[i] && VL < keys[i + 1])
                {
                    leftKey = keys[i];
                    rightKey = keys[i + 1];
                    break;
                }
            }

            if (leftKey == -1 || rightKey == -1)
                throw new ArgumentException("VL вне диапазона данных.");

            // Получаем коэффициенты для левой и правой точки
            var leftCoeffs = SelectCoefficientsByQn(selector(data[leftKey]), Qn);
            var rightCoeffs = SelectCoefficientsByQn(selector(data[rightKey]), Qn);

            // Решаем квадратные уравнения для обеих точек
            double tLeft = TemperatureSolver.SolveQuadratic(
                leftCoeffs.a,
                leftCoeffs.b,
                leftCoeffs.c - F19
            );

            double tRight = TemperatureSolver.SolveQuadratic(
                rightCoeffs.a,
                rightCoeffs.b,
                rightCoeffs.c - F19
            );

            // Используем функцию Interpolate для интерполяции между двумя точками
            return HeatCapacityInterpolator.Interpolate(VL, new double[] { leftKey, rightKey }, new double[] { tLeft, tRight });
        }
    }





    // Класс для интерполяции средней теплоёмкости по таблице
    public static class HeatCapacityInterpolator
    {
        public static double Interpolate(double x, double[] xVals, double[] yVals)
        {
            if (xVals.Length != yVals.Length)
                throw new ArgumentException("Размеры массивов не совпадают");

            int n = xVals.Length;

            if (x <= xVals[0])
                return yVals[0] + (x - xVals[0]) * (yVals[1] - yVals[0]) / (xVals[1] - xVals[0]);

            if (x >= xVals[n - 1])
                return yVals[n - 1] + (x - xVals[n - 1]) * (yVals[n - 1] - yVals[n - 2]) / (xVals[n - 1] - xVals[n - 2]);

            for (int i = 0; i < n - 1; i++)
            {
                if (x >= xVals[i] && x <= xVals[i + 1])
                {
                    return yVals[i] + (x - xVals[i]) * (yVals[i + 1] - yVals[i]) / (xVals[i + 1] - xVals[i]);
                }
            }

            throw new Exception("Не удалось интерполировать значение");
        }
    }

    // Таблица теплоемкостей компонентов
    public static class HeatCapacityTable
    {
        public static readonly double[] Temperatures = {
        0, 100, 200, 300, 400, 500, 600, 700, 800, 900, 1000, 1100, 1200
    };

        public static readonly Dictionary<string, double[]> Cp = new Dictionary<string, double[]>
    {
        { "CO",     new double[] { 1.293, 1.367, 1.443, 1.519, 1.595, 1.671, 1.747, 1.823, 1.899, 1.975, 2.051, 2.127, 2.203 } },
        { "SO2",    new double[] { 1.854, 1.963, 2.072, 2.181, 2.29, 2.399, 2.508, 2.617, 2.726, 2.835, 2.944, 3.053, 3.162 } },
        { "H2S",    new double[] { 1.62, 1.712, 1.804, 1.896, 1.988, 2.08, 2.172, 2.264, 2.356, 2.448, 2.54, 2.632, 2.724 } },
        { "H2",     new double[] { 1.293, 1.367, 1.443, 1.519, 1.595, 1.671, 1.747, 1.823, 1.899, 1.975, 2.051, 2.127, 2.203 } },
        { "O2",     new double[] { 1.429, 1.517, 1.605, 1.693, 1.781, 1.869, 1.957, 2.045, 2.133, 2.221, 2.309, 2.397, 2.485 } },
        { "C2H4",   new double[] { 1.261, 1.338, 1.415, 1.492, 1.569, 1.646, 1.723, 1.8, 1.877, 1.954, 2.031, 2.108, 2.185 } },
        { "C2H2",   new double[] { 1.171, 1.245, 1.319, 1.393, 1.467, 1.541, 1.615, 1.689, 1.763, 1.837, 1.911, 1.985, 2.059 } },
        { "CH4",    new double[] { 1.55, 1.6421, 1.7588, 1.8862, 2.0155, 2.1403, 2.2609, 2.30768, 2.4941, 2.6025, 2.6992, 2.7863, 2.8629 } },
        { "C2H6",   new double[] { 2.2098, 2.4949, 2.7746, 3.0442, 3.3084, 3.5525, 3.7778, 3.9863, 4.1809, 4.362, 4.5293, 4.6838, 4.8255 } },
        { "C3H8",   new double[] { 3.0484, 3.5098, 3.9653, 4.3691, 4.7596, 5.0937, 5.4322, 5.7236, 5.9887, 6.2315, 6.4614, 6.6778, 6.8817 } },
        { "C4H10",  new double[] { 4.1284, 4.7054, 5.2564, 5.7722, 6.2671, 6.6891, 7.1149, 7.4851, 7.8083, 8.1144, 8.4041, 8.6788, 8.9384 } },
        { "C5H12",  new double[] { 5.1274, 5.8354, 6.5154, 7.1355, 7.7409, 8.2563, 8.7831, 9.2315, 9.6255, 9.9918, 10.3448, 10.6794, 10.9967 } },
        { "CO2",    new double[] { 1.5998, 1.7003, 1.7874, 1.8628, 1.9298, 1.9888, 2.0412, 2.0885, 2.1312, 2.1693, 2.2036, 2.235, 2.2639 } },
        { "N2",     new double[] { 1.2946, 1.2958, 1.2996, 1.3068, 1.3164, 1.3277, 1.3403, 1.3536, 1.367, 1.3796, 1.3918, 1.4035, 1.4143 } },
        { "H2O",    new double[] { 1.4943, 1.5052, 1.5224, 1.5424, 1.5655, 1.5898, 1.6149, 1.6412, 1.668, 1.6957, 1.7229, 1.7502, 1.7769 } },
        { "DryAir", new double[] { 1.2971, 1.3004, 1.3071, 1.3172, 1.3289, 1.3427, 1.3565, 1.3708, 1.3842, 1.3976, 1.4097, 1.4214, 1.4327 } },
        { "WetAir", new double[] { 1.3188, 1.3243, 1.3318, 1.3423, 1.3544, 1.3683, 1.3829, 1.3976, 1.4114, 1.4248, 1.4373, 1.4499, 1.4612 } }
    };
    }

}


