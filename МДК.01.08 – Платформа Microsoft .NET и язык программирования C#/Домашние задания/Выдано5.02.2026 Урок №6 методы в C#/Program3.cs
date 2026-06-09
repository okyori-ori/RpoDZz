using System;

class Program
{
    static void Main(string[] args)
    {
   
        string[] пользователи = { "Анна", "Иван", "Мария", "Дмитрий", "Ольга" };

        int выбор = 0;

        while (выбор != 3)
        {
        
            Console.WriteLine("\n=== МЕНЮ ===");
            Console.WriteLine("1 - Показать всех пользователей");
            Console.WriteLine("2 - Редактировать имя пользователя");
            Console.WriteLine("3 - Выйти из программы");
            Console.Write("Ваш выбор: ");

            string ввод = Console.ReadLine();


            if (ввод == "1")
            {
                ПоказатьПользователей(пользователи);
            }
            else if (ввод == "2")
            {
                РедактироватьПользователя(пользователи);
            }
            else if (ввод == "3")
            {
                Console.WriteLine("До свидания!");
                выбор = 3;
            }
            else
            {
                Console.WriteLine("Ошибка! Выберите 1, 2 или 3");
            }
        }
    }


    static void ПоказатьПользователей(string[] пользователи)
    {
        Console.WriteLine("\nСписок пользователей:");

        for (int i = 0; i < пользователи.Length; i++)
        {
            Console.WriteLine($"{i + 1}. {пользователи[i]}");
        }
    }

  
    static void РедактироватьПользователя(string[] пользователи)
    {
        Console.WriteLine("\n--- РЕДАКТИРОВАНИЕ ПОЛЬЗОВАТЕЛЯ ---");

       
        for (int i = 0; i < пользователи.Length; i++)
        {
            Console.WriteLine($"{i + 1}. {пользователи[i]}");
        }


        Console.Write("\nВведите номер пользователя для редактирования: ");
        string ввод = Console.ReadLine();

        int номер = Convert.ToInt32(ввод);

        if (номер >= 1 && номер <= пользователи.Length)
        {
            int индекс = номер - 1; 

            Console.WriteLine($"Текущее имя: {пользователи[индекс]}");
            Console.Write("Введите новое имя: ");

            string новоеИмя = Console.ReadLine();

            if (новоеИмя != "")
            {
                пользователи[индекс] = новоеИмя;
                Console.WriteLine($"✓ Готово! Пользователь {номер} теперь называется {новоеИмя}");
            }
            else
            {
                Console.WriteLine("Ошибка: имя не может быть пустым!");
            }
        }
        else
        {
            Console.WriteLine("Ошибка: такого номера нет!");
        }

        Console.WriteLine("\nНажмите Enter, чтобы продолжить...");
        Console.ReadLine();
    }
}