using System;

class Rectangle
{
    public double width;
    public double height;

    public double GetArea()
    {
        return width * height;
    }

    public double GetPerimeter()
    {
        return 2 * (width + height);
    }
}

class Program
{
    static void Main()
    {
        Console.InputEncoding = System.Text.Encoding.UTF8;
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Rectangle first = new Rectangle();
        first.width = 10;
        first.height = 5;
        double firstArea = first.GetArea();
        double firstPerimeter = first.GetPerimeter();

        Rectangle second = new Rectangle();
        second.width = 7.5;
        second.height = 3.2;
        double secondArea = second.GetArea();
        double secondPerimeter = second.GetPerimeter();

        Console.WriteLine("Прямоугольник 1:");
        Console.WriteLine($"Ширина: {first.width}");
        Console.WriteLine($"Высота: {first.height}");
        Console.WriteLine($"Площадь: {firstArea}");
        Console.WriteLine($"Периметр: {firstPerimeter}");
        Console.WriteLine();
        Console.WriteLine("Прямоугольник 2:");
        Console.WriteLine($"Ширина: {second.width}");
        Console.WriteLine($"Высота: {second.height}");
        Console.WriteLine($"Площадь: {secondArea}");
        Console.WriteLine($"Периметр: {secondPerimeter}");
    }
}
