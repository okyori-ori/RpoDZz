#include <iostream>
#include <numeric>

using namespace std;

class Fraction {
private:
    int numerator;
    int denominator;

public:
    Fraction() {
        numerator = 0;
        denominator = 1;
    }

    Fraction(int num, int den) {
        numerator = num;
        if (den == 0) {
            cout << "Ошибка: знаменатель не может быть равен 0! Установлено значение 1." << endl;
            denominator = 1;
        } else {
            denominator = den;
        }
    }

    void print() {
        cout << numerator << "/" << denominator << endl;
    }

    void simplify() {
        int common_divisor = std::gcd(numerator, denominator);
        numerator /= common_divisor;
        denominator /= common_divisor;
        if (denominator < 0) {
            numerator = -numerator;
            denominator = -denominator;
        }
    }

    Fraction operator+(const Fraction& other) {
        int num = numerator * other.denominator + denominator * other.numerator;
        int den = denominator * other.denominator;
        Fraction result(num, den);
        result.simplify();
        return result;
    }

    Fraction operator-(const Fraction& other) {
        int num = numerator * other.denominator - denominator * other.numerator;
        int den = denominator * other.denominator;
        Fraction result(num, den);
        result.simplify();
        return result;
    }

    Fraction operator*(const Fraction& other) {
        int num = numerator * other.numerator;
        int den = denominator * other.denominator;
        Fraction result(num, den);
        result.simplify();
        return result;
    }

    Fraction operator/(const Fraction& other) {
        if (other.numerator == 0) {
            cout << "Ошибка: деление на ноль!" << endl;
            return Fraction(0, 1);
        }
        int num = numerator * other.denominator;
        int den = denominator * other.numerator;
        Fraction result(num, den);
        result.simplify();
        return result;
    }

    bool operator==(const Fraction& other) {
        return numerator * other.denominator == denominator * other.numerator;
    }

    friend ostream& operator<<(ostream& os, const Fraction& f) {
        os << f.numerator << "/" << f.denominator;
        return os;
    }
};

int main() {
    setlocale(LC_ALL, "Russian");

    Fraction f1(3, 4);
    Fraction f2(2, 5);

    cout << "Дробь 1: ";
    f1.print();
    cout << "Дробь 2: ";
    f2.print();

    Fraction sum = f1 + f2;
    cout << "Сложение: " << sum << endl;

    Fraction diff = f1 - f2;
    cout << "Вычитание: " << diff << endl;

    Fraction mult = f1 * f2;
    cout << "Умножение: " << mult << endl;

    Fraction div = f1 / f2;
    cout << "Деление: " << div << endl;

    Fraction f3(6, 8);
    cout << "Дробь 3 до сокращения: " << f3 << endl;
    f3.simplify();
    cout << "Дробь 3 после сокращения: " << f3 << endl;

    if (f1 == f3) {
        cout << "Дробь 1 равна Дроби 3" << endl;
    } else {
        cout << "Дроби не равны" << endl;
    }

    return 0;
}