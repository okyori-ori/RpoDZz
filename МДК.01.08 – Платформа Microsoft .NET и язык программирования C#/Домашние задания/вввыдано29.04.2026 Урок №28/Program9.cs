using System;
using System.Threading; 

interface Attackable
{
    void TakeDamage(int damage); 
    bool IsAlive();         
}

interface Attacker
{
    void Attack(Attackable target); 
}


class Warrior : Attackable, Attacker
{

    public int Health;    
    public int Strength;  

    public Warrior(int health, int strength)
    {
        Health = health;
        Strength = strength;
    }

    public void Attack(Attackable target)
    {
        Console.WriteLine($"ВОИН атакует и наносит {Strength} урона!");
        target.TakeDamage(Strength);  
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;
        Console.WriteLine($"   Воин получил {damage} урона. Осталось здоровья: {Health}");

        if (Health <= 0)
        {
            Console.WriteLine("   ВОИН ПОВЕРЖЕН!  ");
        }
    }

    public bool IsAlive()
    {
        return Health > 0;
    }

    public void ShowInfo()
    {
        Console.WriteLine($" ВОИН | Здоровье: {Health} | Сила: {Strength}");
    }
}

class Mage : Attackable, Attacker
{

    public int Health;    
    public int Mana;       
    public int SpellPower;

    public Mage(int health, int mana, int spellPower)
    {
        Health = health;
        Mana = mana;
        SpellPower = spellPower;
    }

    public void Attack(Attackable target)
    {
        if (Mana >= 10)
        {
            Console.WriteLine($" МАГ использует заклинание и наносит {SpellPower} урона!");
            target.TakeDamage(SpellPower);
            Mana -= 10;
            Console.WriteLine($"   У мага осталось маны: {Mana}");
        }
        else
        {
            Console.WriteLine($" МАГ атакует посохом и наносит 1 урона!");
            target.TakeDamage(1);
        }
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;
        Console.WriteLine($"   Маг получил {damage} урона. Осталось здоровья: {Health}");

        if (Health <= 0)
        {
            Console.WriteLine("    МАГ ПОВЕРЖЕН!  ");
        }
    }

    public bool IsAlive()
    {
        return Health > 0;
    }

    public void ShowInfo()
    {
        Console.WriteLine($" МАГ | Здоровье: {Health} | Мана: {Mana} | Сила заклинания: {SpellPower}");
    }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("=== БОЙ МЕЖДУ ВОИНОМ И МАГОМ ===\n");

        Warrior warrior = new Warrior(100, 20);

        Mage mage = new Mage(80, 50, 30);

        Console.WriteLine("--- НАЧАЛЬНЫЕ ХАРАКТЕРИСТИКИ ---");
        warrior.ShowInfo();
        mage.ShowInfo();

        Console.WriteLine("\n--- НАЧИНАЕТСЯ БОЙ! ---\n");


        int turn = 1;

        while (warrior.IsAlive() && mage.IsAlive())
        {
            Console.WriteLine($"\n===== ХОД {turn} =====");

            Console.WriteLine("\n АТАКА ВОИНА:");
            warrior.Attack(mage);

            if (!mage.IsAlive())
            {
                Console.WriteLine("\n ВОИН ПОБЕДИЛ! ");
                break;
            }


            Console.WriteLine("\n АТАКА МАГА:");
            mage.Attack(warrior);

            if (!warrior.IsAlive())
            {
                Console.WriteLine("\n МАГ ПОБЕДИЛ! ");
                break;
            }

            Console.WriteLine("\n--- ТЕКУЩЕЕ СОСТОЯНИЕ ---");
            warrior.ShowInfo();
            mage.ShowInfo();

            turn++;

            Thread.Sleep(1000);
        }

 
        Console.WriteLine("\n=== ИТОГ БИТВЫ ===");
        if (warrior.IsAlive())
        {
            Console.WriteLine($" ПОБЕДИТЕЛЬ: ВОИН! Осталось здоровья: {warrior.Health}");
        }
        else if (mage.IsAlive())
        {
            Console.WriteLine($" ПОБЕДИТЕЛЬ: МАГ! Осталось здоровья: {mage.Health}");
        }

        Console.WriteLine("\nНажмите Enter, чтобы выйти...");
        Console.ReadLine();
    }
}