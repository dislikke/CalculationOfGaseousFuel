using System;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public static class InputReader
    {
        private static double Parse(Form form, string controlName)
        {
            var control = form.Controls.Find(controlName, true)[0] as TextBox;
            return double.Parse(control.Text);
        }

        public static InputData ReadInputs(Form form)
        {
            var data = new InputData();

            // Чтение компонентов газа
            data.CH4 = Parse(form, "txtCH4");
            data.CO = Parse(form, "txtCO");
            data.H2 = Parse(form, "txtH2");
            data.CO2 = Parse(form, "txtCO2");
            data.N2 = Parse(form, "txtN2");
            data.SO2 = Parse(form, "txtSO2");
            data.H2S = Parse(form, "txtH2S");
            data.H2O = Parse(form, "txtH2O");
            data.C2H6 = Parse(form, "txtC2H6");
            data.C3H8 = Parse(form, "txtC3H8");
            data.C4H10 = Parse(form, "txtC4H10");
            data.C5H12 = Parse(form, "txtC5H12");
            data.C2H2 = Parse(form, "txtC2H2");
            data.C2H4 = Parse(form, "txtC2H4");
            data.O2 = Parse(form, "txtO2");

            // Чтение параметров процесса
            data.Alpha = Parse(form, "txtAlpha");
            data.TempFuel = Parse(form, "txtTempFuel");
            data.TempAir = Parse(form, "txtTempAir");
            data.Moisture = Parse(form, "txtMoistureContent");
            data.I3I4 = Parse(form, "txtI3I4");

            return data;
        }
    }
}
