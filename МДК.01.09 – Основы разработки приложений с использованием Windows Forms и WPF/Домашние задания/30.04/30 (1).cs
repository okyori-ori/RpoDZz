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
        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
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

    private async void Refresh_Click(object sender, EventArgs e)
    {
        refreshButton.Enabled = false;
        try
        {
            using HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(20);
            byte[] data = await client.GetByteArrayAsync("https://www.cbr.ru/scripts/XML_daily.asp");
            using MemoryStream stream = new MemoryStream(data);
            XDocument document = XDocument.Load(stream);
            Dictionary<string, decimal> newRates = new Dictionary<string, decimal>();
            newRates.Add("RUB", 1m);
            ratesGrid.Rows.Clear();
            ratesGrid.Rows.Add("RUB", "Российский рубль", "1,0000");
            foreach (XElement currency in document.Root.Elements("Valute"))
            {
                string code = currency.Element("CharCode").Value;
                decimal nominal = decimal.Parse(currency.Element("Nominal").Value, CultureInfo.InvariantCulture);
                decimal value = decimal.Parse(currency.Element("Value").Value, CultureInfo.GetCultureInfo("ru-RU"));
                decimal rate = value / nominal;
                newRates.Add(code, rate);
                ratesGrid.Rows.Add(code, currency.Element("Name").Value, rate.ToString("F4"));
            }
            rates = newRates;
            fromBox.Items.Clear();
            toBox.Items.Clear();
            foreach (string code in rates.Keys)
            {
                fromBox.Items.Add(code);
                toBox.Items.Add(code);
            }
            fromBox.SelectedItem = "USD";
            toBox.SelectedItem = "RUB";
            dateLabel.Text = "Официальные курсы Банка России на " + document.Root.Attribute("Date").Value;
            resultLabel.Text = "Курсы загружены. Выберите валюты.";
        }
        catch (Exception ex)
        {
            resultLabel.Text = "Не удалось обновить курсы: " + ex.Message;
            if (rates.Count == 0) dateLabel.Text = "Курсы не загружены. Проверьте интернет.";
        }
        finally
        {
            refreshButton.Enabled = true;
        }
    }

    private void Convert_Click(object sender, EventArgs e)
    {
        if (fromBox.SelectedItem == null || toBox.SelectedItem == null)
        {
            resultLabel.Text = "Сначала загрузите курсы и выберите валюты.";
            return;
        }
        string from = fromBox.SelectedItem.ToString();
        string to = toBox.SelectedItem.ToString();
        decimal result = amountBox.Value * rates[from] / rates[to];
        resultLabel.Text = $"{amountBox.Value:F2} {from} = {result:F2} {to}";
    }

}
