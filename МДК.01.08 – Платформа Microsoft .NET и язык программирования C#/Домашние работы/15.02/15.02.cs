using System;

class Program
{
    static void Main()
    {
        Console.InputEncoding = System.Text.Encoding.UTF8;
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        string[] users = new string[20];
        while (true)
        {
            Console.WriteLine("1. Добавить пользователя");
            Console.WriteLine("2. Удалить пользователя");
            Console.WriteLine("3. Изменить пользователя");
            Console.WriteLine("4. Показать пользователей");
            Console.WriteLine("0. Выход");
            string action = Console.ReadLine();
            if (action == null || action == "0") return;
            switch (action)
            {
                case "1": users = AddUser(users); break;
                case "2": users = RemoveUser(users); break;
                case "3": ChangeUser(users); break;
                case "4": ShowUsers(users); break;
                default: Console.WriteLine("Неверная команда."); break;
            }
        }
    }

    static string[] AddUser(string[] users)
    {
        Console.Write("Введите имя нового пользователя: ");
        string name = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("Имя не может быть пустым.");
            return users;
        }
        for (int i = 0; i < users.Length; i++)
        {
            if (string.IsNullOrEmpty(users[i]))
            {
                users[i] = name;
                Console.WriteLine("Пользователь добавлен.");
                return users;
            }
        }
        Console.WriteLine("Нет свободного места.");
        return users;
    }

    static string[] RemoveUser(string[] users)
    {
        ShowUsers(users);
        Console.Write("Введите имя удаляемого пользователя: ");
        string name = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("Имя не может быть пустым.");
            return users;
        }
        for (int i = 0; i < users.Length; i++)
        {
            if (users[i] == name)
            {
                users[i] = string.Empty;
                Console.WriteLine("Пользователь удалён.");
                return users;
            }
        }
        Console.WriteLine("Пользователь не найден.");
        return users;
    }

    static void ChangeUser(string[] users)
    {
        ShowUsers(users);
        Console.Write("Введите имя изменяемого пользователя: ");
        string oldName = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(oldName))
        {
            Console.WriteLine("Имя не может быть пустым.");
            return;
        }
        for (int i = 0; i < users.Length; i++)
        {
            if (users[i] == oldName)
            {
                Console.Write("Введите новое имя: ");
                string newName = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(newName))
                {
                    Console.WriteLine("Имя не может быть пустым.");
                    return;
                }
                users[i] = newName;
                Console.WriteLine("Имя изменено.");
                return;
            }
        }
        Console.WriteLine("Пользователь не найден.");
    }

    static void ShowUsers(string[] users)
    {
        bool found = false;
        for (int i = 0; i < users.Length; i++)
        {
            if (!string.IsNullOrEmpty(users[i]))
            {
                Console.WriteLine($"{i + 1}. {users[i]}");
                found = true;
            }
        }
        if (!found) Console.WriteLine("Список пуст.");
    }
}
