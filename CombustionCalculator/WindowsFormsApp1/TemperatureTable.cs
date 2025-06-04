using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    /// <summary>
    /// Класс для работы с таблицами температур и энтальпий
    /// Содержит методы для интерполяции и расчета температур на основе табличных данных
    /// </summary>
    public static class TemperatureTable
    {
        /// <summary>
        /// Записывает отладочную информацию в лог-файл и окно отладки
        /// </summary>
        private static void Log(string message)
        {
            Debug.WriteLine(message);
            File.AppendAllText("temperature_debug.log", message + Environment.NewLine);
            System.Diagnostics.Debug.WriteLine(message);
        }

        // Базовые значения энтальпий, используемые во всех таблицах
        private static readonly double[] EnthalpyValues = new double[] 
        { 160, 462, 735, 916, 1092, 1415, 1777, 2150, 2541, 2940, 3457, 3990, 4620, 5040 };

        // Таблица для расчета температур T0b и Tab при низкой теплоте сгорания (Qn <= 8400)
        // Ключ словаря - значение влагосодержания VL (%)
        // Значение - кортеж из массивов температур и соответствующих им энтальпий
        private static readonly Dictionary<int, (double[] Temperatures, double[] Enthalpies)> EnthalpyTable_LowQnB = new Dictionary<int, (double[] Temperatures, double[] Enthalpies)>
        {
            { 0, (new double[]   { 100, 300, 500, 600, 700, 900, 1100, 1300, 1500, 1700, 1900, 2100, 2300, 2500 },
                  new double[] { 160, 462, 735, 916, 1092, 1415, 1777, 2150, 2541, 2885, 3276, 3679, 4091, 4460 }) },

            { 20, (new double[] { 600, 700, 900, 1100, 1300, 1500, 1700, 1900, 2100, 2300, 2500 },
                   new double[] { 903, 1050, 1386,  1730, 2100, 2520, 2835, 3205, 3570,3940, 4347 }) },


            { 40, (new double[] { 600, 700, 900, 1100, 1300, 1500, 1700, 1900, 2100, 2300, 2500 },
                   new double[] { 890, 1042, 1365, 1680, 2012, 2415, 2894, 3137, 3473, 3864, 4208 })},


            { 60, (new double[] { 600, 700, 900, 1100, 1300, 1500, 1700, 1900, 2100, 2300, 2500 },
                   new double[] { 861, 1029, 1310, 1638, 1987, 2352, 2617, 3045, 3436, 3885, 4376 }) },

            { 80, (new double[] { 600, 700, 900, 1100, 1300, 1500, 1700, 1900, 2100, 2300, 2500 },
                   new double[] { 840, 995, 1285, 1613, 1924, 2226, 2617, 2940, 3297, 3679, 4095}) },

            { 100, (new double[] { 100, 300, 500, 600, 700, 900, 1100, 1300, 1500, 1700, 1900, 2100, 2300, 2500 },
                    new double[] { 155, 420, 672, 832, 953, 1260, 1575, 1886, 2205, 2520, 2839, 3167, 3515, 3835 }) }
        };

        // Таблица для расчета температур T0b и Tab при высокой теплоте сгорания (Qn > 12500)
        private static readonly Dictionary<int, (double[] Temperatures, double[] Enthalpies)> EnthalpyTable_HighQnB = new Dictionary<int, (double[] Temperatures, double[] Enthalpies)>
        {
            { 0, (new double[]   { 100, 300, 500, 600, 700, 900, 1100, 1300, 1500, 1700, 1900, 2100, 2300, 2500 },
                  new double[] { 109, 420, 735, 890, 1050, 1386, 1730, 2058, 2436, 2814, 3171, 3553, 3927, 4305 }) },

            { 20, (new double[] { 600, 700, 900, 1100, 1300, 1500, 1700, 1900, 2100, 2300, 2500 },
                   new double[] { 882, 1033, 1365, 1705, 2016, 2394, 2730, 3095, 3465, 3835, 4200}) },


            { 40, (new double[] { 600, 700, 900, 1100, 1300, 1500, 1700, 1900, 2100, 2300, 2500 },
                   new double[] { 861, 1016, 1340, 1680, 1995, 2331, 2688, 3045, 3402, 3738, 4099 })},


            { 60, (new double[] { 600, 700, 900, 1100, 1300, 1500, 1700, 1900, 2100, 2300, 2500 },
                   new double[] { 844, 1000, 1310, 1630, 1974, 2289, 2625, 2982, 3318, 3675, 3998 }) },

            { 80, (new double[] { 600, 700, 900, 1100, 1300, 1500, 1700, 1900, 2100, 2300, 2500 },
                   new double[] { 840, 974, 1285, 1600, 1911, 2260, 2604, 2932, 3255, 3570, 3919 }) },

            { 100, (new double[] { 100, 300, 500, 600, 700, 900, 1100, 1300, 1500, 1700, 1900, 2100, 2300, 2500 },
                    new double[] { 105, 378, 655, 806, 945, 1260, 1575, 1890, 2205, 2533, 2848, 3184, 3520, 3839 }) }
        };

        // Таблица для расчета температур T0 и Ta при низкой теплоте сгорания (Qn <= 8400)
        private static readonly Dictionary<int, (double[] Temperatures, double[] Enthalpies)> EnthalpyTable_LowQn = new Dictionary<int, (double[] Temperatures, double[] Enthalpies)>
        {
            { 0, (new double[]   { 100, 300, 500, 600, 700, 900, 1100, 1300, 1500, 1700, 1900, 2100, 2300 },
                  new double[] { 160, 462, 735, 916, 1092, 1415, 1777, 2150, 2541, 2940, 3457, 3990, 4620 }) },

            { 20, (new double[] { 600, 700, 900, 1100, 1300, 1500, 1700, 1900, 2100, 2300, 2500 },
                   new double[] { 903, 1050, 1386, 1730, 2100, 2520, 2835, 3255, 3730, 4322, 5040 }) },


            { 40, (new double[] { 600, 700, 900, 1100, 1300, 1500, 1700, 1900, 2100, 2300, 2500 },
                   new double[] { 890, 1042, 1365, 1680, 2012, 2415, 2894, 3142, 3570, 4091, 4696 })},


            { 60, (new double[] { 600, 700, 900, 1100, 1300, 1500, 1700, 1900, 2100, 2300, 2500 },
                   new double[] { 861, 1029, 1310, 1638, 1987, 2352, 2617, 3045, 3436, 3885, 4376 }) },

            { 80, (new double[] { 600, 700, 900, 1100, 1300, 1500, 1700, 1900, 2100, 2300, 2500 },
                   new double[] { 840, 995, 1285, 1613, 1924, 2226, 2617, 2940, 3297, 3679, 4095 }) },

            { 100, (new double[] { 100, 300, 500, 600, 700, 900, 1100, 1300, 1500, 1700, 1900, 2100, 2300, 2500 },
                    new double[] { 155, 420, 672, 832, 953, 1260, 1575, 1886, 2205, 2520, 2839, 3167, 3515, 3835 }) }
        };

        // Таблица для расчета температур T0 и Ta при высокой теплоте сгорания (Qn > 12500)
        private static readonly Dictionary<int, (double[] Temperatures, double[] Enthalpies)> EnthalpyTable_HighQn = new Dictionary<int, (double[] Temperatures, double[] Enthalpies)>
        {
            { 0, (new double[]   { 100, 300, 500, 600, 700, 900, 1100, 1300, 1500, 1700, 1900, 2100, 2300 },
                  new double[] { 109, 420, 735, 890, 1050, 1386, 1730, 2058, 2436, 2856, 3297, 3780, 4410 }) },

            { 20, (new double[] { 600, 700, 900, 1100, 1300, 1500, 1700, 1900, 2100, 2300, 2500 },
                   new double[] { 882, 1033, 1365, 1705, 2016, 2394, 2755, 3150, 3570, 4095, 4704 }) },


            { 40, (new double[] { 600, 700, 900, 1100, 1300, 1500, 1700, 1900, 2100, 2300, 2500 },
                   new double[] { 861, 1016, 1340, 1680, 1995, 2331, 2688, 3045, 3465, 3927, 4452 })},


            { 60, (new double[] { 600, 700, 900, 1100, 1300, 1500, 1700, 1900, 2100, 2300, 2500 },
                   new double[] { 844, 1000, 1310, 1630, 1974, 2289, 2625, 3003, 3360, 3738, 4200 }) },

            { 80, (new double[] { 600, 700, 900, 1100, 1300, 1500, 1700, 1900, 2100, 2300, 2500 },
                   new double[] { 840, 974, 1285, 1600, 1911, 2260, 2604, 2940, 3263, 3654, 4032 }) },

            { 100, (new double[] { 100, 300, 500, 600, 700, 900, 1100, 1300, 1500, 1700, 1900, 2100, 2300, 2500 },
                    new double[] { 105, 378, 655, 806, 945, 1260, 1575, 1890, 2205, 2533, 2848, 3184, 3520, 3839 }) }
        };


        // Таблица для T0, Ta при Qn > 12500
        private static readonly Dictionary<int, double[]> EnthalpiesHigh_T = new Dictionary<int, double[]>
        {
            { 0, new double[] { 90, 280, 500, 580, 680, 880, 1080, 1250, 1450, 1650, 1850, 2050, 2250, 2500 } },
            { 20, new double[] { 90, 280, 500, 570, 670, 870, 1070, 1240, 1440, 1600, 1800, 2000, 2200, 2400 } },
            { 40, new double[] { 90, 280, 500, 560, 660, 860, 1060, 1230, 1400, 1580, 1750, 1950, 2150, 2350 } },
            { 60, new double[] { 85, 260, 450, 550, 650, 850, 1050, 1220, 1380, 1550, 1750, 1900, 2100, 2300 } },
            { 80, new double[] { 85, 260, 450, 550, 630, 840, 1030, 1200, 1380, 1550, 1700, 1900, 2100, 2300 } },
            { 100, new double[] { 85, 260, 450, 530, 620, 830, 1000, 1180, 1350, 1500, 1700, 1900, 2100, 2250 } }
        };

        // Таблица для T0б, Taб при Qn <= 8400
        private static readonly Dictionary<int, double[]> EnthalpiesLow_Tb = new Dictionary<int, double[]>
        {
            { 0, new double[] { 100, 300, 500, 600, 700, 900, 1100, 1300, 1500, 1700, 1900, 2100, 2300, 2450 } },
            { 20, new double[] { 100, 300, 500, 590, 680, 880, 1080, 1280, 1480, 1650, 1850, 2050, 2200, 2400 } },
            { 40, new double[] { 100, 300, 500, 580, 670, 870, 1050, 1250, 1450, 1650, 1800, 2000, 2150, 2350 } },
            { 60, new double[] { 100, 300, 500, 570, 660, 850, 1030, 1230, 1430, 1580, 1780, 1950, 2100, 2300 } },
            { 80, new double[] { 95, 280, 480, 560, 650, 840, 1020, 1200, 1380, 1580, 1750, 1900, 2050, 2250 } },
            { 100, new double[] { 95, 280, 480, 550, 630, 830, 1000, 1200, 1400, 1550, 1750, 1900, 2050, 2200 } }
        };

        // Таблица для T0б, Taб при Qn > 12500
        private static readonly Dictionary<int, double[]> EnthalpiesHigh_Tb = new Dictionary<int, double[]>
        {
            { 0, new double[] { 90, 280, 500, 580, 680, 880, 1080, 1250, 1450, 1650, 1850, 2050, 2200, 2400 } },
            { 20, new double[] { 90, 280, 500, 570, 670, 870, 1070, 1240, 1440, 1600, 1800, 2000, 2150, 2350 } },
            { 40, new double[] { 90, 280, 500, 560, 660, 860, 1060, 1230, 1400, 1580, 1750, 1950, 2100, 2300 } },
            { 60, new double[] { 85, 260, 450, 550, 650, 850, 1050, 1220, 1380, 1550, 1700, 1900, 2050, 2250 } },
            { 80, new double[] { 85, 260, 450, 550, 630, 840, 1030, 1200, 1380, 1550, 1700, 1850, 2000, 2200 } },
            { 100, new double[] { 85, 260, 450, 530, 620, 830, 1000, 1180, 1350, 1500, 1700, 1850, 2000, 2150 } }
        };

        

        /// <summary>
        /// Выполняет интерполяцию для нахождения температуры по заданной энтальпии
        /// </summary>
        /// <param name="targetEnthalpy">Целевое значение энтальпии</param>
        /// <param name="enthalpyValues">Массив известных значений энтальпий</param>
        /// <param name="temperatureValues">Массив соответствующих температур</param>
        /// <returns>Интерполированное значение температуры</returns>
        private static double InterpolateForEnthalpy(double targetEnthalpy, double[] enthalpyValues, double[] temperatureValues)
        {
            try
            {
                Log($"Ищем температуру для энтальпии {targetEnthalpy:F2}");
                Log($"Доступные значения энтальпий: {string.Join(", ", enthalpyValues)}");
                Log($"Соответствующие температуры: {string.Join(", ", temperatureValues)}");

                // Проверяем, не выходит ли энтальпия за границы таблицы
                if (targetEnthalpy < enthalpyValues[0] || targetEnthalpy > enthalpyValues[enthalpyValues.Length - 1])
                {
                    var message = $"Энтальпия {targetEnthalpy:F2} кДж/м³ вне диапазона энтальпий ({enthalpyValues[0]:F2} - {enthalpyValues[enthalpyValues.Length - 1]:F2} кДж/м³)";
                    Log(message);
                    throw new ArgumentException(message);
                }

                // Ищем интервал, в который попадает заданная энтальпия
                int leftIndex = -1;
                for (int i = 0; i < enthalpyValues.Length - 1; i++)
                {
                    if (targetEnthalpy >= enthalpyValues[i] && targetEnthalpy <= enthalpyValues[i + 1])
                    {
                        leftIndex = i;
                        Log($"Найден интервал: [{enthalpyValues[i]:F2}, {enthalpyValues[i + 1]:F2}] кДж/м³");
                        break;
                    }
                }

                // Если интервал не найден, проверяем на точное совпадение
                if (leftIndex == -1)
                {
                    for (int i = 0; i < enthalpyValues.Length; i++)
                    {
                        if (Math.Abs(targetEnthalpy - enthalpyValues[i]) < 0.0001)
                        {
                            Log($"Найдено точное совпадение: {enthalpyValues[i]:F2} кДж/м³ -> {temperatureValues[i]:F2}°C");
                            return temperatureValues[i];
                        }
                    }
                    
                    throw new ArgumentException($"Не удалось найти подходящий интервал для энтальпии {targetEnthalpy:F2} кДж/м³");
                }

                // Выполняем линейную интерполяцию
                double h1 = enthalpyValues[leftIndex];
                double h2 = enthalpyValues[leftIndex + 1];
                double t1 = temperatureValues[leftIndex];
                double t2 = temperatureValues[leftIndex + 1];

                var result = t1 + (targetEnthalpy - h1) * (t2 - t1) / (h2 - h1);
                Log($"Линейная интерполяция:");
                Log($"h1={h1:F2} кДж/м³ -> t1={t1:F2}°C");
                Log($"h2={h2:F2} кДж/м³ -> t2={t2:F2}°C");
                Log($"Результат интерполяции: {result:F2}°C");
                return result;
            }
            catch (Exception ex)
            {
                Log($"Ошибка в InterpolateForEnthalpy: {ex.Message}");
                throw;
            }
        }


        private static readonly double[] TemperatureValues = new double[] { 100, 300, 500, 600, 700, 900, 1100, 1300, 1500, 1700, 1900, 2100, 2300, 2500 };

        /// <summary>
        /// Выполняет двойную интерполяцию для нахождения температуры по энтальпии и влагосодержанию
        /// </summary>
        public static double InterpolateTemperatureByEnthalpy(
            double targetEnthalpy,
            double VL,
            Dictionary<int, (double[] Temperatures, double[] Enthalpies)> enthalpyTable)
        {
            // Получаем список доступных значений влагосодержания
            var vlList = enthalpyTable.Keys.OrderBy(x => x).ToList();

            // Находим ближайшие табличные значения VL
            int vlLow = vlList.Where(v => v <= VL).DefaultIfEmpty(vlList.First()).Max();
            int vlHigh = vlList.Where(v => v >= VL).DefaultIfEmpty(vlList.Last()).Min();

            // Получаем данные для обоих значений VL
            var (temperaturesLow, enthalpiesLow) = enthalpyTable[vlLow];
            var (temperaturesHigh, enthalpiesHigh) = enthalpyTable[vlHigh];

            // Находим температуры для обоих значений VL
            double tLow = InterpolateFromTable(targetEnthalpy, enthalpiesLow, temperaturesLow);
            double tHigh = InterpolateFromTable(targetEnthalpy, enthalpiesHigh, temperaturesHigh);

            // Если значения VL совпадают, возвращаем результат без интерполяции
            if (vlLow == vlHigh)
                return tLow;

            // Выполняем линейную интерполяцию по VL
            return tLow + (tHigh - tLow) * (VL - vlLow) / (vlHigh - vlLow);
        }


        /// <summary>
        /// Выполняет линейную интерполяцию значения y по заданному x и массивам x/y
        /// </summary>
        /// <param name="x">Искомое значение x</param>
        /// <param name="xArray">Массив известных значений x</param>
        /// <param name="yArray">Массив соответствующих значений y</param>
        public static double InterpolateFromTable(double x, double[] xArray, double[] yArray)
        {
            // Находим индекс ближайшего меньшего значения x
            int i = 0;
            while (i < xArray.Length && xArray[i] < x) i++;
            if (i == 0) i = 1;
            if (i >= xArray.Length) i = xArray.Length - 1;

            // Выполняем линейную интерполяцию
            double x0 = xArray[i - 1];
            double x1 = xArray[i];
            double y0 = yArray[i - 1];
            double y1 = yArray[i];

            return y0 + (y1 - y0) * (x - x0) / (x1 - x0);
        }

        /// <summary>
        /// Выбирает подходящую таблицу энтальпий в зависимости от теплоты сгорания и типа расчета
        /// </summary>
        /// <param name="Qn">Теплота сгорания</param>
        /// <param name="isTb">True для расчета T0b/Tab, False для T0/Ta</param>
        private static Dictionary<int, (double[] Temperatures, double[] Enthalpies)> SelectEnthalpyTable(double Qn, bool isTb)
        {
            if (isTb)
            {
                return Qn <= 8400 ? EnthalpyTable_LowQnB : EnthalpyTable_HighQnB;
            }
            return Qn <= 8400 ? EnthalpyTable_LowQn : EnthalpyTable_HighQn;
        }

        /// <summary>
        /// Рассчитывает температуру на основе энтальпии, влагосодержания и теплоты сгорания
        /// </summary>
        /// <param name="enthalpy">Энтальпия, кДж/м³</param>
        /// <param name="VL">Влагосодержание, %</param>
        /// <param name="Qn">Теплота сгорания, кДж/м³</param>
        /// <param name="isTb">True для расчета T0b/Tab, False для T0/Ta</param>
        public static double CalculateTemperature(double enthalpy, double VL, double Qn, bool isTb)
        {
            var table = SelectEnthalpyTable(Qn, isTb);
            return InterpolateTemperatureByEnthalpy(enthalpy, VL, table);
        }

        /// <summary>
        /// Получает основную таблицу температур и энтальпий в зависимости от теплоты сгорания
        /// </summary>
        public static Dictionary<int, (double[] Temperatures, double[] Enthalpies)> GetMainTable(double Qn)
        {
            return Qn <= 8400 ? EnthalpyTable_LowQn : EnthalpyTable_HighQn;
        }

        /// <summary>
        /// Получает таблицу температур и энтальпий для расчета T0b/Tab в зависимости от теплоты сгорания
        /// </summary>
        public static Dictionary<int, (double[] Temperatures, double[] Enthalpies)> GetBTable(double Qn)
        {
            return Qn <= 8400 ? EnthalpyTable_LowQnB : EnthalpyTable_HighQnB;
        }



    }
} 