using System;

namespace WindowsFormsApp1
{
    public static class HeatCapacityCalculator
    {
        public static double CalculateAverageCp(InputData d, double temp)
        {
            double totalCp = 0;
            double sum = 0;

            void AddComponent(string gas, double percent)
            {
                if (percent <= 0 || !HeatCapacityTable.Cp.ContainsKey(gas)) 
                    return;

                double[] cpValues = HeatCapacityTable.Cp[gas];
                double interpolated = HeatCapacityInterpolator.Interpolate(
                    temp,
                    HeatCapacityTable.Temperatures,
                    cpValues
                );

                totalCp += (percent / 100.0) * interpolated;
                sum += (percent / 100.0);
            }

            // Добавляем компоненты газа
            AddComponent("CH4", d.CH4);
            AddComponent("CO", d.CO);
            AddComponent("H2", d.H2);
            AddComponent("CO2", d.CO2);
            AddComponent("SO2", d.SO2);
            AddComponent("C2H6", d.C2H6);
            AddComponent("C3H8", d.C3H8);
            AddComponent("C4H10", d.C4H10);
            AddComponent("C5H12", d.C5H12);
            AddComponent("N2", d.N2);
            AddComponent("H2S", d.H2S);
            AddComponent("C2H2", d.C2H2);
            AddComponent("C2H4", d.C2H4);
            AddComponent("H2O", d.H2O);

            return sum > 0 ? totalCp : 0;
        }

        public static double CalculateCpWetAir(double temp) => 
            HeatCapacityInterpolator.Interpolate(
                temp,
                HeatCapacityTable.Temperatures,
                HeatCapacityTable.Cp["WetAir"]
            );

        public static double CalculateCpDryAir(double temp) => 
            HeatCapacityInterpolator.Interpolate(
                temp,
                HeatCapacityTable.Temperatures,
                HeatCapacityTable.Cp["DryAir"]
            );
    }
}
