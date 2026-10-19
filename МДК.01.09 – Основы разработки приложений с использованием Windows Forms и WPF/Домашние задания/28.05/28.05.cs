using System;
using System.Windows;
using System.Windows.Controls;
using System.Globalization;

namespace Homework
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

        }

        private void Calculate_Click(object sender, RoutedEventArgs e)
        {
            TextBox[] boxes = { Day0, Day1, Day2, Day3, Day4, Day5, Day6 };
            decimal sum = 0;
            decimal maximum = decimal.MinValue;
            foreach (TextBox box in boxes)
            {
                decimal value;
                if (!decimal.TryParse(box.Text.Replace(',', '.'), NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign,
                    CultureInfo.InvariantCulture, out value)) value = 0;
                sum += value;
                if (value > maximum) maximum = value;
            }
            ResultBlock.Text = $"Сумма: {sum:F2}\nСредний расход: {sum / 7:F2}\nМаксимум: {maximum:F2}";
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            TextBox[] boxes = { Day0, Day1, Day2, Day3, Day4, Day5, Day6 };
            foreach (TextBox box in boxes) box.Clear();
            ResultBlock.Text = "Сумма: 0\nСредний расход: 0\nМаксимум: 0";
        }

    }
}
