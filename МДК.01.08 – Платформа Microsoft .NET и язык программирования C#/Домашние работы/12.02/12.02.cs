using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        Console.InputEncoding = System.Text.Encoding.UTF8;
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        string[] words = { "программа", "монитор", "клавиатура", "компьютер", "массив" };
        Random random = new Random();
        string word = words[random.Next(words.Length)];
        List<char> letters = new List<char>();
        int errors = 0;
        while (errors < 6 && !IsGuessed(word, letters))
        {
            DrawHangman(errors);
            ShowWord(word, letters);
            Console.WriteLine("Введённые буквы: " + string.Join(" ", letters));
            Console.Write("Введите букву: ");
            string input = Console.ReadLine();
            if (input == null) return;
            input = input.Trim().ToLowerInvariant();
            if (input.Length != 1 || !char.IsLetter(input[0]))
            {
                Console.WriteLine("Введите одну букву.");
                continue;
            }
            char letter = input[0];
            if (letters.Contains(letter))
            {
                Console.WriteLine("Эта буква уже была.");
                continue;
            }
            letters.Add(letter);
            if (!word.Contains(letter)) errors++;
        }
        DrawHangman(errors);
        ShowWord(word, letters);
        Console.WriteLine(IsGuessed(word, letters) ? "Вы победили!" : "Вы проиграли.");
        Console.WriteLine("Слово: " + word);
    }

    static bool IsGuessed(string word, List<char> letters)
    {
        foreach (char letter in word)
            if (!letters.Contains(letter)) return false;
        return true;
    }

    static void ShowWord(string word, List<char> letters)
    {
        foreach (char letter in word)
            Console.Write(letters.Contains(letter) ? letter + " " : "_ ");
        Console.WriteLine();
    }

    static void DrawHangman(int errors)
    {
        Console.WriteLine(" +---+");
        Console.WriteLine(" |   |");
        Console.WriteLine(errors >= 1 ? " O   |" : "     |");
        string body = errors >= 4 ? "/|\\" : errors >= 3 ? "/|" : errors >= 2 ? " |" : "";
        Console.WriteLine(body.PadRight(5) + "|");
        string legs = errors >= 6 ? "/ \\" : errors >= 5 ? "/" : "";
        Console.WriteLine(legs.PadRight(5) + "|");
        Console.WriteLine("=====+");
        Console.WriteLine($"Ошибок: {errors} из 6");
    }
}
