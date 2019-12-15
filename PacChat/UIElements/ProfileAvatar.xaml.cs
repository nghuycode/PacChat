﻿using Microsoft.Win32;
using PacChat.Network.RestAPI;
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

namespace PacChat.UIElements
{
    /// <summary>
    /// Interaction logic for ProfileAvatar.xaml
    /// </summary>
    public partial class ProfileAvatar : UserControl
    {
        public ProfileAvatar()
        {
            InitializeComponent();
        }

        private void UpdateBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();

            op.Title = "Select a picture";
            op.Filter = "Supported Graphics|*.jpg;*.jpeg|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|";

            if (op.ShowDialog() == true)
            {
                List<string> paths = op.FileNames.ToList();
                var app = MainWindow.chatApplication;
                FileAPI.UploadMedia(app.model.SelfID,
                    paths, OnImageUploadCompleted, OnImageUploadError);
            }
        }

        private void OnImageUploadError(Exception error)
        {
            throw new NotImplementedException();
        }

        private void OnImageUploadCompleted(Dictionary<string, string> result)
        {
            
        }
    }
}
