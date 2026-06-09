
using System;

class Program
{
    static string[] words =
    {
        "машина",
        "телефон",
        "компьютер",
        "программа",
        "алгоритм"
    };

    static Random random = new Random();

    static void Main()
    {
        string secretWord = GetRandomWord();

        char[] guessedWord = CreateHiddenWord(secretWord);

        int attempts = 6;

        Console.WriteLine("=== Игра Виселица ===");

        while (attempts > 0)
        {
            Console.WriteLine("\nСлово: " + new string(guessedWord));
            Console.WriteLine("Попыток осталось: " + attempts);

            Console.Write("Введите букву: ");
            char letter = Convert.ToChar(Console.ReadLine());

            bool result = CheckLetter(secretWord, guessedWord, letter);

            if (result == true)
            {
                Console.WriteLine("Буква есть!");
            }
            else
            {
                Console.WriteLine("Такой буквы нет!");
                attempts--;
            }
            if (IsWordGuessed(guessedWord))
            {
                Console.WriteLine("\nВы выиграли!");
                Console.WriteLine("Слово: " + secretWord);
                return;
            }
        }

        Console.WriteLine("\nВы проиграли!");
        Console.WriteLine("Загаданное слово: " + secretWord);
    }
    static string GetRandomWord()
    {
        int index = random.Next(words.Length);
        return words[index];
    }
    static char[] CreateHiddenWord(string word)
    {
        char[] hidden = new char[word.Length];

        for (int i = 0; i < hidden.Length; i++)
        {
            hidden[i] = '_';
        }

        return hidden;
    }
    static bool CheckLetter(string secretWord, char[] guessedWord, char letter)
    {
        bool found = false;

        for (int i = 0; i < secretWord.Length; i++)
        {
            if (secretWord[i] == letter)
            {
                guessedWord[i] = letter;
                found = true;
            }
        }

        return found;
    }

    static bool IsWordGuessed(char[] guessedWord)
    {
        for (int i = 0; i < guessedWord.Length; i++)
        {
            if (guessedWord[i] == '_')
            {
                return false;
            }
        }

        return true;
    }
}

