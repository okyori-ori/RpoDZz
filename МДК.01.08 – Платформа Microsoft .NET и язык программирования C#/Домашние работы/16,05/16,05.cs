using System;
using System.Collections.Generic;
using System.Linq;

class Student
{
    public string FullName { get; set; }
    public int Age { get; set; }
    public double AverageGrade { get; set; }
}

class Program
{
    static void Main()
    {
        Console.InputEncoding = System.Text.Encoding.UTF8;
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        List<Student> students = new List<Student>
        {
            new Student { FullName = "Петров Иван", Age = 20, AverageGrade = 85.5 },
            new Student { FullName = "Сидорова Анна", Age = 23, AverageGrade = 78.1 },
            new Student { FullName = "Кузнецов Олег", Age = 19, AverageGrade = 74.9 },
            new Student { FullName = "Васильева Мария", Age = 26, AverageGrade = 92.0 },
            new Student { FullName = "Смирнов Алексей", Age = 22, AverageGrade = 95.2 }
        };

        var goodStudents = students.Where(s => s.AverageGrade >= 75 && s.AverageGrade <= 90);
        Console.WriteLine("--- Список студентов-хорошистов (балл от 75 до 90) ---");
        foreach (Student student in goodStudents)
            Console.WriteLine($"{student.FullName} - {student.AverageGrade}");

        var names = students.Select(s => s.FullName);
        Console.WriteLine("--- Все студенты ---");
        foreach (string name in names)
            Console.WriteLine(name);

        var byAge = students.OrderBy(s => s.Age);
        Console.WriteLine("--- Сортировка по возрасту ---");
        foreach (Student student in byAge)
            Console.WriteLine($"{student.FullName} - {student.Age} лет");

        var rating = students.Where(s => s.Age < 25)
            .OrderByDescending(s => s.AverageGrade)
            .Select(s => $"{s.FullName} - {s.AverageGrade}");
        Console.WriteLine("--- Рейтинг студентов младше 25 лет ---");
        foreach (string line in rating)
            Console.WriteLine(line);
    }
}
