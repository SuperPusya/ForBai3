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
using System.Data.SqlClient;
using System.Configuration;
using System.Data;



namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            p1.MaxLength = 4;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
          
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {

                string connectionString;
                var pass = p1.Password.ToString();
                connectionString = ConfigurationManager.ConnectionStrings["RestoranConnectionString"].ConnectionString;
                SqlConnection connection = new SqlConnection(connectionString);
                SqlDataAdapter adapter = new SqlDataAdapter();
                DataTable table = new DataTable();
              //  string query = $"select * from works where pass = '{pass}' and post = 'Хостес'";
              string query = $"select * from works where pass = '{pass}'";
                SqlCommand command = new SqlCommand(query, connection);
                adapter.SelectCommand = command;
                adapter.Fill(table);
                if (table.Rows.Count != 0)
                {
                    SqlDataAdapter adapter1 = new SqlDataAdapter();
                    DataTable table1 = new DataTable();
                    string sql = $"select sur_w from works where pass = N'{pass}' and post = N'Хостес'";
                    SqlCommand command1 = new SqlCommand(sql, connection);
                    adapter1.SelectCommand = command1;
                    adapter1.Fill(table1);
                    if (table1.Rows.Count != 0)
                    {
                        SqlCommand cmd = new SqlCommand(sql, connection);
                        connection.Open();
                        string f = cmd.ExecuteScalar().ToString();
                        this.Hide();
                        Manager.famil = f;
                        Hostes s = new Hostes();
                        s.Show();
                        goto end;
                    }
                    string sql2 = $"select sur_w from works where pass = N'{pass}' and post = N'Администратор'";
                    command1 = new SqlCommand(sql2, connection);
                    adapter1.SelectCommand = command1;
                    adapter1.Fill(table1);
                    if (table1.Rows.Count != 0)
                    {
                        SqlCommand cmd = new SqlCommand(sql2, connection);
                        connection.Open();
                        string f = cmd.ExecuteScalar().ToString();
                        this.Hide();
                        Manager.famil = f;
                        Admin s = new Admin();
                        s.Show();
                        goto end;
                    }
                    string sql3 = $"select sur_w from works where pass = N'{pass}' and post = N'Официант'";
                    command1 = new SqlCommand(sql3, connection);
                    adapter1.SelectCommand = command1;
                    adapter1.Fill(table1);
                    if (table1.Rows.Count != 0)
                    {
                        SqlCommand cmd = new SqlCommand(sql3, connection);
                        connection.Open();
                        string f = cmd.ExecuteScalar().ToString();
                        this.Hide();
                        Manager.famil = f;
                        Waiter s = new Waiter();
                        s.Show();
                        goto end;
                    }
                    string sql4 = $"select sur_w from works where pass = N'{pass}' and post = N'Шеф-повар'";
                    command1 = new SqlCommand(sql4, connection);
                    adapter1.SelectCommand = command1;
                    adapter1.Fill(table1);
                    if (table1.Rows.Count != 0)
                    {
                        this.Hide();
                        Povar s = new Povar();
                        s.Show();
                        goto end;
                    }

                end:;
                }
                else
                {
                    MessageBox.Show("Пароль введен неверно. Попробуйте снова.");
                }
            }
            catch (System.Data.SqlClient.SqlException) { MessageBox.Show("Проверьте подключение к серверу и попробуйте еще раз"); }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }

        }

        private void Window_Closed(object sender, EventArgs e)
        {
            System.Environment.Exit(1);
        }
    }
}
