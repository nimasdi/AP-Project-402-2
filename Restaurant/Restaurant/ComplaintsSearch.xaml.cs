using DBAccess;
using Microsoft.VisualBasic.ApplicationServices;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Restaurant_Pages
{
    /// <summary>
    /// Interaction logic for ComplaintsSearch.xaml
    /// </summary>
    public partial class ComplaintsSearch : Window
    {
        public ComplaintsSearch()
        {
            InitializeComponent();
        }

        static DataAccess dataAccess = new DataAccess();

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Exiting complaint search panel");
            this.Close();
        }

        private void ComplaintSearchButton_Click(object sender, RoutedEventArgs e)
        {
            string? selectedAspect = ComplaintSearchCriteriaComboBox.SelectedItem.ToString()
                    .Split(':')[1].Trim();
            if (string.IsNullOrEmpty(txtSearch.Text) && selectedAspect != "Latest Complaint" && selectedAspect != "All")
            {
                MessageBox.Show("The search txt box cannot be empty, please try again!");
            }
            else
            {
                var Complaints = dataAccess.LoadData<Complaint, dynamic>("SELECT * FROM dbo.Complaints", new { });
                var Restaurants = dataAccess.LoadData<Restaurants, dynamic>("SELECT * FROM dbo.Restaurants", new { });
                var Users = dataAccess.LoadData<Users, dynamic>("SELECT * FROM dbo.Users", new { });

                
                string filterName = txtSearch.Text;

                if (string.IsNullOrEmpty(selectedAspect))
                {
                    MessageBox.Show("You should choose something to search for in combobox");
                }
                else
                {
                    switch (selectedAspect)
                    {
                        case "Username":
                            var userIds = Users.Where(user => user.UserName.Equals(filterName, StringComparison.OrdinalIgnoreCase))
                           .Select(user => user.UserID)
                           .ToList();

                            if (!userIds.Any())
                            {
                                MessageBox.Show("There is no user object with such user name");
                                ComplaintResultsDataGrid.ItemsSource = null;
                                break;
                            }
                            else
                            {
                                var filter = Complaints.Where(complaint => userIds.Contains((int)complaint.UserId)).ToList();
                                ComplaintResultsDataGrid.ItemsSource = filter;
                                break;
                            }

                        case "Title":
                            var titleMatch = Complaints.Where(complaint => complaint.Title == filterName);
                            if (!titleMatch.Any())
                            {
                                MessageBox.Show("There is no title to match what has been written");
                                ComplaintResultsDataGrid.ItemsSource = null;
                                break;
                            }
                            else
                            {
                                ComplaintResultsDataGrid.ItemsSource = titleMatch;
                                break;
                            }

                        case "First Name":
                            var filter_firstname = Users.Where(user => user.FirstName.Equals(filterName, StringComparison.OrdinalIgnoreCase)).Select(user => user.UserID)
                           .ToList();

                            if (!filter_firstname.Any())
                            {
                                MessageBox.Show("There is no user available with such firstname");
                                ComplaintResultsDataGrid.ItemsSource = null;
                                break;
                            }
                            else
                            {
                                var filter = Complaints.Where(complaint => filter_firstname.Contains((int)complaint.UserId)).ToList();
                                ComplaintResultsDataGrid.ItemsSource = filter;
                                break;
                            }

                        case "Last Name":
                            var filter_lastname = Users.Where(user => user.LastName.Equals(filterName, StringComparison.OrdinalIgnoreCase)).Select(user => user.UserID)
                           .ToList();

                            if (!filter_lastname.Any())
                            {
                                MessageBox.Show("There is no user available with such lastname");
                                ComplaintResultsDataGrid.ItemsSource = null;
                                break;
                            }
                            else
                            {
                                var filter = Complaints.Where(complaint => filter_lastname.Contains((int)complaint.UserId)).ToList();
                                ComplaintResultsDataGrid.ItemsSource = filter;
                                break;
                            }

                        case "Status":
                            if (filterName != "Checked" &&  filterName != "Not Checked")
                            {
                                MessageBox.Show("Wrong input for txt, you are allowed to use 'Checked' or 'Not Checked' here");
                                ComplaintResultsDataGrid.ItemsSource = null;
                                break;
                            }
                            else
                            {
                                var filterStatus = Complaints.Where(complaint => complaint.Status == filterName);
                                ComplaintResultsDataGrid.ItemsSource = filterStatus;
                                break;
                            }

                        case "Restaurant Name":
                            var resturantName = Restaurants.Where(res => res.Name.Equals(filterName, StringComparison.OrdinalIgnoreCase)).Select(res => res.RestaurantID).ToList();
                            if (!resturantName.Any())
                            {
                                MessageBox.Show("There is no restaurant with such name");
                                ComplaintResultsDataGrid.ItemsSource = null;
                                break;
                            }
                            else
                            {
                                var filter = Complaints.Where(complaint => resturantName.Contains((int)complaint.RestaurantId)).ToList();
                                ComplaintResultsDataGrid.ItemsSource = filter;
                                break;
                            }

                        case "Latest Complaint":
                            Complaint? latesComplaint = Complaints.OrderByDescending(complaint => complaint.CreateDate).Where(complaint => complaint.Status == "Not Checked").FirstOrDefault();
                            List<Complaint> com = new List<Complaint>();
                            com.Add(latesComplaint);
                            if (latesComplaint != null)
                            {
                                ComplaintResultsDataGrid.ItemsSource = com;
                            }
                            else
                            {
                                MessageBox.Show("All the complaints have been checked");
                                ComplaintResultsDataGrid.ItemsSource = null;
                            }
                            break;
                        case "All":
                            if(Complaints != null)
                            {
                                ComplaintResultsDataGrid.ItemsSource = Complaints;
                            }
                            else
                            {
                                MessageBox.Show("There are no complaints filed from any user");
                                ComplaintResultsDataGrid.ItemsSource = null;
                            }
                            break;
                        default:
                            MessageBox.Show("Something went wrong");
                            break;
                    }
                }
            }
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void ComplaintResultsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ComplaintSearchCriteriaComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
