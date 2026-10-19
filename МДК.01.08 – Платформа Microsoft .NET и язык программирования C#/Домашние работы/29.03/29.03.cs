using System;

struct Book
{
    public string Title { get; set; }
    public string Author { get; set; }

    public Book(string title, string author)
    {
        Title = title;
        Author = author;
    }
}

class Student
{
    public string Name { get; set; }
    public Book FavoriteBook { get; set; }
    public static int Count { get; private set; }

    public Student(string name, Book favoriteBook)
    {
        Name = name;
        FavoriteBook = favoriteBook;
        Count++;
    }
}

class Program
{
    static void Main()
    {
        Console.InputEncoding = System.Text.Encoding.UTF8;
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("--- Система учета студентов ---");
        Console.WriteLine($"Начальное количество студентов в системе: {Student.Count}");
        Student student1 = new Student("Иван", new Book("Хоббит", "Дж. Р. Р. Толкин"));
        Console.WriteLine($"Создан студент {student1.Name}.");
        Console.WriteLine($"Текущее количество студентов в системе: {Student.Count}");
        Student student2 = new Student("Анна", new Book("Маленький принц", "Антуан де Сент-Экзюпери"));
        Console.WriteLine($"Создан студент {student2.Name}.");
        Console.WriteLine($"Текущее количество студентов в системе: {Student.Count}");

        Console.WriteLine("--- Эксперимент с копированием ---");
        Console.WriteLine($"Оригинальный студент: {student1.Name}");
        Console.WriteLine($"Любимая книга: {student1.FavoriteBook.Title}, автор: {student1.FavoriteBook.Author}");
        Student studentCopy = student1;
        Book bookCopy = student1.FavoriteBook;
        studentCopy.Name = "Петр";
        bookCopy.Title = "Властелин Колец";

        Console.WriteLine("Результат после изменений:");
        Console.WriteLine($"Имя оригинального студента: {student1.Name}");
        Console.WriteLine($"Любимая книга оригинального студента: {student1.FavoriteBook.Title}");
        Console.WriteLine($"Название копии книги: {bookCopy.Title}");
        Console.WriteLine($"Количество студентов после копирования: {Student.Count}");
        Console.WriteLine("Имя изменилось: обе переменные ссылаются на один объект Student.");
        Console.WriteLine("Книга не изменилась: структура Book скопирована по значению.");
    }
}
