using System;
using System.Collections.Generic;
using System.Windows;
using Diary.ViewModel;

namespace Diary
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            MainWindow window = new MainWindow();

            var viewModel = new MainWindowViewModel(Diary.Properties.Resources.ConnectCommand);

            window.DataContext = viewModel;

            window.Show();
        }
    }
}
