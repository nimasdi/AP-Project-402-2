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
    /// Interaction logic for InputDialog.xaml
    /// </summary>
    public partial class InputDialog : Window
    {
        public string Input { get; private set; }
        public InputDialog(string title, string prompt)
        {
            InitializeComponent();
            this.Title = title;
            PromptTextBlock.Text = prompt;
        }

        public InputDialog(string title, string prompt, string initialInput)
        {
            InitializeComponent();
            this.Title = title;
            PromptTextBlock.Text = prompt;
            InputTextBox.Text = initialInput;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(InputTextBox.Text))
            {
                MessageBox.Show("Please enter a value.");
                return;
            }

            Input = InputTextBox.Text;
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
