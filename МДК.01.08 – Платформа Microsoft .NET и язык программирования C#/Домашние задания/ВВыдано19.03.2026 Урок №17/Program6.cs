using System;


struct Book
{

    public string Title;    
    public string Author;  

    public Book(string title, string author)
    {
        Title = title;
        Author = author;
    }

    public void PrintInfo()
    {
        Console.Write($"\"{Title}\" автора {Author}");
    }
}


class Student
{

    private static int totalStudents = 0;

    public string Name;        
    public Book FavoriteBook;  

    public static int TotalStudents
    {
        get { return totalStudents; }
    }

    public Student(string name, Book favoriteBook)
    {
        Name = name;
        FavoriteBook = favoriteBook;

        totalStudents++;
    }

    public void PrintInfo()
    {
        Console.Write($"Студент {Name}, любимая книга ");
        FavoriteBook.PrintInfo();
    }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("--- Система учета студентов ---\n");

        Console.WriteLine($"Начальное количество студентов в системе: {Student.TotalStudents}");
        Console.WriteLine();

        Book book1 = new Book("Хоббит", "Дж. Р. Р. Толкин");
        Student student1 = new Student("Иван", book1);
        Console.WriteLine($"Создан студент {student1.Name}.");
        Console.WriteLine($"Текущее количество студентов в системе: {Student.TotalStudents}");
        Console.WriteLine();

        Book book2 = new Book("Мастер и Маргарита", "Михаил Булгаков");
        Student student2 = new Student("Анна", book2);
        Console.WriteLine($"Создан студент {student2.Name}.");
        Console.WriteLine($"Текущее количество студентов в системе: {Student.TotalStudents}");
        Console.WriteLine();

        Console.WriteLine("--- Эксперимент с копированием ---\n");

        Console.Write("Оригинальный студент: ");
        student1.PrintInfo();
        Console.WriteLine("\n");

        Student studentCopy = student1;

        Book bookCopy = student1.FavoriteBook;

        Console.WriteLine("...Копируем данные и вносим изменения...");

        studentCopy.Name = "Петр";

        bookCopy.Title = "Властелин Колец";

        Console.WriteLine("Изменяем имя у копии студента на 'Петр'.");
        Console.WriteLine("Изменяем название у копии книги на 'Властелин Колец'.\n");

        Console.WriteLine("--- Результат после изменений ---");

        Console.Write($"Имя оригинального студента (student1): {student1.Name}");
        Console.Write($"\nНазвание любимой книги оригинального студента (student1.FavoriteBook): {student1.FavoriteBook.Title}");

        Console.WriteLine("\n\n--- ВЫВОД ---");
        Console.WriteLine("Имя студента ИЗМЕНИЛОСЬ, так как классы копируются по ССЫЛКЕ.");
        Console.WriteLine("Книга НЕ ИЗМЕНИЛАСЬ, так как структуры копируются по ЗНАЧЕНИЮ.");

        Console.WriteLine("\n\nНажмите Enter, чтобы выйти...");
        Console.ReadLine();
    }
}