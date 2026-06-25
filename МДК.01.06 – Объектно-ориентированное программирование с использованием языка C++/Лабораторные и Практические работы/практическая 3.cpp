#include <iostream>
#include <vector>
#include <string>
#include <algorithm>

using namespace std;

class Track {
protected:
    string title;
    int duration;
public:
    Track(string title, int duration) {
        this->title = title;
        this->duration = duration;
    }
    virtual void play() {
        cout << "Играет трек: " << title << endl;
    }
    int getDuration() const {
        return duration;
    }
    virtual ~Track() {}
};

class Song : public Track {
private:
    string artist;
public:
    Song(string title, int duration, string artist) : Track(title, duration) {
        this->artist = artist;
    }
    void play() override {
        cout << "Играет песня: " << title << " " << artist << endl;
    }
};

class Instrumental : public Track {
private:
    string instrument;
public:
    Instrumental(string title, int duration, string instrument) : Track(title, duration) {
        this->instrument = instrument;
    }
    void play() override {
        cout << "Играет инструментал: " << title << " (" << instrument << ")" << endl;
    }
};

class Podcast : public Track {
private:
    string host;
public:
    Podcast(string title, int duration, string host) : Track(title, duration) {
        this->host = host;
    }
    void play() override {
        cout << "Играет подкаст: " << title << " (Ведущий: " << host << ")" << endl;
    }
};

bool compareTracks(Track* a, Track* b) {
    return a->getDuration() < b->getDuration();
}

int main() {
    vector<Track*> catalog;
    int choice = -1;

    while (choice != 0) {
        cout << "\n1. Добавить песню\n2. Добавить инструментал\n3. Добавить подкаст\n4. Показать каталог (play)\n5. Сортировать по длительности\n0. Выход\nВыбор: ";
        cin >> choice;
        cin.ignore();

        if (choice == 1) {
            string title, artist;
            int duration;
            cout << "Название: "; getline(cin, title);
            cout << "Длительность (сек): "; cin >> duration; cin.ignore();
            cout << "Исполнитель: "; getline(cin, artist);
            catalog.push_back(new Song(title, duration, artist));
        } 
        else if (choice == 2) {
            string title, instrument;
            int duration;
            cout << "Название: "; getline(cin, title);
            cout << "Длительность (сек): "; cin >> duration; cin.ignore();
            cout << "Инструмент: "; getline(cin, instrument);
            catalog.push_back(new Instrumental(title, duration, instrument));
        } 
        else if (choice == 3) {
            string title, host;
            int duration;
            cout << "Название: "; getline(cin, title);
            cout << "Длительность (сек): "; cin >> duration; cin.ignore();
            cout << "Ведущий: "; getline(cin, host);
            catalog.push_back(new Podcast(title, duration, host));
        } 
        else if (choice == 4) {
            for (Track* track : catalog) {
                track->play();
            }
        } 
        else if (choice == 5) {
            sort(catalog.begin(), catalog.end(), compareTracks);
            cout << "Каталог отсортирован.\n";
        }
    }

    for (Track* track : catalog) {
        delete track;
    }
    catalog.clear();

    return 0;
}