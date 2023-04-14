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

namespace ArduinoApp01
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            InitializeComponent();

        }

        private void Exit_BTN_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            MessageBox.Show("Program is Closed","Info",MessageBoxButton.OK,MessageBoxImage.Information);
        }

        private void Continue_BTN_Click(object sender, RoutedEventArgs e)
        {
            Configs_Page Configration_Page = new Configs_Page();

            this.Visibility = Visibility.Hidden;

            Configration_Page.Show();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBox.Show("Program is Closed", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            e.Cancel = true;
            this.Visibility = Visibility.Collapsed;

        }
    }
}
