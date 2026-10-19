using System;
using System.Drawing;
using System.Windows.Forms;

partial class MainForm
{
    private TextBox infoBox;
    private System.Windows.Forms.Timer timer;

    private void InitializeComponent()
    {
        SuspendLayout();
        Text = "Состояние компьютера";
        ClientSize = new Size(680, 530);
        Font = new Font("Segoe UI", 10);
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        infoBox = new TextBox { Location = new Point(20, 20), Size = new Size(640, 430), Multiline = true, ReadOnly = true, ScrollBars = ScrollBars.Vertical };
        Controls.Add(infoBox);
        Button refresh = new Button { Text = "Обновить", Location = new Point(20, 470), Size = new Size(180, 35) };
        refresh.Click += UpdateInfo;
        Controls.Add(refresh);
        timer = new System.Windows.Forms.Timer { Interval = 3000 };
        timer.Tick += UpdateInfo;
        Shown += UpdateInfo;
        FormClosed += (sender, e) => timer.Dispose();
        timer.Start();

        ResumeLayout(false);
        PerformLayout();
    }
}
