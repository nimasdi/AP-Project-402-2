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
        public string ImageURL { get; private set; }
        public float AverageRating { get; private set; }

        public AddMenuItemDialog(string category, int restaurantId)
        {
            InitializeComponent();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateInputs())
            {
                ItemName = ItemNameTextBox.Text;
                Ingredients = IngredientsTextBox.Text;
                Price = decimal.Parse(PriceTextBox.Text); // Ensure proper validation for decimal parsing
                Quantity = int.Parse(QuantityTextBox.Text); // Ensure proper validation for integer parsing
                ImageURL = ImageURLTextBox.Text;
                AverageRating = float.Parse(AverageRatingTextBox.Text); // Ensure proper validation for float parsing

                DialogResult = true;
            }
            else
            {
                MessageBox.Show("Please enter valid values for all fields.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private bool ValidateInputs()
        {
            // Perform validation for each input field
            if (string.IsNullOrWhiteSpace(ItemNameTextBox.Text) ||
                string.IsNullOrWhiteSpace(IngredientsTextBox.Text) ||
                !decimal.TryParse(PriceTextBox.Text, out _) ||
                !int.TryParse(QuantityTextBox.Text, out _) ||
                string.IsNullOrWhiteSpace(ImageURLTextBox.Text) ||
                !float.TryParse(AverageRatingTextBox.Text, out _))
            {
                return false;
            }

            return true;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateInputs())
            {
                ItemName = ItemNameTextBox.Text;
                Ingredients = IngredientsTextBox.Text;
                Price = decimal.Parse(PriceTextBox.Text); // Ensure proper validation for decimal parsing
                Quantity = int.Parse(QuantityTextBox.Text); // Ensure proper validation for integer parsing
                ImageURL = ImageURLTextBox.Text;
                AverageRating = float.Parse(AverageRatingTextBox.Text); // Ensure proper validation for float parsing

                DialogResult = true;
                Close(); // Close the dialog
            }
            else
            {
                MessageBox.Show("Please enter valid values for all fields.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
