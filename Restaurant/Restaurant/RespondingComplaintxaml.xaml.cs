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
    /// Interaction logic for RespondingComplaintxaml.xaml
    /// </summary>
    public partial class RespondingComplaintxaml : Window
    {
        public RespondingComplaintxaml()
        {
            InitializeComponent();
            UpdateDataGrid();
        }
        DataAccess dataAccess = new DataAccess();

        private void UpdateDataGrid()
        {
            var complaints = dataAccess.LoadData<Complaint, dynamic>("SELECT * FROM dbo.Complaints", new { })
                .Where(complaint => complaint.Status == "Not Checked");

            ComplaintsDataGrid.ItemsSource = complaints;
        }

        private void ComplaintsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComplaintsDataGrid.SelectedItem is Complaint selectedComplaint)
            {
                MessageBox.Show($"Selected Complaint ID: {selectedComplaint.ComplaintID}");
            }
            else
            {
                MessageBox.Show("No item selected");
            }
        }

        private void SubmitResponse_Click(object sender, RoutedEventArgs e)
        {
            if (ComplaintsDataGrid.SelectedItem is Complaint selectedComplaint)
            {
                string response = ResponseTextBox.Text;
                if (!string.IsNullOrEmpty(response))
                {
                    int? complaintId = selectedComplaint.ComplaintID;

                    try
                    {
                        using (var connection = new SqlConnection(dataAccess.ConnectionString))
                        {
                            var query = "UPDATE dbo.Complaints SET Response = @Response, Status = 'Checked' WHERE ComplaintID = @ComplaintID";
                            connection.Execute(query, new { Response = response, ComplaintID = complaintId });
                        }

                        MessageBox.Show("The complaint got answered by the admin");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error updating data: " + ex.Message);
                    }

                    
                    UpdateDataGrid();
                }
                else
                {
                    MessageBox.Show("Response part cannot be empty, give a response to a complaint");
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
