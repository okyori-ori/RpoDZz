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
            double amount;
            double rate;
            int months;
            if (!double.TryParse(AmountBox.Text.Replace(',', '.'), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out amount) || amount <= 0 ||
                !double.TryParse(RateBox.Text.Replace(',', '.'), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out rate) || rate < 0 ||
                !int.TryParse(MonthsBox.Text, out months) || months <= 0)
            {
                ResultBlock.Text = "Введите положительную сумму и срок, ставку не ниже 0.";
                return;
            }
            double monthlyRate = rate / 100 / 12;
            double payment = monthlyRate == 0 ? amount / months
                : amount * monthlyRate / (1 - Math.Pow(1 + monthlyRate, -months));
            double total = payment * months;
            if (!double.IsFinite(payment) || !double.IsFinite(total))
            {
                ResultBlock.Text = "Значения слишком велики для расчёта.";
                return;
            }
            ResultBlock.Text = $"Платёж в месяц: {payment:F2} руб.\nВсего выплат: {total:F2} руб.\nПереплата: {total - amount:F2} руб.";
        }

    }
}
