﻿using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace NedlastingKlient.Gui
{
    /// <summary>
    ///     Interaction logic for ListAllDatasets.xaml
    /// </summary>
    public partial class ListAllDatasets : Page
    {
        public ListAllDatasets()
        {
            InitializeComponent();
            DgDatasets.ItemsSource = new DatasetService().GetDatasets();
        }

        private void ShowDatasetClick(object sender, RoutedEventArgs e)
        {
            Dataset selectedDataset = DgDatasets.SelectedItem as Dataset;

            DatasetDetails detailsPage = new DatasetDetails();

            NavigationService.Navigate(detailsPage);
        }
    }
}