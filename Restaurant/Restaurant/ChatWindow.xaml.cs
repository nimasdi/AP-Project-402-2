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
using Microsoft.AspNetCore.SignalR.Client;
using Project_s_classes;

namespace Restaurant
{
    /// <summary>
    /// Interaction logic for ChatWindow.xaml
    /// </summary>
    public partial class ChatWindow : Window
    {
        private HubConnection _connection;

        public ChatWindow()
        {
            InitializeComponent();
            _connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5000/chathub")
                .Build();

            _connection.On<string, string>("ReceiveMessage", (user, message) =>
            {
                Dispatcher.Invoke(() =>
                {
                    ChatTextBox.AppendText($"{user}: {message}\n");
                });
            });

            _connection.StartAsync();
        }

        private async void SendButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(MessageTextBox.Text))
            {
                await _connection.InvokeAsync("SendMessage", "User", MessageTextBox.Text);
                MessageTextBox.Clear();
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            _connection.StopAsync();
            base.OnClosed(e);
        }
    }
}
