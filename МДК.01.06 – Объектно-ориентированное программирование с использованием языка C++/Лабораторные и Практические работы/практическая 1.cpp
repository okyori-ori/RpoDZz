#include <iostream>
#include <string>
#include <iomanip>

using namespace std;


// ЗАДАНИЕ 1

class Student {
private:
    string name;
    int age;
    double grade;

public:
  
    Student(string name, int age, double grade) {
        this->name = name;
        this->age = age;
        this->grade = grade;
    }

    ~Student() {
        cout << "[Destructor]: Student destroyed (" << this->name << ")\n";
    }

    void setData(string name, int age, double grade) {
        this->name = name;
        this->age = age;
        this->grade = grade;
    }

    void printInfo() const {
        cout << "--- Информация о студенте ---\n"
             << "Имя: " << name << "\n"
             << "Возраст: " << age << "\n"
             << "Средний балл: " << fixed << setprecision(1) << grade << "\n"
             << "-----------------------------\n";
    }
};

// ЗАДАНИЕ 2. КЛАСС CAR
class Car {
public:
    string brand;

private:
    int year;

protected:
    int speed;

public:

    Car(string brand, int year, int speed) {
        this->brand = brand;
        this->year = year;
        this->speed = speed;
    }

    void setYear(int y) {
        this->year = y;
    }
    int getYear() const {
        return this->year;
    }

    void setSpeed(int s) {
        this->speed = s;
    }
    int getSpeed() const {
        return this->speed;
    }

    void printInfo() const {
        cout << "--- Характеристики авто ---\n"
             << "Марка: " << brand << "\n"
             << "Год выпуска: " << year << "\n"
             << "Скорость: " << speed << " км/ч\n"
             << "---------------------------\n";
    }
};

// ЗАДАНИЕ 3. КЛАСС PRODUCT
class Product {
private:
    string name;
    double price;
    int quantity;

public:
    Product(string name, double price, int quantity) {
        this->name = name;
        this->price = price;
        this->quantity = quantity;
    }

    ~Product() {
        cout << "[Destructor]: Product destroyed (" << this->name << ")\n";
    }

    Product& setData(string name, double price, int quantity) {
        this->name = name;
        this->price = price;
        this->quantity = quantity;
        return *this; 
    }

    void buy(int amount) {
        if (amount <= 0) {
            cout << "Ошибка: Количество для покупки должно быть больше 0.\n";
            return;
        }

        if (this->quantity >= amount) {
            this->quantity -= amount;
            cout << "Успешно куплено " << amount << " шт. товара \"" << this->name << "\".\n";
        } else {
            cout << "Ошибка: Недостаточно товара \"" << this->name << "\" на складе! "
                 << "Доступно: " << this->quantity << " шт.\n";
        }
    }

    void printInfo() const {
        cout << "--- Данные товара ---\n"
             << "Название: " << name << "\n"
             << "Цена: " << fixed << setprecision(2) << price << " руб.\n"
             << "Количество: " << quantity << " шт.\n"
             << "---------------------\n";
    }
};

int main() {
    setlocale(LC_ALL, "Russian");

    cout << "ТЕСТИРОВАНИЕ ЗАДАНИЯ 1: Студенты\n";
    {
        Student s1("Alex", 20, 4.5);
        s1.printInfo();

        cout << "Выполняем поверхностное копирование (s2 = s1)...\n";
        Student s2 = s1; 
        s2.printInfo();

        cout << "Изменяем данные у s1 через setData...\n";
        s1.setData("Maxim", 21, 4.8);
        cout << "s1 после изменений:\n";
        s1.printInfo();
        cout << "s2 остался прежним:\n";
        s2.printInfo();

    cout << "ТЕСТИРОВАНИЕ ЗАДАНИЯ 2: Автомобили\n";
    {
        Car c1("Toyota", 2020, 180);
        c1.printInfo();

        cout << "Изменяем год на 2021 и скорость на 200 через сеттеры...\n";
        c1.setYear(2021);
        c1.setSpeed(200);
        c1.printInfo();

        cout << "Проверка работы геттеров:\n";
        cout << "Бренд (public): " << c1.brand << "\n";
        cout << "Год (через геттер): " << c1.getYear() << "\n";
        cout << "Скорость (через геттер): " << c1.getSpeed() << "\n";
    }

    cout << "ТЕСТИРОВАНИЕ ЗАДАНИЯ 3: Товары\n";
    {
        Product p1("Laptop", 1500.0, 10);
        p1.printInfo();

        p1.buy(3);
        p1.printInfo();

        cout << "Пытаемся купить больше, чем есть на складе:\n";
        p1.buy(12); 

        cout << "\nТестирование цепочки методов благодаря возврату *this в setData:\n";
        p1.setData("Smartphone", 800.0, 5).printInfo();
    } // Здесь вызовется деструктор для p1

    return 0;
}