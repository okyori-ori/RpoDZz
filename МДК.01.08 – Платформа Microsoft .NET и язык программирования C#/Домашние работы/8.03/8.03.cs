using System;

class Program
{
    static void Main()
    {
        Console.InputEncoding = System.Text.Encoding.UTF8;
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Random random = new Random();
        int playerX = 0;
        int playerY = 0;
        int monsterX;
        int monsterY;
        do
        {
            monsterX = random.Next(10);
            monsterY = random.Next(10);
        }
        while (monsterX == playerX && monsterY == playerY);

        int turns = 0;
        while (turns < 20)
        {
            DrawMap(playerX, playerY, monsterX, monsterY, turns);
            ConsoleKey key = Console.ReadKey(true).Key;
            int newX = playerX;
            int newY = playerY;
            switch (key)
            {
                case ConsoleKey.W: newY--; break;
                case ConsoleKey.S: newY++; break;
                case ConsoleKey.A: newX--; break;
                case ConsoleKey.D: newX++; break;
                default: continue;
            }
            if (newX < 0 || newX >= 10 || newY < 0 || newY >= 10) continue;
            playerX = newX;
            playerY = newY;
            turns++;
            if (playerX == monsterX && playerY == monsterY)
            {
                DrawMap(playerX, playerY, monsterX, monsterY, turns);
                Console.WriteLine("Монстр поймал игрока. Игра окончена.");
                return;
            }
            MoveMonster(ref monsterX, ref monsterY, playerX, playerY, random);
            if (playerX == monsterX && playerY == monsterY)
            {
                DrawMap(playerX, playerY, monsterX, monsterY, turns);
                Console.WriteLine("Монстр поймал игрока. Игра окончена.");
                return;
            }
        }
        DrawMap(playerX, playerY, monsterX, monsterY, turns);
        Console.WriteLine("Вы победили! Вы продержались 20 ходов.");
    }

    static void MoveMonster(ref int monsterX, ref int monsterY,
        int playerX, int playerY, Random random)
    {
        bool horizontal = monsterX != playerX;
        bool vertical = monsterY != playerY;
        if (horizontal && vertical) horizontal = random.Next(2) == 0;
        if (horizontal)
            monsterX += playerX > monsterX ? 1 : -1;
        else if (vertical)
            monsterY += playerY > monsterY ? 1 : -1;
    }

    static void DrawMap(int playerX, int playerY, int monsterX, int monsterY, int turns)
    {
        Console.Clear();
        Console.WriteLine("W, S, A, D - движение");
        Console.WriteLine($"Ходов: {turns} из 20");
        for (int y = 0; y < 10; y++)
        {
            for (int x = 0; x < 10; x++)
            {
                if (x == playerX && y == playerY) Console.Write('P');
                else if (x == monsterX && y == monsterY) Console.Write('M');
                else Console.Write('.');
            }
            Console.WriteLine();
        }
    }
}
