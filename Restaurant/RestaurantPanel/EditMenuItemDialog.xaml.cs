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
using System.Windows.Shapes;
using DBAccess;
using Project_s_classes;

namespace RestaurantPanel
{
    /// <summary>
    /// Interaction logic for EditMenuItemDialog.xaml
    /// </summary>
    public partial class EditMenuItemDialog : Window
    {
        private readonly Project_s_classes.Menu _menu;

        public EditMenuItemDialog(Project_s_classes.Menu menu)
        {
            InitializeComponent();
            _menu = menu;

            ItemNameTextBox.Text = _menu.ItemName;
            IngredientsTextBox.Text = _menu.Ingredients;
            PriceTextBox.Text = _menu.Price.ToString();
            QuantityTextBox.Text = _menu.QuantityAvailable.ToString();
        }

        private void SaveChangesButton_Click(object sender, RoutedEventArgs e)
        {
            _menu.ItemName = ItemNameTextBox.Text;
            _menu.Ingredients = IngredientsTextBox.Text;
            _menu.Price = decimal.Parse(PriceTextBox.Text); // Add validation for decimal parsing
            _menu.QuantityAvailable = int.Parse(QuantityTextBox.Text); // Add validation for integer parsing

            DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
