#include <iostream>
#include <fstream>
#include <string>

bool isInfo(const std::string& line) {
    return line.rfind("INFO:", 0) == 0;
}

bool isWarning(const std::string& line) {
    return line.rfind("WARNING:", 0) == 0;
}

bool isError(const std::string& line) {
    return line.rfind("ERROR:", 0) == 0;
}

int main() {
    std::ifstream file("log.txt");
    if (!file.is_open()) {
        return 1;
    }

    std::string line;
    int infoCount = 0;
    int warningCount = 0;
    int errorCount = 0;
    std::string errorMessages = "";

    while (std::getline(file, line)) {
        if (isInfo(line)) {
            infoCount++;
        } else if (isWarning(line)) {
            warningCount++;
        } else if (isError(line)) {
            errorCount++;
            errorMessages += line + "\n";
        }
    }
    file.close();

    std::cout << "Статистика:\n";
    std::cout << "INFO: " << infoCount << "\n";
    std::cout << "WARNING: " << warningCount << "\n";
    std::cout << "ERROR: " << errorCount << "\n\n";

    std::cout << "Ошибки:\n";
    std::cout << errorMessages;

    return 0;
}