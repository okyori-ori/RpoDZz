using System;

class Program
{
    static int ширинаКарты = 10;
    static int высотаКарты = 10;
    static int игрокX = 0;
    static int игрокY = 0;
    static int монстрX = 9;
    static int монстрY = 9;
    static int ход = 0;

    static void Main()
    {
        Console.Title = "УБЕГИ ОТ МОНСТРА";

        while (true)
        {
            Console.Clear();
            РисоватьКарту();

            if (игрокX == монстрX && игрокY == монстрY)
            {
                Console.WriteLine("\n=========================================");
                Console.WriteLine("        ВАС СЪЕЛ МОНСТР! ");
                Console.WriteLine("=========================================");
                Console.WriteLine($"Вы продержались {ход} ходов!");
                Console.ReadLine();
                break;
            }

            if (ход >= 20)
            {
                Console.WriteLine("\n=========================================");
                Console.WriteLine("        ВЫ ПОБЕДИЛИ! ");
                Console.WriteLine("=========================================");
                Console.WriteLine("Вы продержались 20 ходов и сбежали!");
                Console.ReadLine();
                break;
            }

            Console.WriteLine($"\nХод: {ход}/20");
            Console.WriteLine("Управление: W (вверх), S (вниз), A (влево), D (вправо)");
            Console.Write("Ваш ход: ");

            string клавиша = Console.ReadLine().ToLower();

            if (клавиша == "w" && игрокY > 0) игрокY--;
            else if (клавиша == "s" && игрокY < высотаКарты - 1) игрокY++;
            else if (клавиша == "a" && игрокX > 0) игрокX--;
            else if (клавиша == "d" && игрокX < ширинаКарты - 1) игрокX++;
            else
            {
                Console.WriteLine("Неверная клавиша или граница!");
                Console.ReadLine();
                continue;
            }

            ДвигатьМонстра();
            ход++;
        }
    }

    static void РисоватьКарту()
    {
        Console.WriteLine("=== КАРТА ===");

        for (int y = 0; y < высотаКарты; y++)
        {
            for (int x = 0; x < ширинаКарты; x++)
            {
                if (x == игрокX && y == игрокY)
                    Console.Write("P "); 
                else if (x == монстрX && y == монстрY)
                    Console.Write("M "); 
                else
                    Console.Write(". ");
            }
            Console.WriteLine();
        }
    }

    static void ДвигатьМонстра()
    {
        if (монстрX < игрокX && монстрX < ширинаКарты - 1)
            монстрX++;
        else if (монстрX > игрокX && монстрX > 0)
            монстрX--;
        else if (монстрY < игрокY && монстрY < высотаКарты - 1)
            монстрY++;
        else if (монстрY > игрокY && монстрY > 0)
            монстрY--;
    }
}