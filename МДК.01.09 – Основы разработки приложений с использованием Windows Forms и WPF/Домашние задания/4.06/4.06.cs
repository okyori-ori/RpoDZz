using System;
using System.Windows;
using System.Windows.Controls;
using System.Globalization;
using System.Collections.ObjectModel;

namespace Homework
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            NotesList.ItemsSource = notes;

        }

        private ObservableCollection<Note> notes = new ObservableCollection<Note>();

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TitleBox.Text))
            {
                MessageBox.Show("Введите заголовок.");
                return;
            }
            Note note = new Note { Title = TitleBox.Text, Content = ContentBox.Text, Created = DateTime.Now };
            notes.Add(note);
            NotesList.SelectedItem = note;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            Note note = NotesList.SelectedItem as Note;
            if (note == null || string.IsNullOrWhiteSpace(TitleBox.Text))
            {
                MessageBox.Show("Выберите заметку и заполните заголовок.");
                return;
            }
            note.Title = TitleBox.Text;
            note.Content = ContentBox.Text;
            NotesList.Items.Refresh();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            Note note = NotesList.SelectedItem as Note;
            if (note == null) { MessageBox.Show("Выберите заметку."); return; }
            notes.Remove(note);
            Clear_Click(sender, e);
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            NotesList.SelectedIndex = -1;
            TitleBox.Clear();
            ContentBox.Clear();
            DateBlock.Text = "Дата создания:";
        }

        private void NotesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Note note = NotesList.SelectedItem as Note;
            if (note == null) return;
            TitleBox.Text = note.Title;
            ContentBox.Text = note.Content;
            DateBlock.Text = "Дата создания: " + note.Created.ToString("dd.MM.yyyy HH:mm");
        }

    }
}
