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
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для Checking.xaml
    /// </summary>
    public partial class Checking : Window
    {
        string connectionString;
        DataTable dt = new DataTable();
        SqlDataAdapter adapter;
        public Checking()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["RestoranConnectionString"].ConnectionString;
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cm = new SqlCommand("select * from tab", connection);
            adapter = new SqlDataAdapter(cm);
            connection.Open();
            adapter.Fill(dt);
            dataGrid1.ItemsSource = dt.DefaultView;
            t1.MaxLength = 5;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataRowView rowView = dataGrid1.SelectedValue as DataRowView;
                t2.Text = rowView[0].ToString();
            }
            catch (System.NullReferenceException) { MessageBox.Show("Выберите строку в таблице."); }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
           {
                DateTime? selectedDate = dp.SelectedDate;
                string formatted = selectedDate.Value.ToString("dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture);
                var data0 = formatted + " " + t1.Text;
                var data1 = Convert.ToDateTime(data0).AddHours(-1);
                var data2 = Convert.ToDateTime(data0).AddHours(1);
                int tab = Convert.ToInt32(t2.Text);
                string connectionString;
                connectionString = ConfigurationManager.ConnectionStrings["RestoranConnectionString"].ConnectionString;
                SqlConnection connection = new SqlConnection(connectionString);
                SqlDataAdapter adapter = new SqlDataAdapter();
                DataTable table = new DataTable();
                string query = $"select * from res where time_res >= '{data1}' and [time_res] <= '{data2}' and k_tab = '{tab}' and status='Ожидает'";
                SqlCommand command = new SqlCommand(query, connection);
                adapter.SelectCommand = command;
                adapter.Fill(table);
                if (table.Rows.Count != 0)
                {
                    MessageBox.Show("К сожалению, стол уже занят на ближайший ко времени час. Предложите клиенту другой стол.");

                }
                else
                {
                    MessageBoxResult dialogResult = MessageBox.Show("Перейти к странице добавления?", "Стол еще не занят.", MessageBoxButton.YesNo);
                    if (dialogResult == MessageBoxResult.Yes)
                    {
                        Manager.MainFrame.Navigate(new AddResPage(data0,t1.Text,tab));
                        this.Close();
                    }

                }
            }
            catch (System.FormatException) {MessageBox.Show("Введите дату, время и выберите стол."); }
            catch (System.InvalidOperationException) { MessageBox.Show("Введите дату и время."); }
            catch (Exception) { MessageBox.Show("Проверьте подключение."); } 



        }

    }
}
