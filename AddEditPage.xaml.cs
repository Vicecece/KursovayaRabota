using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Логика взаимодействия для AddEditPage.xaml
    /// </summary>
    public partial class AddEditPage : Page
    {
        private Литература _currentLit = new Литература();
        public AddEditPage( Литература SelectedLit)
        {
            InitializeComponent();
            if (SelectedLit != null)
            {
                _currentLit = SelectedLit;
            }
            DataContext = _currentLit;

        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new BooksPage());
        }

        private void Picture_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                _currentLit.Image = openFileDialog.FileName;
                ImageLit.Source = new BitmapImage(new Uri(openFileDialog.FileName));
            }
        }
        public static bool IsValidYear(string year)
        {
            string pattern = @"[1234567890]{4}$";
            return Regex.IsMatch(year, pattern);
        }
        public static bool IsValidPage(string page)
        {
            string pattern = @"[1234567890]$";
            return Regex.IsMatch(page, pattern);
        }
        public static bool IsValidZal(string zal)
        {
            string pattern = @"[1234567890]$";
            return Regex.IsMatch(zal, pattern);
        }
        public static bool IsValidEdLit(string EdLit)
        {
            string pattern = @"[1234567890]$";
            return Regex.IsMatch(EdLit, pattern);
        }


        private void Save_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(_currentLit.Название))
            {
                errors.AppendLine("Укажите Название");
            }
            if (string.IsNullOrWhiteSpace(_currentLit.Категория_литературы))
            {
                errors.AppendLine("Укажите Категорию");
            }
            if (string.IsNullOrWhiteSpace(_currentLit.Авторы))
            {
                errors.AppendLine("Укажите Авторов");
            }
            if (string.IsNullOrWhiteSpace(_currentLit.Издательство))
            {
                errors.AppendLine("Укажите Издательство");
            }
            if (string.IsNullOrWhiteSpace(_currentLit.Год_издательства.ToString()))
            {
                errors.AppendLine("Укажите Год издательства");
            }
            if (string.IsNullOrWhiteSpace(_currentLit.Кол_во_страниц.ToString()))
            {
                errors.AppendLine("Укажите Кол-во страниц");
            }
            if (string.IsNullOrWhiteSpace(_currentLit.Читальный_зал_ID.ToString()))
            {
                errors.AppendLine("Укажите Читальный зал ID");
            }
            if (string.IsNullOrWhiteSpace(_currentLit.Кол_во_единиц_лит_ры.ToString()))
            {
                errors.AppendLine("Укажите Кол-во единиц литературы");
            }
            if (!IsValidYear(YearIzd.Text))
            {
                errors.AppendLine("Укажите год издательства правильно(4 цифры)");
            }
            if (!IsValidPage(CountPage.Text))
            {
                errors.AppendLine("Укажите кол-во страниц корректно");
            }
            if (!IsValidZal(ChitalID.Text))
            {
                errors.AppendLine("Укажите ID читального зала корректно ");
            }
            if (!IsValidEdLit(CountLit.Text))
            {
                errors.AppendLine("Укажите кол-во единиц литературы корректно");
            }
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }
            if (_currentLit.ID_литературы == 0)
            {
                CityLibraryEntities.GetContext().Литература.Add(_currentLit);
            }
            try
            {
                CityLibraryEntities.GetContext().SaveChanges();
                MessageBox.Show("Информация сохранена");
                Manager.MainFrame.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                
            }

        }
    }
}
