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
using Microsoft.VisualBasic.ApplicationServices;
using Project_s_classes;


namespace Restaurant
{
    /// <summary>
    /// Interaction logic for MenuItemDetailsWindow.xaml
    /// </summary>
    public partial class MenuItemDetailsWindow : Window
    {
        private readonly Project_s_classes.Menu _menuItem;
        private readonly Users _currentUser;

        public MenuItemDetailsWindow(Project_s_classes.Menu menuItem, Users currentUser)
        {
            InitializeComponent();
            _menuItem = menuItem;
            _currentUser = currentUser;
            DataContext = _menuItem;

            LoadComments();
        }

        private void LoadComments()
        {
            _menuItem.Comments = Comment.GetCommentsForMenu((int)_menuItem.MenuID).ToList();

            RefreshCommentList();
        }

        private void SubmitCommentButton_Click(object sender, RoutedEventArgs e)
        {
            bool hasRated = _menuItem.Comments.Any(c => c.UserID == _currentUser.UserID && c.Rating != null);

            if (hasRated)
            {
                MessageBox.Show("You have already rated this menu item.");

                float existingRating = _menuItem.Comments.FirstOrDefault(c => c.UserID == _currentUser.UserID && c.Rating != null)?.Rating ?? 0;

                var newComment = new Comment(_menuItem.MenuID, _currentUser.UserID, _currentUser.UserName, CommentTextBox.Text, existingRating, DateTime.Now, false);
                newComment.SaveToDatabase();
                _menuItem.Comments.Add(newComment);
            }
            else
            {
                var newComment = new Comment(_menuItem.MenuID, _currentUser.UserID, _currentUser.UserName, CommentTextBox.Text, (float)RatingSlider.Value, DateTime.Now, false);
                newComment.SaveToDatabase();
                _menuItem.Comments.Add(newComment);
            }

            RefreshCommentList();

            CommentTextBox.Text = "";
            RatingSlider.Value = 5;
        }


        private void EditCommentButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            int commentId = (int)button.Tag;
            var commentToEdit = _menuItem.Comments.FirstOrDefault(c => c.CommentID == commentId);
            if (commentToEdit != null)
            {
                CommentTextBox.Text = commentToEdit.Content;
                RatingSlider.Value = (float)commentToEdit.Rating;
                SubmitCommentButton.Tag = commentToEdit.CommentID;
            }
        }

        private void SubmitEditedCommentButton_Click(object sender, RoutedEventArgs e)
        {
            if (SubmitCommentButton.Tag is int commentId)
            {
                var commentToEdit = _menuItem.Comments.FirstOrDefault(c => c.CommentID == commentId);
                if (commentToEdit != null)
                {
                    commentToEdit.Content = CommentTextBox.Text;
                    commentToEdit.Rating = (float)RatingSlider.Value;
                    commentToEdit.Edited = true;
                    commentToEdit.UpdateInDatabase(); 
                    RefreshCommentList();

                    // Clear editing state
                    CommentTextBox.Text = "";
                    RatingSlider.Value = 5;
                    SubmitCommentButton.Tag = null;
                }
            }
        }

        

        private void DeleteCommentButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            int commentId = (int)button.Tag;
            var commentToDelete = _menuItem.Comments.FirstOrDefault(c => c.CommentID == commentId);
            if (commentToDelete != null)
            {
                commentToDelete.Delete();
                _menuItem.Comments.Remove(commentToDelete);
                RefreshCommentList();
            }
        }

        private void RefreshCommentList()
        {
            CommentListBox.ItemsSource = null;
            CommentListBox.ItemsSource = _menuItem.Comments;
        }
    }
}

