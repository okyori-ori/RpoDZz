using System.Windows;

namespace RpgCharacterWizard;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        // Загружаем элементы окна из MainWindow.xaml.
        InitializeComponent();
        // Создаём один объект с данными. Все шаги используют его, поэтому выбор сохраняется при переходах.
        DataContext = new WizardViewModel();
    }
}
