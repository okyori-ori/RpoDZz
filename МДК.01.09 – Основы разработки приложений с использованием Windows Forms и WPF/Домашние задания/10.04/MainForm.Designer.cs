using System;
using System.Drawing;
using System.Windows.Forms;

partial class MainForm
{

    private void InitializeComponent()
    {
        SuspendLayout();
        Text = "Заказ такси";
        ClientSize = new Size(530, 540);
        Font = new Font("Segoe UI", 10);
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        Controls.Add(new Label { Text = "Заказ такси", Location = new Point(25, 20), Size = new Size(400, 28) });
        Controls.Add(new Label { Text = "Ваше имя", Location = new Point(25, 70), Size = new Size(150, 28) });
        TextBox nameBox = new TextBox { Location = new Point(180, 70), Size = new Size(320, 28) };
        Controls.Add(nameBox);
        Controls.Add(new Label { Text = "Телефон", Location = new Point(25, 115), Size = new Size(150, 28) });
        TextBox phoneBox = new TextBox { Location = new Point(180, 115), Size = new Size(320, 28) };
        Controls.Add(phoneBox);
        Controls.Add(new Label { Text = "Откуда", Location = new Point(25, 160), Size = new Size(150, 28) });
        TextBox fromBox = new TextBox { Location = new Point(180, 160), Size = new Size(320, 28) };
        Controls.Add(fromBox);
        Controls.Add(new Label { Text = "Куда", Location = new Point(25, 205), Size = new Size(150, 28) });
        TextBox toBox = new TextBox { Location = new Point(180, 205), Size = new Size(320, 28) };
        Controls.Add(toBox);
        Controls.Add(new Label { Text = "Класс автомобиля", Location = new Point(25, 250), Size = new Size(150, 28) });
        ComboBox car = new ComboBox { Location = new Point(180, 250), Size = new Size(320, 28), DropDownStyle = ComboBoxStyle.DropDownList };
        car.Items.AddRange(new object[] { "Эконом", "Комфорт", "Бизнес" });
        car.SelectedIndex = 0;
        Controls.Add(car);
        Controls.Add(new Label { Text = "Время подачи", Location = new Point(25, 295), Size = new Size(150, 28) });
        DateTimePicker time = new DateTimePicker { Location = new Point(180, 295), Size = new Size(320, 28), Format = DateTimePickerFormat.Custom, CustomFormat = "dd.MM.yyyy HH:mm" };
        Controls.Add(time);
        GroupBox payment = new GroupBox { Text = "Оплата", Location = new Point(25, 340), Size = new Size(475, 70) };
        payment.Controls.Add(new RadioButton { Text = "Наличными", Location = new Point(20, 27), Checked = true, AutoSize = true });
        payment.Controls.Add(new RadioButton { Text = "Картой", Location = new Point(235, 27), AutoSize = true });
        Controls.Add(payment);
        Controls.Add(new CheckBox { Text = "Детское кресло", Location = new Point(25, 425), AutoSize = true });
        Controls.Add(new Button { Text = "Заказать такси", Location = new Point(180, 470), Size = new Size(320, 42), BackColor = Color.Gold });

        ResumeLayout(false);
        PerformLayout();
    }
}
