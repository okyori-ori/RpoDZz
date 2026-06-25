#include <iostream>
#include <fstream>
#include <string>
#include <vector>

class LogEntry {
protected:
    std::string message;
public:
    LogEntry(const std::string& msg) : message(msg) {}
    virtual void print() = 0;
    virtual std::string getType() = 0;
    virtual ~LogEntry() {}
};

class InfoLog : public LogEntry {
public:
    InfoLog(const std::string& msg) : LogEntry(msg) {}
    void print() override {
        std::cout << "INFO: " << message << std::endl;
    }
    std::string getType() override {
        return "INFO";
    }
};

class WarningLog : public LogEntry {
public:
    WarningLog(const std::string& msg) : LogEntry(msg) {}
    void print() override {
        std::cout << "WARNING: " << message << std::endl;
    }
    std::string getType() override {
        return "WARNING";
    }
};

class ErrorLog : public LogEntry {
public:
    ErrorLog(const std::string& msg) : LogEntry(msg) {}
    void print() override {
        std::cout << "ERROR: " << message << std::endl;
    }
    std::string getType() override {
        return "ERROR";
    }
};

int main() {
    std::ifstream file("log.txt");
    if (!file.is_open()) {
        return 1;
    }

    std::vector<LogEntry*> logs;
    std::string line;

    while (std::getline(file, line)) {
        if (line.rfind("INFO: ", 0) == 0) {
            logs.push_back(new InfoLog(line.substr(6)));
        } else if (line.rfind("WARNING: ", 0) == 0) {
            logs.push_back(new WarningLog(line.substr(9)));
        } else if (line.rfind("ERROR: ", 0) == 0) {
            logs.push_back(new ErrorLog(line.substr(7)));
        }
    }
    file.close();

    int infoCount = 0;
    int warningCount = 0;
    int errorCount = 0;

    for (LogEntry* log : logs) {
        std::string type = log->getType();
        if (type == "INFO") infoCount++;
        else if (type == "WARNING") warningCount++;
        else if (type == "ERROR") errorCount++;
    }

    std::cout << "Статистика:\n";
    std::cout << "INFO: " << infoCount << "\n";
    std::cout << "WARNING: " << warningCount << "\n";
    std::cout << "ERROR: " << errorCount << "\n\n";

    std::cout << "Ошибки:\n";
    for (LogEntry* log : logs) {
        if (log->getType() == "ERROR") {
            log->print();
        }
    }

    for (LogEntry* log : logs) {
        delete log;
    }
    logs.clear();

    return 0;
}