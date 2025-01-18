using System;
using System.Collections.ObjectModel;
using MobileMAUI_2.Model;
using MobileMAUI_2.Services;
using Microsoft.Maui.Controls;

namespace MobileMAUI_2.Pages
{
    public partial class FriendPage : ContentPage
    {
        private ObservableCollection<Friend> friends;
        private Friend selectedFriend;
        private readonly DatabaseService _databaseService;

        public FriendPage()
        {
            InitializeComponent();
            _databaseService = new DatabaseService();
            LoadFriends();
        }

        // Load all friends from the database
        private async void LoadFriends()
        {
            var friendList = await _databaseService.GetAllFriendsAsync();
            friends = new ObservableCollection<Friend>(friendList);
            FriendsCollectionView.ItemsSource = friends;
        }

        // Add Friend Button Clicked
        private async void OnAddFriendClicked(object sender, EventArgs e)
        {
            FriendFormFrame.IsVisible = true;  // Show the form
        }

        // Save Button Clicked (Add/Edit)
        private async void OnSaveClicked(object sender, EventArgs e)
        {
            if (selectedFriend == null)  // New Friend
            {
                var friend = new Friend
                {
                    Name = NameEntry.Text,
                    Phone = PhoneEntry.Text,
                    Description = DescriptionEntry.Text
                };
                await _databaseService.AddFriendAsync(friend);  // Save to the database
                friends.Add(friend);  // Add to the ObservableCollection
            }
            else  // Edit Existing Friend
            {
                selectedFriend.Name = NameEntry.Text;
                selectedFriend.Phone = PhoneEntry.Text;
                selectedFriend.Description = DescriptionEntry.Text;
                await _databaseService.UpdateFriendAsync(selectedFriend);  // Update the database
            }

            ClearForm();
        }

        // Cancel Button Clicked
        private void OnCancelClicked(object sender, EventArgs e)
        {
            ClearForm();
        }

        // Edit Button Clicked
        private void OnEditClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            selectedFriend = button?.BindingContext as Friend;

            if (selectedFriend != null)
            {
                NameEntry.Text = selectedFriend.Name;
                PhoneEntry.Text = selectedFriend.Phone;
                DescriptionEntry.Text = selectedFriend.Description;
                FriendFormFrame.IsVisible = true;  // Show the form to edit
            }
        }

        // Delete Button Clicked
        private async void OnDeleteClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var friend = button?.BindingContext as Friend;

            if (friend != null)
            {
                friends.Remove(friend);  // Remove from ObservableCollection
                await _databaseService.DeleteFriendAsync(friend);  // Delete from the database
            }
        }

        // On Selection Changed
        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.Count > 0)
            {
                var selectedFriend = e.CurrentSelection[0] as Friend;
                // Handle selection logic if needed
                // For example, populate the form with selected friend's data
            }
        }

        // Clear Form Fields
        private void ClearForm()
        {
            NameEntry.Text = "";
            PhoneEntry.Text = "";
            DescriptionEntry.Text = "";
            FriendFormFrame.IsVisible = false;
            selectedFriend = null;
        }
    }
}
