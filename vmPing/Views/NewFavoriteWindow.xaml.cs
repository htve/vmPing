﻿using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using vmPing.Classes;

namespace vmPing.Views
{
    /// <summary>
    /// NewFavoriteWindow provides an interface for creating a favorite.  A favorite is a
    /// collection of hosts that can be recalled later.
    /// </summary>
    public partial class NewFavoriteWindow : Window
    {
        private List<string> HostList;
        private int ColumnCount;

        public NewFavoriteWindow(List<string> hostList, int columnCount)
        {
            InitializeComponent();

            Contents.ItemsSource = hostList;

            HostList = hostList;
            ColumnCount = columnCount;

            // Set initial focus to text box.
            Loaded += (sender, e) =>
                MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            // Validate favorite name.
            if (Favorite.IsTitleInvalid(MyTitle.Text))
            {
                var errorWindow = DialogWindow.ErrorWindow($"Please enter a valid name for this favorite set.");
                errorWindow.Owner = this;
                errorWindow.ShowDialog();
                MyTitle.Focus();
                MyTitle.SelectAll();
                return;
            }

            // Check if favorite title already exists.
            if (Favorite.DoesTitleExist(MyTitle.Text))
            {
                var warningWindow = DialogWindow.WarningWindow(
                    message: $"{MyTitle.Text} already exists.  Would you like to overwrite?",
                    confirmButtonText: "Overwrite");
                warningWindow.Owner = this;
                if (warningWindow.ShowDialog() == true)
                {
                    // User opted to overwrite existing favorite entry.
                    SaveFavorite();
                }
            }
            else
            {
                // Checks passed.  Saving.
                SaveFavorite();
            }
        }

        private void SaveFavorite()
        {
            Favorite.SaveFavoriteSet(MyTitle.Text, HostList, ColumnCount);
            DialogResult = true;
        }
    }
}
