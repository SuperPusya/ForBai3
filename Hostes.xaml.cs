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

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для Hostes.xaml
    /// </summary>
    public partial class Hostes : Window
    {
        public Hostes()
        {
            InitializeComponent();
            Manager.MainFrame = frame;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new PageRes(0));
            text.Visibility = Visibility.Hidden;
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new PageTab());
            text.Visibility = Visibility.Hidden;
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            Checking c = new Checking();
            c.Show();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
           // System.Environment.Exit(1);
        }



        private void MenuItem_Click_4(object sender, RoutedEventArgs e)
        {
            MainWindow m = new MainWindow();
            m.Show();
            this.Close();
        }

        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            System.Environment.Exit(1);
        }
    }
}
