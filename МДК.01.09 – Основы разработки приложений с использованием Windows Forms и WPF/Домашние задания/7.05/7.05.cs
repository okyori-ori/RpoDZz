using System;
using System.Drawing;
using System.Windows.Forms;
using System.Globalization;
using System.Collections.Generic;
using System.Net.Http;
using System.Xml.Linq;
using System.IO;

static class Program
{
    [STAThread]
    static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new MainForm());
    }
}

public partial class MainForm : Form
{
    public MainForm()
    {
        InitializeComponent();
    }

    private void UpdateInfo(object sender, EventArgs e)
    {
        try
        {
            Microsoft.VisualBasic.Devices.ComputerInfo computer = new Microsoft.VisualBasic.Devices.ComputerInfo();
            double total = computer.TotalPhysicalMemory / 1073741824.0;
            double available = computer.AvailablePhysicalMemory / 1073741824.0;
            TimeSpan uptime = TimeSpan.FromMilliseconds(Environment.TickCount64);
            string text = $"Обновлено: {DateTime.Now:dd.MM.yyyy HH:mm:ss}\r\n";
            text += $"Компьютер: {Environment.MachineName}\r\n";
            text += $"Пользователь: {Environment.UserName}\r\n";
            text += $"ОС: {computer.OSFullName}\r\n";
            text += $"64-разрядная ОС: {(Environment.Is64BitOperatingSystem ? "Да" : "Нет")}\r\n";
            text += $"Логических процессоров: {Environment.ProcessorCount}\r\n";
            text += $"Оперативная память: {total:F2} ГБ\r\n";
            text += $"Свободная память: {available:F2} ГБ\r\n";
            text += $"Занято памяти: {total - available:F2} ГБ\r\n";
            text += $"Время работы: {uptime.Days} дн. {uptime.Hours} ч. {uptime.Minutes} мин.\r\n\r\n";
            foreach (DriveInfo drive in DriveInfo.GetDrives())
            {
                if (drive.IsReady && drive.DriveType == DriveType.Fixed)
                    text += $"Диск {drive.Name}: свободно {drive.AvailableFreeSpace / 1073741824.0:F2} из {drive.TotalSize / 1073741824.0:F2} ГБ\r\n";
            }
            infoBox.Text = text;
        }
        catch (Exception ex)
        {
            infoBox.Text = "Не удалось получить сведения: " + ex.Message;
        }
    }

}
