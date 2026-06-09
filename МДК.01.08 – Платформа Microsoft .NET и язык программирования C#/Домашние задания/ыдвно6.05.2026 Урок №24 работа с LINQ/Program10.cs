using System;
using System.Collections.Generic;
using System.Linq;


class Student
{

    public string FirstName { get; set; }   
    public string LastName { get; set; }  
    public int Age { get; set; }         
    public double AverageScore { get; set; } 

    public string FullName
    {
        get { return $"{LastName} {FirstName}"; }
    }

    public Student(string firstName, string lastName, int age, double averageScore)
    {
        FirstName = firstName;
        LastName = lastName;
        Age = age;
        AverageScore = averageScore;
    }

    public void PrintInfo()
    {
        Console.WriteLine($"{FullName} - {Age} лет, средний балл: {AverageScore}");
    }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("=== АНАЛИЗ СПИСКА СТУДЕНТОВ ===\n");

        List<Student> students = new List<Student>
        {
            new Student("Иван", "Петров", 20, 85.5),
            new Student("Анна", "Сидорова", 23, 78.1),
            new Student("Олег", "Кузнецов", 19, 74.9),
            new Student("Мария", "Васильева", 26, 92.3),
            new Student("Алексей", "Смирнов", 22, 95.2),
            new Student("Екатерина", "Михайлова", 21, 68.4),
            new Student("Дмитрий", "Фёдоров", 24, 88.7)
        };

        Console.WriteLine("--- Список студентов-хорошистов (балл от 75 до 90) ---");

        var goodStudents = from student in students
                           where student.AverageScore >= 75 && student.AverageScore <= 90
                           select student;

        foreach (var student in goodStudents)
        {
            Console.WriteLine($"{student.FullName} - {student.AverageScore}");
        }

        if (!goodStudents.Any())
        {
            Console.WriteLine("Студентов с таким баллом не найдено.");
        }

        Console.WriteLine();

        Console.WriteLine("--- Все студенты (только имена) ---");

        var namesOnly = from student in students
                        select student.FullName;

        foreach (string name in namesOnly)
        {
            Console.WriteLine(name);
        }

        Console.WriteLine();

        Console.WriteLine("--- Сортировка по возрасту (от младшего к старшему) ---");

        var sortedByAge = from student in students
                          orderby student.Age
                          select student;

        foreach (var student in sortedByAge)
        {

            string ageWord = GetAgeWord(student.Age);
            Console.WriteLine($"{student.FullName} - {student.Age} {ageWord}");
        }

        Console.WriteLine();

        Console.WriteLine("--- Рейтинг студентов младше 25 лет (по убыванию балла) ---");

        var rating = students
            .Where(s => s.Age < 25)                       
            .OrderByDescending(s => s.AverageScore)         
            .Select(s => $"{s.FullName} - {s.AverageScore}"); 

        foreach (string item in rating)
        {
            Console.WriteLine(item);
        }

        Console.WriteLine("\n(Тот же рейтинг, но другим способом записи)");

        var ratingAlt = from student in students
                        where student.Age < 25
                        orderby student.AverageScore descending
                        select $"{student.FullName} - {student.AverageScore}";

        foreach (string item in ratingAlt)
        {
            Console.WriteLine(item);
        }

        Console.WriteLine("\n\nНажмите Enter, чтобы выйти...");
        Console.ReadLine();
    }

    static string GetAgeWord(int age)
    {
        int lastDigit = age % 10;
        int lastTwoDigits = age % 100;

        if (lastTwoDigits >= 11 && lastTwoDigits <= 14)
        {
            return "лет";
        }

        switch (lastDigit)
        {
            case 1: return "год";
            case 2:
            case 3:
            case 4: return "года";
            default: return "лет";
        }
    }
}