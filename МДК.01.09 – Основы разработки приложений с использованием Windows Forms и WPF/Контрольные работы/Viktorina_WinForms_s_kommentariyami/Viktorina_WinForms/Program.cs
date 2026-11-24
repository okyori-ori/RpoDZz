using System;
using System.Windows.Forms;

namespace Viktorina_WinForms
{
    // Здесь начинается запуск всей программы.
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            // Включаем обычное оформление кнопок и других элементов Windows.
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Создаём и открываем окно Form1 с нашей викториной.
            Application.Run(new Form1());
        }
    }
}
