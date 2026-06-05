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

namespace WpfApp2
{
    /// <summary>
    /// Логика взаимодействия для ProductPage.xaml
    /// </summary>
    public partial class ProductPage : Page
    {
        public ProductPage()
        {
            InitializeComponent();

            ProductsListView.ItemsSource = Entities.GetContext().Products.ToList();
        }

        private void Update()
        {
            var Products = Entities.GetContext().Products.ToList();

            Products = FiltCB.SelectedIndex == 1 ? Products.Where(x => x.Discount < 12).ToList()
                : FiltCB.SelectedIndex == 2 ? Products.Where(x => x.Discount >= 12 && x.Discount < 19).ToList()
                : FiltCB.SelectedIndex == 3 ? Products.Where(x => x.Discount >= 19).ToList()
                : Products;

            Products = SortCB.SelectedIndex == 1 ? Products.OrderBy(x => x.Count).ToList()
                : SortCB.SelectedIndex == 2 ? Products.OrderByDescending(x => x.Count).ToList()
                : SortCB.SelectedIndex == 3 ? Products.OrderBy(x => x.Cost).ToList()
                : SortCB.SelectedIndex == 4 ? Products.OrderByDescending(x => x.Cost).ToList()
                : SortCB.SelectedIndex == 5 ? Products.OrderBy(x => x.Discount).ToList()
                : SortCB.SelectedIndex == 6 ? Products.OrderByDescending(x => x.Discount).ToList()
                : Products;

            foreach (var word in SearchTB.Text.Split(' '))
            {
                Products = Products
                    .Where(x => (x.Manufacturers.Name + x.Suppliers.Name + x.Article + x.CategoryName)
                    .ToLower()
                    .Contains(word.ToLower()))
                    .ToList();
            }

            ProductsListView.ItemsSource = Products;
        }
        private void SearchTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            Update();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Update();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var Result = MessageBox.Show("Вы уверены, что хотите удалить продукт?"
                , "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (Result == MessageBoxResult.No)
            {
                return;
            }

            var Product = (sender as Button).DataContext as Products;

            if (Product.OrderProducts.Count != 0)
            {
                MessageBox.Show("Нельзя удалить, продукт в заказе!");
                return;
            }

            Entities.GetContext().Products.Remove(Product);
            Entities.GetContext().SaveChanges();
            Update();
        }

        private void EditButton_Click_1(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPage());
        }
    }
}
