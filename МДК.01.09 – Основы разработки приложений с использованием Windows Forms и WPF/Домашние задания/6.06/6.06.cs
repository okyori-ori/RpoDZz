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

        private void Shape_Changed(object sender, SelectionChangedEventArgs e)
        {
            if (FirstLabel == null || SecondBox == null) return;
            int shape = ShapeBox.SelectedIndex;
            FirstLabel.Text = shape == 0 ? "Ширина" : shape == 1 ? "Сторона" : shape == 2 ? "Радиус" : "Основание";
            bool second = shape == 0 || shape == 3;
            SecondLabel.Visibility = second ? Visibility.Visible : Visibility.Collapsed;
            SecondBox.Visibility = second ? Visibility.Visible : Visibility.Collapsed;
            ResultBlock.Text = "";
        }

        private void Calculate_Click(object sender, RoutedEventArgs e)
        {
            double first;
            double second = 0;
            int shape = ShapeBox.SelectedIndex;
            if (!ReadNumber(FirstBox.Text, out first) ||
                ((shape == 0 || shape == 3) && !ReadNumber(SecondBox.Text, out second)))
            {
                ResultBlock.Text = "Введите положительные числа.";
                return;
            }
            double area;
            if (shape == 0) area = first * second;
            else if (shape == 1) area = first * first;
            else if (shape == 2) area = Math.PI * first * first;
            else area = first * second / 2;
            ResultBlock.Text = double.IsFinite(area) ? $"Площадь: {area:F2}" : "Числа слишком большие.";
        }

        private bool ReadNumber(string text, out double value)
        {
            return double.TryParse(text.Replace(',', '.'), NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture, out value) && value > 0;
        }

    }
}
