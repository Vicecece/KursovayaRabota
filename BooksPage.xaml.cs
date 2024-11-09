using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KursovayaRabota
{
    /// <summary>
    /// Логика взаимодействия для BooksPage.xaml
    /// </summary>
    public partial class BooksPage : Page
    {
        int CountRecords = CityLibraryEntities.GetContext().Литература.Count();
        public BooksPage()
        {
            InitializeComponent();
            var currentBooks = CityLibraryEntities.GetContext().Литература.ToList();
            BooksListView.ItemsSource = currentBooks;
            TBallRecords.Text = "Записей: " + CountRecords + " из " + CountRecords;
        }
        
        
        private void UpdateBooks()
        {
            var currentBooks = CityLibraryEntities.GetContext().Литература.ToList();

            if (ComboType.SelectedIndex == 0)
            {
                currentBooks = currentBooks/*.Where(p => (p.Категория_литературы == "Художественная")).ToList()*/;
            }
            if (ComboType.SelectedIndex == 1)
            {
                currentBooks = currentBooks.Where(p => (p.Категория_литературы == "Художественная")).ToList();
            }
            if (ComboType.SelectedIndex == 2)
            {
                currentBooks = currentBooks.Where(p => (p.Категория_литературы == "Фантастика")).ToList();
            }
            if (ComboType.SelectedIndex == 3)
            {
                currentBooks = currentBooks.Where(p => (p.Категория_литературы == "Научная")).ToList();
            }
            if (ComboType.SelectedIndex == 4)
            {
                currentBooks = currentBooks.Where(p => (p.Категория_литературы == "Научно-популярная")).ToList();
            }
            if (ComboType.SelectedIndex == 5)
            {
                currentBooks = currentBooks.Where(p => (p.Категория_литературы == "Образовательная")).ToList();
            }
            if (ComboType.SelectedIndex == 6)
            {
                currentBooks = currentBooks.Where(p => (p.Категория_литературы == "Практическая")).ToList();
            }

            if (RButtonDown.IsChecked.Value)
            {
                currentBooks = currentBooks.OrderByDescending(p => p.Кол_во_страниц).ToList();
            }
            if (RButtonUp.IsChecked.Value)
            {
                currentBooks = currentBooks.OrderBy(p => p.Кол_во_страниц).ToList();
            }

            var CountRecordsNew = currentBooks.Count;
            TBallRecords.Text = "Записей: " + CountRecordsNew + " из " + CountRecords;

            
            currentBooks = currentBooks.Where(p => p.Название.ToLower().Contains(TboxSearch.Text.ToLower())).ToList();
            BooksListView.ItemsSource = currentBooks;
        }

        private void TboxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateBooks();
        }

        private void ComboType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateBooks();
        }

        private void RButtonUp_Checked(object sender, RoutedEventArgs e)
        {
            UpdateBooks();
        }

        private void RButtonDown_Checked(object sender, RoutedEventArgs e)
        {
            UpdateBooks();
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (((sender as Button).DataContext as Литература).Кол_во_единиц_лит_ры == 0)
            {
                if (MessageBox.Show("Вы точно хотите выполнить удаление?", "Внимание", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    CityLibraryEntities.GetContext().Литература.Remove((sender as Button).DataContext as Литература);
                    CityLibraryEntities.GetContext().SaveChanges();

                    BooksListView.ItemsSource = CityLibraryEntities.GetContext().Литература.ToList();
                    CountRecords -= 1; 
                    UpdateBooks();
                }

            }
            else
            {
                MessageBox.Show("Невозможно удалить", "Ошибка");
            }
        }

        private void DeleteFilter_Click(object sender, RoutedEventArgs e)
        {
            RButtonDown.IsChecked = false;
            RButtonUp.IsChecked = false;
            UpdateBooks();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPage(null));
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPage((sender as Button).DataContext as Литература));
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                CityLibraryEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                BooksListView.ItemsSource = CityLibraryEntities.GetContext().Литература.ToList();
            }
            CountRecords = CityLibraryEntities.GetContext().Литература.Count();
            UpdateBooks();
        }
    }
}
