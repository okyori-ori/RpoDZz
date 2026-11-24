using System;
using System.Drawing;
using System.Windows.Forms;

namespace Viktorina_WinForms
{
    // Form1 — это наше окно с викториной.
    public class Form1 : Form
    {
        // Создаём элементы, которые будут видны на экране.
        Label questionLabel = new Label();       // Текст вопроса.
        Label progressLabel = new Label();       // Надпись «1/5», «2/5» и так далее.
        GroupBox answersBox = new GroupBox();    // Рамка вокруг ответов.
        RadioButton[] choices = new RadioButton[4]; // Четыре кружочка для выбора ответа.
        ProgressBar progressBar = new ProgressBar(); // Полоска прогресса.
        Button nextButton = new Button();        // Кнопка «Далее» или «Результат».

        // Все 5 вопросов. Каждый вопрос записан в кавычках.
        string[] questions =
        {
            "Что такое Windows Forms?",
            "Какой элемент нужен для выбора одного ответа?",
            "Какой элемент объединяет другие элементы?",
            "Какой элемент показывает ход выполнения?",
            "Какой элемент показывает текст на форме?"
        };

        // Варианты ответов: каждая строка — ответы к одному вопросу.
        // Например, первая строка относится к первому вопросу.
        string[,] answers =
        {
            { "База данных", "Создание оконных приложений", "Браузер", "Операционная система" },
            { "Button", "Label", "RadioButton", "ProgressBar" },
            { "GroupBox", "Label", "Button", "TextBox" },
            { "RadioButton", "GroupBox", "Label", "ProgressBar" },
            { "Button", "Label", "ProgressBar", "GroupBox" }
        };

        // Номера правильных вариантов для каждого вопроса.
        // Счёт в программе начинается с нуля:
        // 0 — первый ответ, 1 — второй, 2 — третий, 3 — четвёртый.
        int[] correctAnswers = { 1, 2, 0, 3, 1 };

        int questionNumber = 0; // Номер текущего вопроса. 0 — это первый вопрос.
        int correctCount = 0;   // Сколько правильных ответов дал пользователь.

        // Этот код запускается при открытии окна.
        public Form1()
        {
            // Настраиваем само окно: название, размер, положение.
            Text = "Викторина";
            ClientSize = new Size(510, 340);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Font = new Font("Segoe UI", 10);

            // Добавляем на окно место для текста вопроса.
            questionLabel.Location = new Point(25, 20);
            questionLabel.Size = new Size(460, 50);
            questionLabel.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            Controls.Add(questionLabel);

            // Создаём рамку, внутри которой будут варианты ответов.
            answersBox.Text = "Варианты ответов";
            answersBox.Location = new Point(25, 80);
            answersBox.Size = new Size(460, 175);
            Controls.Add(answersBox);

            // Цикл 4 раза создаёт по одному кружочку RadioButton.
            // i меняется так: 0, 1, 2, 3.
            for (int i = 0; i < 4; i++)
            {
                choices[i] = new RadioButton();
                choices[i].Location = new Point(18, 28 + i * 35); // Каждый ответ ниже предыдущего.
                choices[i].Size = new Size(420, 30);
                answersBox.Controls.Add(choices[i]); // Помещаем ответ внутрь рамки.
            }

            // Создаём полоску, показывающую продвижение по 5 вопросам.
            progressBar.Location = new Point(25, 270);
            progressBar.Size = new Size(350, 22);
            progressBar.Minimum = 0;
            progressBar.Maximum = 5;
            Controls.Add(progressBar);

            // Рядом с полоской будет написано, например, «3/5».
            progressLabel.Location = new Point(390, 270);
            progressLabel.Size = new Size(95, 25);
            progressLabel.TextAlign = ContentAlignment.MiddleCenter;
            Controls.Add(progressLabel);

            // Создаём кнопку. При нажатии запускается NextButton_Click.
            nextButton.Text = "Далее";
            nextButton.Location = new Point(355, 301);
            nextButton.Size = new Size(130, 30);
            nextButton.Click += NextButton_Click;
            Controls.Add(nextButton);

            ShowQuestion(); // Сразу показываем первый вопрос.
        }

        // Показывает на экране нужный вопрос и 4 ответа.
        void ShowQuestion()
        {
            // Берём вопрос по его номеру.
            questionLabel.Text = questions[questionNumber];

            // Заполняем четыре ответа текстом и убираем старый выбор.
            for (int i = 0; i < 4; i++)
            {
                choices[i].Text = answers[questionNumber, i];
                choices[i].Checked = false; // Новый вопрос — пока ничего не выбрано.
            }

            // Обновляем полоску и надпись с номером вопроса.
            progressBar.Value = questionNumber + 1;
            progressLabel.Text = (questionNumber + 1) + "/5";

            // На пятом вопросе вместо «Далее» пишем «Результат».
            nextButton.Text = questionNumber == 4 ? "Результат" : "Далее";
        }

        // Этот метод выполняется при нажатии на кнопку.
        void NextButton_Click(object sender, EventArgs e)
        {
            int selected = -1; // -1 означает, что ответ пока не выбран.

            // Проверяем все 4 кружочка и ищем выбранный.
            for (int i = 0; i < 4; i++)
            {
                if (choices[i].Checked)
                    selected = i; // Запоминаем номер выбранного ответа.
            }

            // Если ничего не выбрали, просим выбрать ответ и не идём дальше.
            if (selected == -1)
            {
                MessageBox.Show("Выберите ответ!");
                return; // Останавливаем обработку нажатия.
            }

            // Если выбран правильный вариант, прибавляем одно очко.
            if (selected == correctAnswers[questionNumber])
                correctCount++;

            questionNumber++; // Переходим к следующему вопросу.

            // Если вопросы ещё остались, показываем следующий.
            if (questionNumber < 5)
            {
                ShowQuestion();
            }
            else
            {
                // Пять вопросов закончились: блокируем ответы и кнопку.
                questionLabel.Text = "Викторина завершена!";
                answersBox.Enabled = false;
                nextButton.Enabled = false;

                // Показываем итог: сколько правильных ответов из пяти.
                MessageBox.Show("Правильных ответов: " + correctCount + " из 5", "Результат");
            }
        }
    }
}
