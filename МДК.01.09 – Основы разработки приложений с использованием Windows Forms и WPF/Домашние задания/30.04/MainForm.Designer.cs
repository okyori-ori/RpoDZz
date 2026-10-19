using System;
using System.Drawing;
using System.Windows.Forms;

partial class MainForm
{
    private Label dateLabel;
    private Label resultLabel;
    private DataGridView ratesGrid;
    private ComboBox fromBox;
    private ComboBox toBox;
    private NumericUpDown amountBox;
    private Button refreshButton;
    private System.Collections.Generic.Dictionary<string, decimal> rates = new System.Collections.Generic.Dictionary<string, decimal>();

    private void InitializeComponent()
    {
        SuspendLayout();
        Text = "Курсы валют и конвертация";
        ClientSize = new Size(660, 480);
        Font = new Font("Segoe UI", 10);
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        dateLabel = new Label { Text = "Загрузка курсов...", Location = new Point(20, 20), Size = new Size(620, 30) };
        Controls.Add(dateLabel);
        ratesGrid = new DataGridView { Location = new Point(20, 60), Size = new Size(620, 190), ReadOnly = true, AllowUserToAddRows = false, AllowUserToDeleteRows = false, RowHeadersVisible = false, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };
        ratesGrid.Columns.Add("Code", "Валюта");
        ratesGrid.Columns.Add("Name", "Название");
        ratesGrid.Columns.Add("Rate", "Рублей за 1 единицу");
        Controls.Add(ratesGrid);
        amountBox = new NumericUpDown { Location = new Point(20, 290), Size = new Size(170, 30), DecimalPlaces = 2, Maximum = 100000000, Value = 100 };
        fromBox = new ComboBox { Location = new Point(210, 290), Size = new Size(160, 30), DropDownStyle = ComboBoxStyle.DropDownList };
        toBox = new ComboBox { Location = new Point(390, 290), Size = new Size(160, 30), DropDownStyle = ComboBoxStyle.DropDownList };
        Controls.Add(amountBox);
        Controls.Add(fromBox);
        Controls.Add(toBox);
        Controls.Add(new Label { Text = "Сумма", Location = new Point(20, 260), Size = new Size(150, 28) });
        Controls.Add(new Label { Text = "Из валюты", Location = new Point(210, 260), Size = new Size(150, 28) });
        Controls.Add(new Label { Text = "В валюту", Location = new Point(390, 260), Size = new Size(150, 28) });

        Button convert = new Button { Text = "Конвертировать", Location = new Point(20, 340), Size = new Size(250, 38) };
        convert.Click += Convert_Click;
        Controls.Add(convert);
        refreshButton = new Button { Text = "Обновить курсы", Location = new Point(390, 340), Size = new Size(250, 38) };
        refreshButton.Click += Refresh_Click;
        Controls.Add(refreshButton);
        resultLabel = new Label { Location = new Point(20, 400), Size = new Size(620, 65) };
        Controls.Add(resultLabel);
        Shown += Refresh_Click;

        ResumeLayout(false);
        PerformLayout();
    }
}
