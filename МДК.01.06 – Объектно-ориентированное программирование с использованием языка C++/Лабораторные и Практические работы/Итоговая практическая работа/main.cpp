#include <iostream>
#include <vector>
#include <string>
#include <fstream>
#include <sstream>
#include <iomanip>

using namespace std;

struct Operation {
    int id;
    string type;
    string category;
    double amount;
    string date;
    string description;
};

int nextId = 1;
vector<Operation> operations;

void loadFromFile() {
    ifstream file("finance.txt");
    if (!file.is_open()) return;

    operations.clear();
    string line;
    int maxId = 0;

    while (getline(file, line)) {
        stringstream ss(line);
        string idStr, type, category, amountStr, date, description;

        if (getline(ss, idStr, '|') &&
            getline(ss, type, '|') &&
            getline(ss, category, '|') &&
            getline(ss, amountStr, '|') &&
            getline(ss, date, '|') &&
            getline(ss, description)) {

            Operation op;
            op.id = stoi(idStr);
            op.type = type;
            op.category = category;
            op.amount = stod(amountStr);
            op.date = date;
            op.description = description;

            operations.push_back(op);
            if (op.id > maxId) {
                maxId = op.id;
            }
        }
    }
    file.close();
    nextId = maxId + 1;
}

void saveToFile() {
    ofstream file("finance.txt");
    if (!file.is_open()) return;

    for (const auto& op : operations) {
        file << op.id << "|"
             << op.type << "|"
             << op.category << "|"
             << op.amount << "|"
             << op.date << "|"
             << op.description << "\n";
    }
    file.close();
}

void addOperation() {
    Operation op;
    op.id = nextId++;

    int typeChoice;
    while (true) {
        cout << "Выберите тип операции (1 - Доход, 2 - Расход): ";
        if (cin >> typeChoice && (typeChoice == 1 || typeChoice == 2)) {
            break;
        }
        cout << "Ошибка ввода! Введите 1 или 2.\n";
        cin.clear();
        cin.ignore(10000, '\n');
    }
    op.type = (typeChoice == 1) ? "Доход" : "Расход";

    cout << "Введите категорию: ";
    cin >> op.category;

    while (true) {
        cout << "Введите сумму: ";
        if (cin >> op.amount && op.amount > 0) {
            break;
        }
        cout << "Ошибка ввода! Сумма должна быть числом больше 0.\n";
        cin.clear();
        cin.ignore(10000, '\n');
    }

    cout << "Введите дату (ГГГГММДД): ";
    cin >> op.date;

    cout << "Введите описание (опционально): ";
    cin.ignore();
    getline(cin, op.description);
    if (op.description.empty()) {
        op.description = "-";
    }

    operations.push_back(op);
    saveToFile();
    cout << "Запись успешно добавлена! ID: " << op.id << "\n";
}

void printOperationHeader() {
    cout << "-------------------------------------------------------------------------\n";
    cout << left << setw(5) << "ID" 
         << setw(10) << "Тип" 
         << setw(15) << "Категория" 
         << setw(12) << "Сумма" 
         << setw(12) << "Дата" 
         << "Описание" << "\n";
    cout << "-------------------------------------------------------------------------\n";
}

void printOperationRow(const Operation& op) {
    cout << left << setw(5) << op.id 
         << setw(10) << op.type 
         << setw(15) << op.category 
         << setw(12) << op.amount 
         << setw(12) << op.date 
         << op.description << "\n";
}

void viewOperations() {
    if (operations.empty()) {
        cout << "Список операций пуст.\n";
        return;
    }

    cout << "\n--- Просмотр операций ---\n";
    cout << "1. Все операции\n";
    cout << "2. Только доходы\n";
    cout << "3. Только расходы\n";
    cout << "4. Фильтр по периоду (дате)\n";
    cout << "Выберите вариант: ";

    int choice;
    if (!(cin >> choice)) {
        cin.clear();
        cin.ignore(10000, '\n');
        cout << "Неверный выбор.\n";
        return;
    }

    if (choice == 1) {
        printOperationHeader();
        for (const auto& op : operations) {
            printOperationRow(op);
        }
    } 
    else if (choice == 2 || choice == 3) {
        string targetType = (choice == 2) ? "Доход" : "Расход";
        printOperationHeader();
        for (const auto& op : operations) {
            if (op.type == targetType) {
                printOperationRow(op);
            }
        }
    } 
    else if (choice == 4) {
        string startDate, endDate;
        cout << "Введите начальную дату (ГГГГММДД): ";
        cin >> startDate;
        cout << "Введите конечную дату (ГГГГММДД): ";
        cin >> endDate;

        printOperationHeader();
        for (const auto& op : operations) {
            if (op.date >= startDate && op.date <= endDate) {
                printOperationRow(op);
            }
        }
    } 
    else {
        cout << "Неверный вариант.\n";
    }
}

void deleteOperation() {
    if (operations.empty()) {
        cout << "Список операций пуст.\n";
        return;
    }

    int idToDelete;
    cout << "Введите ID записи для удаления: ";
    if (!(cin >> idToDelete)) {
        cin.clear();
        cin.ignore(10000, '\n');
        cout << "Ошибка ввода! ID должен быть числом.\n";
        return;
    }

    for (auto it = operations.begin(); it != operations.end(); ++it) {
        if (it->id == idToDelete) {
            operations.erase(it);
            saveToFile();
            cout << "Запись с ID " << idToDelete << " успешно удалена.\n";
            return;
        }
    }
    cout << "Запись с таким ID не найдена.\n";
}

void showStatistics() {
    if (operations.empty()) {
        cout << "Нет данных для расчета статистики.\n";
        return;
    }

    double totalIncomes = 0;
    double totalExpenses = 0;

    struct CategoryStat {
        string name;
        double sum;
    };
    vector<CategoryStat> catStats;

    for (const auto& op : operations) {
        if (op.type == "Доход") {
            totalIncomes += op.amount;
        } else {
            totalExpenses += op.amount;
        }

        bool found = false;
        for (auto& stat : catStats) {
            if (stat.name == op.category) {
                stat.sum += op.amount;
                found = true;
                break;
            }
        }
        if (!found) {
            catStats.push_back({op.category, op.amount});
        }
    }

    cout << "\n=== ФИНАНСОВАЯ СТАТИСТИКА ===\n";
    cout << "Общая сумма доходов:  " << totalIncomes << "\n";
    cout << "Общая сумма расходов: " << totalExpenses << "\n";
    cout << "Текущий баланс:       " << (totalIncomes - totalExpenses) << "\n";
    cout << "-----------------------------\n";
    cout << "Статистика по категориям:\n";
    for (const auto& stat : catStats) {
        cout << " - " << left << setw(15) << stat.name << ": " << stat.sum << "\n";
    }
    cout << "=============================\n";
}

int main() {
    loadFromFile();

    int choice = 0;
    while (true) {
        cout << "\n=== Учет личных финансов ===\n";
        cout << "1. Добавить запись\n";
        cout << "2. Просмотр операций\n";
        cout << "3. Удалить запись по ID\n";
        cout << "4. Финансовая статистика\n";
        cout << "5. Выйти из программы\n";
        cout << "Выберите пункт меню: ";

        if (!(cin >> choice)) {
            cout << "Ошибка ввода! Пожалуйста, выберите пункт из меню.\n";
            cin.clear();
            cin.ignore(10000, '\n');
            continue;
        }

        if (choice == 1) {
            addOperation();
        } else if (choice == 2) {
            viewOperations();
        } else if (choice == 3) {
            deleteOperation();
        } else if (choice == 4) {
            showStatistics();
        } else if (choice == 5) {
            cout << "Программа завершает работу. Данные сохранены.\n";
            break;
        } else {
            cout << "Неверный пункт меню. Попробуйте снова.\n";
        }
    }

    return 0;
}