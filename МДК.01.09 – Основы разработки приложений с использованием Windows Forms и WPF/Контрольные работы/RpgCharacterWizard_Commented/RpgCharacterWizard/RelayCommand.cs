using System.Windows.Input;

namespace RpgCharacterWizard;

public sealed class RelayCommand : ICommand
{
    // Храним действие кнопки и проверку, можно ли его выполнить.
    private readonly Action execute;
    private readonly Func<bool> canExecute;

    public RelayCommand(Action execute, Func<bool> canExecute)
    {
        this.execute = execute;
        this.canExecute = canExecute;
    }

    public event EventHandler? CanExecuteChanged;

    // WPF вызывает этот метод, чтобы решить, активна ли кнопка.
    public bool CanExecute(object? parameter) => canExecute();

    public void Execute(object? parameter)
    {
        // Перед выполнением ещё раз проверяем условие.
        if (CanExecute(parameter))
        {
            execute();
        }
    }

    // Просим WPF снова проверить доступность кнопки.
    public void Refresh() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
}
