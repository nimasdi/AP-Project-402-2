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

namespace RestaurantPanel
{
    /// <summary>
    /// Interaction logic for AddMenuItemDialog.xaml
    /// </summary>
    public partial class AddMenuItemDialog : Window
    {
        public string ItemName { get; private set; }
        public string Ingredients { get; private set; }
        public decimal Price { get; private set; }
        public int Quantity { get; private set; }

        public AddMenuItemDialog(string category, int restaurantId)
        {
            InitializeComponent();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            ItemName = ItemNameTextBox.Text;
            Ingredients = IngredientsTextBox.Text;
            Price = decimal.Parse(PriceTextBox.Text); // Add validation for decimal parsing
            Quantity = int.Parse(QuantityTextBox.Text); // Add validation for integer parsing

            DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
