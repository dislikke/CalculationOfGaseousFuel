using System;
using System.Diagnostics;

namespace WindowsFormsApp1
{
    internal static class CombustionCalculator
    {
        #region Расчет объемов воздуха
        public static double CalculateVO2(InputData d)
        {
            return 0.01 * (
                0.5 * d.CO +
                0.5 * d.H2 +
                1.5 * d.H2S +
                2.0 * d.CH4 +
                3.5 * d.C2H6 +
                5.0 * d.C3H8 +
                6.5 * d.C4H10 +
                8.0 * d.C5H12 +
                3.0 * d.C2H2 +
                2.5 * d.C2H4 -
                d.O2
            );
        }

        public static double CalculateL0(double VO2) => (1 + 3.762) * VO2;

        public static double CalculateL0Wet(double L0, double moisture) => (1 + 0.001244 * moisture) * L0;

        public static double CalculateLa(double L0, double alpha) => alpha * L0;

        public static double CalculateLaWet(double L0Wet, double alpha) => alpha * L0Wet;
        #endregion

        #region Расчет объемов продуктов сгорания
        public static double CalculateV0CO2(InputData d)
        {
            return 0.01 * (
                d.CO2 + d.CO + d.CH4 +
                2 * d.C2H6 +
                3 * d.C3H8 +
                4 * d.C4H10 +
                5 * d.C5H12 +
                2 * d.C2H2 +
                2 * d.C2H4
            );
        }

        public static double CalculateV0SO2(InputData d) => 0.01 * (d.SO2 + d.H2S);

        public static double CalculateV0N2(InputData d, double L0) => 0.01 * d.N2 + 0.79 * L0;

        public static double CalculateV0H2O(InputData d, double moisture, double L0)
        {
            return 0.01 * (
                d.H2O + d.H2 + d.H2S +
                2 * d.CH4 + 3 * d.C2H6 + 4 * d.C3H8 + 5 * d.C4H10 + 6 * d.C5H12 +
                2 * d.C2H2 + d.C2H4
            ) + 0.001244 * moisture * L0;
        }

        public static double CalculateV0Total(double vCO2, double vSO2, double vN2, double vH2O) => 
            vCO2 + vSO2 + vN2 + vH2O;

        public static double CalculateVaN2(double V0N2, double L0, double alpha) => 
            V0N2 + (alpha - 1) * 0.79 * L0;

        public static double CalculateVaO2(double VO2, double alpha) => 
            (alpha - 1) * VO2;

        public static double CalculateVaH2O(double V0H2O, double L0, double alpha, double moisture) => 
            V0H2O + (alpha - 1) * 0.001244 * moisture * L0;

        public static double CalculateVaTotal(double CO2, double SO2, double N2, double O2, double H2O) => 
            CO2 + SO2 + N2 + O2 + H2O;
        #endregion

        #region Расчет теплоты сгорания и энтальпии
        public static double CalculateQn(InputData d)
        {
            return 127.7 * d.CO +
                   108.0 * d.H2 +
                   358.0 * d.CH4 +
                   590.0 * d.C2H2 +
                   555.0 * d.C2H4 +
                   636.0 * d.C2H6 +
                   913.0 * d.C3H8 +
                   1185.0 * d.C4H10 +
                   1465.0 * d.C5H12 +
                   234.0 * d.H2S;
        }

        public static double CalculateITTotal(double Qn, double V0)
        {
            var result = Qn / V0;
            Debug.WriteLine($"CalculateITTotal: Qn={Qn}, V0={V0}, result={result}");
            return result;
        }

        public static double CalculateIBTotal(double Qn, double V0, double i3I4)
        {
            var result = (Qn - i3I4) / V0;
            Debug.WriteLine($"CalculateIBTotal: Qn={Qn}, V0={V0}, i3I4={i3I4}, result={result}");
            return result;
        }

        public static double CalculateVL(double LaWet, double L0Wet, double VaTotal)
        {
            if (LaWet == 0)
                return 0;

            return (LaWet - L0Wet) * 100.0 / VaTotal;
        }

        public static double CalculateITGeneral(double alpha, double La, double Qn, double VaTotal, double LaWet, double cpDryAir, double tempAir, double cpFuel, double tempFuel)
        {
            Debug.WriteLine($"\n=== Calculating ITGeneral ===");
            Debug.WriteLine($"Input parameters:");
            Debug.WriteLine($"alpha: {alpha:F2}");
            Debug.WriteLine($"La: {La:F2}");
            Debug.WriteLine($"LaWet: {LaWet:F2}");
            Debug.WriteLine($"Qn: {Qn:F2}");
            Debug.WriteLine($"VaTotal: {VaTotal:F2}");
            Debug.WriteLine($"cpDryAir: {cpDryAir:F2}");
            Debug.WriteLine($"tempAir: {tempAir:F2}");
            Debug.WriteLine($"cpFuel: {cpFuel:F2}");
            Debug.WriteLine($"tempFuel: {tempFuel:F2}");

            double L = alpha == 1 ? La : LaWet;
            
            double term1 = Qn / VaTotal;
            double term2 = (L * cpDryAir * tempAir) / VaTotal;
            double term3 = (cpFuel * tempFuel) / VaTotal;
            
            var result = term1 + term2 + term3;
            
            Debug.WriteLine($"\nCalculation terms:");
            Debug.WriteLine($"term1 (Qn/VaTotal): {term1:F2}");
            Debug.WriteLine($"term2 (L*cpDryAir*tempAir/VaTotal): {term2:F2}");
            Debug.WriteLine($"term3 (cpFuel*tempFuel/VaTotal): {term3:F2}");
            Debug.WriteLine($"Final result: {result:F2}");
            
            return result;
        }

        public static double CalculateIBGeneral(double alpha, double La, double Qn, double VaTotal, double LaWet, 
            double cpDryAir, double tempAir, double cpFuel, double tempFuel, double i3i4)
        {
            Debug.WriteLine($"\n=== Calculating IBGeneral ===");
            Debug.WriteLine($"Input parameters:");
            Debug.WriteLine($"alpha: {alpha:F2}");
            Debug.WriteLine($"La: {La:F2}");
            Debug.WriteLine($"LaWet: {LaWet:F2}");
            Debug.WriteLine($"Qn: {Qn:F2}");
            Debug.WriteLine($"VaTotal: {VaTotal:F2}");
            Debug.WriteLine($"cpDryAir: {cpDryAir:F2}");
            Debug.WriteLine($"tempAir: {tempAir:F2}");
            Debug.WriteLine($"cpFuel: {cpFuel:F2}");
            Debug.WriteLine($"tempFuel: {tempFuel:F2}");
            Debug.WriteLine($"i3i4: {i3i4:F2}");

            double L = alpha == 1 ? La : LaWet;
            
            double term1 = Qn / VaTotal;
            double term2 = (L * cpDryAir * tempAir) / VaTotal;
            double term3 = (cpFuel * tempFuel) / VaTotal;
            double term4 = i3i4 / VaTotal;
            
            var result = term1 + term2 + term3 - term4;
            
            Debug.WriteLine($"\nCalculation terms:");
            Debug.WriteLine($"term1 (Qn/VaTotal): {term1:F2}");
            Debug.WriteLine($"term2 (L*cpDryAir*tempAir/VaTotal): {term2:F2}");
            Debug.WriteLine($"term3 (cpFuel*tempFuel/VaTotal): {term3:F2}");
            Debug.WriteLine($"term4 (i3i4/VaTotal): {term4:F2}");
            Debug.WriteLine($"Final result: {result:F2}");
            
            return result;
        }

        #endregion

        #region Вспомогательные методы
        public static double CalculateV(double M, double VaTotal) => M / VaTotal * 100;

        public static double CalculateTotal(double CO2, double CH4, double C2H6, double C3H8, double C4H10, 
            double C5H12, double C2H4, double C2H2, double SO2, double N2, double O2, double H2O, double CO, 
            double H2S, double H2) => 
            CO2 + CH4 + C2H6 + C3H8 + C4H10 + C5H12 + C2H4 + C2H2 + SO2 + N2 + O2 + H2O + CO + H2S + H2;
        #endregion
    }
}
