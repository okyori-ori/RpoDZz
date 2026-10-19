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

}
