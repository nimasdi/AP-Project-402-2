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
        private readonly string _sender;

        public ChatWindow(bool isAdmin)
        {
            InitializeComponent();
            _sender = isAdmin ? "Admin" : "User";
            InitializeSignalR();
        }

        private async void InitializeSignalR()
        {
            _connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5290/chatHub")
                .Build();

            _connection.On<string, string>("ReceiveMessage", (user, message) =>
            {
                Dispatcher.Invoke(() =>
                {
                    ChatListBox.Items.Add($"{user}: {message}");
                });
            });

            try
            {
                await _connection.StartAsync();
                ChatListBox.Items.Add("Connected to chat server.");
            }
            catch (Exception ex)
            {
                ChatListBox.Items.Add($"Connection failed: {ex.Message}");
            }
        }

        private async void SendButton_Click(object sender, RoutedEventArgs e)
        {
            var message = MessageTextBox.Text;
            await _connection.InvokeAsync("SendMessage", _sender, message);
            MessageTextBox.Clear();
        }
    }
}
