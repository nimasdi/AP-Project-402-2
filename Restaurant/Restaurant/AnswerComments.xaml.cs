using Dapper;
using DBAccess;
using Microsoft.Data.SqlClient;
using Project_s_classes;
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

namespace Restaurant
{
    /// <summary>
    /// Interaction logic for AnswerComments.xaml
    /// </summary>
    public partial class AnswerComments : Window
    {
        DataAccess dataAccess;
        public AnswerComments()
        {
            InitializeComponent();
            dataAccess = new DataAccess();
        }

        

        private void UpdateDataGrid()
        {

            var comments = dataAccess.LoadData<Comment, dynamic>("SELECT * FROM dbo.Comments", new { })
                .Where(comment => comment.AdminResponse == string.Empty);

            CommentsDataGrid.ItemsSource = comments;
        }

        private void CommentsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CommentsDataGrid.SelectedItem is Comment selectedComment)
            {
                MessageBox.Show($"Selected Comment ID: {selectedComment.CommentID}");
            }
            else
            {
                MessageBox.Show("No item selected");
            }
        }

        private void SubmitResponse_Click(object sender, RoutedEventArgs e)
        {
            if (CommentsDataGrid.SelectedItem is Comment selectedComment)
            {
                string response = ResponseTextBox.Text;
                if (!string.IsNullOrEmpty(response))
                {
                    int? commentId = selectedComment.CommentID;

                    try
                    {
                        using (var connection = new SqlConnection(dataAccess.ConnectionString))
                        {
                            string updateQuery = @"UPDATE dbo.Comments 
                                           SET AdminResponse = @AdminResponse, 
                                               ResponseDate = @ResponseDate 
                                           WHERE CommentID = @CommentID";

                            var parameters = new
                            {
                                AdminResponse = response,
                                ResponseDate = DateTime.Now,
                                CommentID = commentId
                            };

                            connection.Execute(updateQuery, parameters);
                        }

                        MessageBox.Show("The comment got answered by the admin");
                        UpdateDataGrid();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error updating data: " + ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("Response part cannot be empty, give a response to a comment");
                }
            }
            else
            {
                MessageBox.Show("Select an item to respond to");
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
