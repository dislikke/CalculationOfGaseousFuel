using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;


namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Устанавливаем значения по умолчанию для всех TextBox'ов
            txtCO.Text = "0";
            txtH2.Text = "0";
            txtCH4.Text = "0";
            txtC2H6.Text = "0";
            txtC3H8.Text = "0";
            txtC4H10.Text = "0";
            txtC5H12.Text = "0";
            txtCO2.Text = "0";
            txtN2.Text = "0";
            txtO2.Text = "0";
            txtH2O.Text = "0";
            txtH2S.Text = "0";
            txtSO2.Text = "0";
            txtC2H4.Text = "0";
            txtC2H2.Text = "0";

            // Значения по умолчанию для других параметров
            txtMoistureContent.Text = "0";
            txtAlpha.Text = "0";
            txtTempFuel.Text = "0";
            txtTempAir.Text = "0";
            txtI3I4.Text = "0";
        }
        private double VO2, L0, L0Wet, La, LaWet;
        private double V0CO2, V0SO2, V0N2, V0H2O, VaO2, VaN2, VaH2O, VaTotal, V0Total;

        private void bttChart_Click(object sender, EventArgs e)
        {
            var chartForm = new ChartForm(Qn, t0, t0b, ta, tab, VL, iT0, iT0b,  iTa, iTab);
            chartForm.ShowDialog();
        }

        private double VL, Qn, averageCp, cpDryAir;
        private double iT0, iT0b, iTa, iTab;
        private double t0, t0b, ta, tab;



        private void btnCalculate_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. Считываем входные данные
                InputData input = InputReader.ReadInputs(this);

                // 2. Расчёт VO2 и воздуха
                VO2 = CombustionCalculator.CalculateVO2(input);
                L0 = CombustionCalculator.CalculateL0(VO2);
                L0Wet = CombustionCalculator.CalculateL0Wet(L0, input.Moisture);
                La = CombustionCalculator.CalculateLa(L0, input.Alpha);
                LaWet = CombustionCalculator.CalculateLaWet(L0Wet, input.Alpha);

                // 3. Расчёт объёмов продуктов сгорания
                V0CO2 = CombustionCalculator.CalculateV0CO2(input);
                V0SO2 = CombustionCalculator.CalculateV0SO2(input);
                V0N2 = CombustionCalculator.CalculateV0N2(input, L0);
                V0H2O = CombustionCalculator.CalculateV0H2O(input, input.Moisture, L0);
                VaO2 = CombustionCalculator.CalculateVaO2(VO2, input.Alpha);
                VaN2 = CombustionCalculator.CalculateVaN2(V0N2, L0, input.Alpha);
                VaH2O = CombustionCalculator.CalculateVaH2O(V0H2O, L0, input.Alpha, input.Moisture);
                VaTotal = CombustionCalculator.CalculateVaTotal(V0CO2, V0SO2, VaN2, VaO2, VaH2O);
                V0Total = CombustionCalculator.CalculateV0Total(V0CO2, V0SO2, V0N2, V0H2O);


                // 4. Расчёт влагосодержания
                VL = CombustionCalculator.CalculateVL(LaWet, L0Wet, VaTotal);

                // 5. Расчёт теплоты сгорания
                Qn = CombustionCalculator.CalculateQn(input);
                averageCp = HeatCapacityCalculator.CalculateAverageCp(input, input.TempFuel);
                cpDryAir = HeatCapacityCalculator.CalculateCpDryAir(input.TempAir);

                // 6. Расчёт энтальпии (iТобщ или iбобщ)
                iT0 = CombustionCalculator.CalculateITTotal(Qn, V0Total);
                iT0b = CombustionCalculator.CalculateIBTotal(Qn, V0Total, input.I3I4);
                iTa = CombustionCalculator.CalculateITGeneral(input.Alpha, La, Qn, VaTotal, LaWet, cpDryAir, input.TempAir, averageCp, input.TempFuel);
                iTab = CombustionCalculator.CalculateIBGeneral(input.Alpha, La, Qn, VaTotal, LaWet, cpDryAir, input.TempAir, averageCp, input.TempFuel, input.I3I4);

                



                // 7. Температура по интерполяции
                try
                {
                    Debug.WriteLine("\n=== Начало расчета температур ===");
                    Debug.WriteLine($"Qn = {Qn:F2} кДж/м³");
                    Debug.WriteLine($"VL = {VL:F2}%");
                    
                    // Проверяем корректность значения VL
                    if (VL < 0 || VL > 100)
                    {
                        throw new ArgumentException($"Значение VL ({VL:F2}%) должно быть в диапазоне 0-100%");
                    }

                    // Определяем, какую таблицу использовать на основе Qn
                    bool isHighQn = Qn > 8400;
                    Debug.WriteLine($"Используем таблицу для {(isHighQn ? "высоких" : "низких")} значений Qn");

                    // Проверяем корректность энтальпий
                    Debug.WriteLine("\nРассчитанные значения энтальпий:");
                    Debug.WriteLine($"iT0 = {iT0:F2} кДж/м³");
                    Debug.WriteLine($"iT0b = {iT0b:F2} кДж/м³");
                    Debug.WriteLine($"iTa = {iTa:F2} кДж/м³");
                    Debug.WriteLine($"iTab = {iTab:F2} кДж/м³");

                    if (iT0 <= 0 || iT0b <= 0 || iTa <= 0 || iTab <= 0)
                    {
                        throw new ArgumentException("Обнаружены некорректные (отрицательные или нулевые) значения энтальпий");
                    }

                    // Округляем VL до ближайшего табличного значения
                    int[] validVLValues = { 0, 20, 40, 60, 80, 100 };
                    int roundedVL = validVLValues.OrderBy(x => Math.Abs(x - VL)).First();
                    Debug.WriteLine($"\nОкругление VL {VL:F2}% до ближайшего табличного значения: {roundedVL}%");

                    // Расчет температур с подробным логированием
                    t0 = TemperatureTable.CalculateTemperature(iT0, 0, Qn, false);
                    t0b = TemperatureTable.CalculateTemperature(iT0b, 0, Qn, true);
                    ta = TemperatureTable.CalculateTemperature(iTa, VL, Qn, false);
                    tab = TemperatureTable.CalculateTemperature(iTab, VL, Qn, true);

                    // Проверяем корректность полученных температур
                    if (t0 <= 0 || t0b <= 0 || ta <= 0 || tab <= 0)
                    {
                        throw new ArgumentException("Получены некорректные (отрицательные или нулевые) значения температур");
                    }

                    // Выводим результаты в текстовые поля
                    txtOutputT0.Text = t0.ToString("F2");
                    txtOutputTb.Text = t0b.ToString("F2");
                    txtOutputT0a.Text = ta.ToString("F2");
                    txtOutputTba.Text = tab.ToString("F2");


                    double CO2 = CombustionCalculator.CalculateV(V0CO2, VaTotal);
                    double SO2 = CombustionCalculator.CalculateV(V0SO2, VaTotal);
                    double N2 = CombustionCalculator.CalculateV(VaN2, VaTotal);
                    double O2 = CombustionCalculator.CalculateV(VaO2, VaTotal);
                    double H2O = CombustionCalculator.CalculateV(VaH2O, VaTotal);
                    double S = CombustionCalculator.CalculateVaTotal(CO2, SO2, N2, O2, H2O);

                    Debug.WriteLine("\n=== Расчет температур успешно завершен ===");

                    // Выводим сводку результатов
                    string results = $"Результаты расчета:\n\n" +
                                   $"Qn = {Qn:F2} кДж/м³\n" +
                                   $"VL = {VL:F2}% \n\n" +
                                   $"Энтальпии:\n" +
                                   $"Энтальпия iT (a=1) = {iT0:F2} кДж/м³ → t0 = {t0:F2}°C\n" +
                                   $"Энтальпия iб  (a=1) = {iT0b:F2} кДж/м³ → t0б = {t0b:F2}°C\n" +
                                   $"Энтальпия iT = {iTa:F2} кДж/м³ → ta = {ta:F2}°C\n" +
                                   $"Энтальпия iб = {iTab:F2} кДж/м³ → taб = {tab:F2}°C\n" +
                                   $"Средняя теплоемкость топлива = {averageCp:F2} кДж/м³\n" +
                                   $"Средняя теплоемкость воздуха = {cpDryAir:F2} кДж/м³\n";
                    string calculatedValue = $"Σ = CO₂ + H₂O + N₂ + O₂ = {CO2:F2}% + {SO2:F2}% + {N2:F2}% + {O2:F2}% + {H2O:F2}% = {S:F2}%";
                    double total = CombustionCalculator.CalculateTotal(input.CO2, input.CH4, input.C2H6, input.C3H8, input.C4H10, input.C5H12, input.C2H4, input.C2H2, input.SO2, input.N2, input.O2, input.H2O, input.CO, input.H2S, input.H2);
                    string calculatedValue2 = $"+N₂ +CO + SO₂ +H₂S + H₂ +H₂O + O₂ = {total:F2}%";
                    txtOutput.Text = results;

                    labelSUM.Text = calculatedValue2;
                    labelSUM2.Text = calculatedValue;


                    


                    MessageBox.Show(results, "Результаты расчета", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"\nОШИБКА: {ex.Message}");
                    Debug.WriteLine(ex.StackTrace);

                    string errorMessage = $"Ошибка при расчете температур:\n\n{ex.Message}\n\n" +
                                       $"Текущие значения:\n" +
                                       $"Qn = {Qn:F2} кДж/м³\n" +
                                       $"VL = {VL:F2}%\n" +
                                       $"iT0 = {iT0:F2} кДж/м³\n" +
                                       $"iT0b = {iT0b:F2} кДж/м³\n" +
                                       $"iTa = {iTa:F2} кДж/м³\n" +
                                       $"iTab = {iTab:F2} кДж/м³\n\n" +
                                       "Подробности см. в файле debug.log";

                    MessageBox.Show(errorMessage, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            txtOutput.Clear();
            txtOutputT0.Text = "";
            txtOutputTb.Text = "";
            txtOutputT0a.Text = "";
            txtOutputTba.Text = "";
            labelSUM.Text = "+N₂ +CO + SO₂ +H₂S + H₂ +H₂O + O₂ =";
            labelSUM2.Text = "Σ = CO₂ + H₂O + N₂ + O₂ =";
            txtOutputT0.Text = "температура";
            txtOutputTb.Text = "температура";
            txtOutputT0a.Text = "температура";
            txtOutputTba.Text = "температура";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "CSV Files|*.csv",
                    FileName = "результаты_расчета.csv"
                };

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Создаем список строк для CSV
                    var lines = new List<string>();

                    // Добавляем заголовки
                    string headers = string.Join(";", new string[] {
                        "VO2", "L0", "L0в", "La", "Laв",
                        "V0(CO2)", "V0(SO2)", "V0(N2)", "V0(H2O)", "V0",
                        "Va(CO2)", "Va(SO2)", "Va(N2)", "Va(O2)", "Va(H2O)",
                        "Va", "Qн", "iТобщ(a=1)", "iбобщ(a=1)",
                        "cт", "cв", "iТобщ", "iбобщ", "VL",
                        "t0T", "t0б", "taT", "taб"
                    });
                    lines.Add(headers);

                    // Добавляем значения
                    string values = string.Join(";", new string[] {
                        VO2.ToString("F2"),
                        L0.ToString("F2"),
                        L0Wet.ToString("F2"),
                        La.ToString("F2"),
                        LaWet.ToString("F2"),
                        V0CO2.ToString("F2"),
                        V0SO2.ToString("F2"),
                        V0N2.ToString("F2"),
                        V0H2O.ToString("F2"),
                        V0Total.ToString("F2"),
                        V0CO2.ToString("F2"),
                        V0SO2.ToString("F2"),
                        VaN2.ToString("F2"),
                        VaO2.ToString("F2"),
                        VaH2O.ToString("F2"),
                        VaTotal.ToString("F2"),
                        Qn.ToString("F2"),
                        iT0.ToString("F2"),
                        iT0b.ToString("F2"),
                        averageCp.ToString("F2"),
                        cpDryAir.ToString("F2"),
                        iTa.ToString("F2"),
                        iTab.ToString("F2"),
                        VL.ToString("F2"),
                        t0.ToString("F2"),
                        t0b.ToString("F2"),
                        ta.ToString("F2"),
                        tab.ToString("F2")
                    });
                    lines.Add(values);

                    // Записываем в файл с кодировкой UTF-8
                    File.WriteAllLines(saveFileDialog.FileName, lines, Encoding.UTF8);

                    MessageBox.Show("Файл успешно сохранен!\nФайл можно открыть в Excel.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Спрашиваем пользователя, хочет ли он открыть файл
                    if (MessageBox.Show("Хотите открыть созданный файл?", "Открыть файл", 
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        Process.Start(saveFileDialog.FileName);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении файла: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        

    }
}

