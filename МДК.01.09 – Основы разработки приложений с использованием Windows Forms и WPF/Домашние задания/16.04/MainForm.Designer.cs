using System;
using System.Drawing;
using System.Windows.Forms;

partial class MainForm
{

    private void InitializeComponent()
    {
        SuspendLayout();
        Text = "Заказ пиццы";
        ClientSize = new Size(710, 550);
        Font = new Font("Segoe UI", 10);
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        BackColor = Color.FromArgb(86, 67, 145);
        TabControl tabs = new TabControl { Dock = DockStyle.Fill };
        TabPage login = new TabPage("Вход");
        TabPage order = new TabPage("Заказ");
        TabPage pay = new TabPage("Оплата");
        tabs.TabPages.AddRange(new TabPage[] { login, order, pay });
        Controls.Add(tabs);
        GroupBox loginGroup = new GroupBox { Text = "Вход", Location = new Point(180, 120), Size = new Size(330, 220) };
        login.Controls.Add(loginGroup);
        loginGroup.Controls.Add(new Label { Text = "E-mail", Location = new Point(25, 45), Size = new Size(90, 28) });
        loginGroup.Controls.Add(new Label { Text = "Пароль", Location = new Point(25, 95), Size = new Size(90, 28) });

        loginGroup.Controls.Add(new TextBox { Location = new Point(120, 42), Size = new Size(180, 28) });
        loginGroup.Controls.Add(new TextBox { Location = new Point(120, 92), Size = new Size(180, 28), UseSystemPasswordChar = true });
        loginGroup.Controls.Add(new Button { Text = "Войти", Location = new Point(120, 150), Size = new Size(180, 36) });
        order.BackColor = Color.Lavender;
        order.Controls.Add(new Label { Text = "Вид пиццы", Location = new Point(20, 25), Size = new Size(190, 28) });

        ComboBox pizza = new ComboBox { Location = new Point(20, 55), Size = new Size(220, 28), DropDownStyle = ComboBoxStyle.DropDownList };
        pizza.Items.AddRange(new object[] { "Пепперони", "Четыре сыра", "Маргарита" });
        pizza.SelectedIndex = 0;
        order.Controls.Add(pizza);
        order.Controls.Add(new Label { Text = "Количество", Location = new Point(20, 100), Size = new Size(190, 28) });

        order.Controls.Add(new NumericUpDown { Location = new Point(20, 130), Size = new Size(220, 28), Minimum = 1, Maximum = 20, Value = 1 });
        order.Controls.Add(new Label { Text = "Добавки", Location = new Point(20, 175), Size = new Size(190, 28) });

        CheckedListBox extras = new CheckedListBox { Location = new Point(20, 205), Size = new Size(220, 150), CheckOnClick = true };
        extras.Items.AddRange(new object[] { "Двойной сыр", "Томаты", "Грибы", "Лук", "Перец", "Оливки" });
        order.Controls.Add(extras);
        order.Controls.Add(new Button { Text = "Добавить пиццу", Location = new Point(20, 380), Size = new Size(220, 36) });
        order.Controls.Add(new Button { Text = "Оформить заказ", Location = new Point(20, 430), Size = new Size(220, 36) });
        GroupBox pizza0 = new GroupBox { Text = "ПЕППЕРОНИ", Location = new Point(275, 25), Size = new Size(390, 120) };
        pizza0.Controls.Add(new Label { Text = "🍕", Font = new Font("Segoe UI Emoji", 36), Location = new Point(15, 28), Size = new Size(100, 75) });
        pizza0.Controls.Add(new Label { Text = "Свежая пицца для вас", Location = new Point(125, 45), AutoSize = true });
        order.Controls.Add(pizza0);
        GroupBox pizza1 = new GroupBox { Text = "ЧЕТЫРЕ СЫРА", Location = new Point(275, 165), Size = new Size(390, 120) };
        pizza1.Controls.Add(new Label { Text = "🍕", Font = new Font("Segoe UI Emoji", 36), Location = new Point(15, 28), Size = new Size(100, 75) });
        pizza1.Controls.Add(new Label { Text = "Свежая пицца для вас", Location = new Point(125, 45), AutoSize = true });
        order.Controls.Add(pizza1);
        GroupBox pizza2 = new GroupBox { Text = "МАРГАРИТА", Location = new Point(275, 305), Size = new Size(390, 120) };
        pizza2.Controls.Add(new Label { Text = "🍕", Font = new Font("Segoe UI Emoji", 36), Location = new Point(15, 28), Size = new Size(100, 75) });
        pizza2.Controls.Add(new Label { Text = "Свежая пицца для вас", Location = new Point(125, 45), AutoSize = true });
        order.Controls.Add(pizza2);
        pay.Controls.Add(new Label { Text = "Адрес доставки", Location = new Point(30, 40), Size = new Size(200, 28) });
        pay.Controls.Add(new TextBox { Location = new Point(30, 75), Size = new Size(590, 28) });
        pay.Controls.Add(new RadioButton { Text = "Наличными", Location = new Point(30, 135), Checked = true, AutoSize = true });
        pay.Controls.Add(new RadioButton { Text = "Картой", Location = new Point(220, 135), AutoSize = true });
        pay.Controls.Add(new Button { Text = "Подтвердить заказ", Location = new Point(30, 200), Size = new Size(250, 40) });

        ResumeLayout(false);
        PerformLayout();
    }
}
