using System;

interface Attackable
{
    void takeDamage(int damage);
    bool isAlive();
}

interface Attacker
{
    void attack(Attackable target);
}

class Warrior : Attackable, Attacker
{
    public int health;
    public int strength;

    public Warrior(int health, int strength)
    {
        this.health = health;
        this.strength = strength;
    }

    public void takeDamage(int damage)
    {
        health -= damage;
        if (health < 0) health = 0;
    }

    public bool isAlive()
    {
        return health > 0;
    }

    public void attack(Attackable target)
    {
        target.takeDamage(strength);
        Console.WriteLine($"Воин наносит {strength} урона.");
    }
}

class Mage : Attackable, Attacker
{
    public int health;
    public int mana;
    public int spellPower;

    public Mage(int health, int mana, int spellPower)
    {
        this.health = health;
        this.mana = mana;
        this.spellPower = spellPower;
    }

    public void takeDamage(int damage)
    {
        health -= damage;
        if (health < 0) health = 0;
    }

    public bool isAlive()
    {
        return health > 0;
    }

    public void attack(Attackable target)
    {
        int damage = 1;
        if (mana >= 10)
        {
            damage = spellPower;
            mana -= 10;
        }
        target.takeDamage(damage);
        Console.WriteLine($"Маг наносит {damage} урона. Мана: {mana}");
    }
}

class Program
{
    static void Main()
    {
        Console.InputEncoding = System.Text.Encoding.UTF8;
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Warrior warrior = new Warrior(100, 15);
        Mage mage = new Mage(80, 20, 25);
        int round = 1;
        while (warrior.isAlive() && mage.isAlive())
        {
            Console.WriteLine($"Раунд {round}");
            warrior.attack(mage);
            if (mage.isAlive()) mage.attack(warrior);
            Console.WriteLine($"Здоровье воина: {warrior.health}");
            Console.WriteLine($"Здоровье мага: {mage.health}");
            round++;
        }
        Console.WriteLine(warrior.isAlive() ? "Победил воин." : "Победил маг.");
    }
}
