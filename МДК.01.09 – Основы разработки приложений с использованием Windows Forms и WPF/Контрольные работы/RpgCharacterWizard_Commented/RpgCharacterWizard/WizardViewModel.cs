using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RpgCharacterWizard;

// Здесь хранятся данные персонажа и правила перехода между экранами.
public sealed class WizardViewModel : INotifyPropertyChanged
{
    // Всего можно вложить 10 очков в силу, ловкость и интеллект.
    private const int TotalPoints = 10;
    // Начинаем с выбора расы. Шаг 4 показывает готовую карточку.
    private int step = 1;
    private int strength;
    private int agility;
    private int intelligence;
    private CharacterOption? selectedRace;
    private CharacterOption? selectedClass;

    public WizardViewModel()
    {
        // У каждой команды есть действие и условие. Например, «Назад» доступна со второго шага.
        BackCommand = new RelayCommand(() => Step--, () => Step > 1);
        NextCommand = new RelayCommand(() => Step++, CanGoNext);
        CreateCommand = new RelayCommand(() => Step = 4, CanCreate);
    }

    // Первое слово видит пользователь, второе используется в имени файла картинки.
    public IReadOnlyList<CharacterOption> Races { get; } =
    [
        new("Человек", "human"),
        new("Эльф", "elf"),
        new("Орк", "orc")
    ];

    public IReadOnlyList<CharacterOption> Classes { get; } =
    [
        new("Воин", "warrior"),
        new("Маг", "mage"),
        new("Вор", "thief")
    ];

    public RelayCommand BackCommand { get; }
    public RelayCommand NextCommand { get; }
    public RelayCommand CreateCommand { get; }

    public int Step
    {
        get => step;
        private set
        {
            // При смене шага обновляем экран, надпись сверху и кнопки.
            step = value;
            Notify();
            Notify(nameof(StepLabel));
            Notify(nameof(ShowNext));
            Notify(nameof(ShowCreate));
            RefreshCommands();
        }
    }

    public string StepLabel => Step < 4 ? $"Шаг {Step} из 3" : "Персонаж создан";
    public bool ShowNext => Step < 3;
    public bool ShowCreate => Step == 3;

    public CharacterOption? SelectedRace
    {
        get => selectedRace;
        set
        {
            if (selectedRace == value)
            {
                return;
            }

            // Запоминаем расу и обновляем аватар и доступность кнопок.
            selectedRace = value;
            Notify();
            Notify(nameof(AvatarPath));
            RefreshCommands();
        }
    }

    public CharacterOption? SelectedClass
    {
        get => selectedClass;
        set
        {
            if (selectedClass == value)
            {
                return;
            }

            // Запоминаем класс и обновляем предпросмотр.
            selectedClass = value;
            Notify();
            Notify(nameof(AvatarPath));
            RefreshCommands();
        }
    }

    public string? AvatarPath
    {
        get
        {
            // Пока раса не выбрана, показывать нечего.
            if (SelectedRace == null)
            {
                return null;
            }

            // До выбора класса показываем обычную иконку расы.
            if (SelectedClass == null)
            {
                return SelectedRace.IconPath;
            }

            // Из ключей расы и класса получаем имя аватара. Например, elf-mage.png.
            return CharacterOption.AssetPath($"{SelectedRace.Key}-{SelectedClass.Key}");
        }
    }

    // Ползунки читают эти свойства и записывают в них новые значения через SetStat.
    public int Strength
    {
        get => strength;
        set => SetStat(ref strength, value, MaxStrength);
    }

    public int Agility
    {
        get => agility;
        set => SetStat(ref agility, value, MaxAgility);
    }

    public int Intelligence
    {
        get => intelligence;
        set => SetStat(ref intelligence, value, MaxIntelligence);
    }

    // Вычитаем потраченные очки из 10. Ползунок можно поднять только на оставшееся число очков.
    public int RemainingPoints => TotalPoints - Strength - Agility - Intelligence;
    public int MaxStrength => Strength + RemainingPoints;
    public int MaxAgility => Agility + RemainingPoints;
    public int MaxIntelligence => Intelligence + RemainingPoints;

    public event PropertyChangedEventHandler? PropertyChanged;

    // С первого шага можно уйти после выбора расы, со второго после распределения всех очков.
    private bool CanGoNext() => Step switch
    {
        1 => SelectedRace != null,
        2 => RemainingPoints == 0,
        _ => false
    };

    // Для создания нужны третий шаг, выбранные раса и класс и нулевой остаток очков.
    private bool CanCreate() =>
        Step == 3 && SelectedRace != null && RemainingPoints == 0 && SelectedClass != null;

    private void SetStat(ref int stat, int value, int maximum, [CallerMemberName] string? property = null)
    {
        // Ограничиваем новое значение диапазоном от нуля до максимума.
        int newValue = Math.Clamp(value, 0, maximum);
        if (stat == newValue)
        {
            return;
        }

        // Обновляем характеристику, остаток очков и пределы всех ползунков. ref позволяет изменить переданное поле.
        stat = newValue;
        Notify(property);
        Notify(nameof(RemainingPoints));
        Notify(nameof(MaxStrength));
        Notify(nameof(MaxAgility));
        Notify(nameof(MaxIntelligence));
        RefreshCommands();
    }

    private void RefreshCommands()
    {
        BackCommand.Refresh();
        NextCommand.Refresh();
        CreateCommand.Refresh();
    }

    // Сообщаем WPF, какое свойство изменилось. Если имя не передали, C# подставит имя свойства, откуда вызван Notify.
    private void Notify([CallerMemberName] string? property = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
}

public sealed record CharacterOption(string Name, string Key)
{
    public string IconPath => AssetPath(Key);

    // Этот адрес говорит WPF взять PNG из папки Assets внутри приложения.
    public static string AssetPath(string name) =>
        $"pack://application:,,,/Assets/{name}.png";
}
