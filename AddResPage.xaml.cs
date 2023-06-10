using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для AddResPage.xaml
    /// </summary>
    public partial class AddResPage : Page
    {
        string connectionString;
        DataTable dt = new DataTable();
        DataTable dt2 = new DataTable();
        SqlDataAdapter adapter;
        SqlDataAdapter adapter2;
        string f = Manager.famil;
        public AddResPage(string d1, string time1, int stol)
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["RestoranConnectionString"].ConnectionString;
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cm2 = new SqlCommand($"SELECT distinct status from res", connection);
            adapter2 = new SqlDataAdapter(cm2);
            connection.Open();
            adapter2.Fill(dt2);
            combo.ItemsSource = dt2.DefaultView;
            combo.DisplayMemberPath = "status";

            t1.MaxLength = 150;
            t2.MaxLength = 11;
            t4.MaxLength = 5;
            dp.SelectedDate = Convert.ToDateTime(d1);
            t4.Text = time1;
            t6.Text = stol.ToString();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string time = t4.Text;
                DateTime? selectedDate = dp.SelectedDate;
                string formatted = selectedDate.Value.ToString("dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture);
                var data0 = formatted + " " + time;
                var data1 = Convert.ToDateTime(data0).AddHours(-1);
                var data2 = Convert.ToDateTime(data0).AddHours(1);

                connectionString = ConfigurationManager.ConnectionStrings["RestoranConnectionString"].ConnectionString;
                SqlConnection connection = new SqlConnection(connectionString);
                SqlCommand cm = new SqlCommand($"SELECT k_tab, location FROM tab WHERE k_tab NOT IN (SELECT k_tab FROM res WHERE time_res >= '{data1}' and [time_res] <= '{data2}' AND status = 'Ожидает');", connection);
                adapter = new SqlDataAdapter(cm);
                connection.Open();
                adapter.Fill(dt);
                dataGrid1.ItemsSource = dt.DefaultView;

                dataGrid1.Visibility = Visibility.Visible;
                b2.IsEnabled = false;
                b1.Visibility = Visibility.Visible;
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }


        }

        private void b1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataRowView rowView = dataGrid1.SelectedValue as DataRowView;
                t6.Text = rowView[0].ToString();
                b2.IsEnabled = true;
                b1.Visibility = Visibility.Hidden;
                dataGrid1.Visibility = Visibility.Hidden;
            }
            catch (System.NullReferenceException) { MessageBox.Show("Выберите строку в таблице."); }
        }

        private void b2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (t1.Text == "" || t2.Text == "" || combo.Text == "")
                {
                    MessageBox.Show("Заполните все поля в соответствии с форматом!");
                }
                else
                {
                    string fio = t1.Text;
                    string num = t2.Text;
                    string time = t4.Text;
                    string sts = combo.Text;
                    int tab = Convert.ToInt32(t6.Text);
                    DateTime? selectedDate = dp.SelectedDate;
                    string formatted = selectedDate.Value.ToString("dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    var data0 = formatted + " " + time;
                    var data1 = Convert.ToDateTime(data0).AddHours(-1);
                    var data2 = Convert.ToDateTime(data0).AddHours(1);
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
                        SqlConnection sqlconn = new SqlConnection(connectionString);
                        //подключение бд
                        SqlCommand sqlcomm = new SqlCommand("AddResProc", sqlconn);
                        //в SqlCommand задаем процедуру
                        sqlcomm.CommandType = System.Data.CommandType.StoredProcedure;
                        sqlcomm.Parameters.Add("@fio", System.Data.SqlDbType.VarChar).Value = fio;
                        sqlcomm.Parameters.Add("@num", System.Data.SqlDbType.VarChar).Value = num;
                        sqlcomm.Parameters.Add("@tab", System.Data.SqlDbType.Int).Value = tab;
                        sqlcomm.Parameters.Add("@time_res", System.Data.SqlDbType.DateTime).Value = data0;
                        sqlcomm.Parameters.Add("@status", System.Data.SqlDbType.VarChar).Value = sts;
                        sqlcomm.Parameters.Add("@fam", System.Data.SqlDbType.VarChar).Value = f;
                        //задаем значения процедур
                        SqlDataAdapter sqladap = new SqlDataAdapter(sqlcomm);
                        System.Data.DataTable dtable = new System.Data.DataTable();
                        sqladap.Fill(dtable);
                        //помещаем результат процедуры
                        Manager.MainFrame.Navigate(new PageRes(0));
                        MessageBox.Show("Добавление завершено. Перезайдите на страницу.");
                    }
                }
            }

            catch (System.FormatException) { MessageBox.Show("Заполните все поля в соответствии с форматом!"); }
            catch (System.InvalidOperationException) { MessageBox.Show("Введите дату и время"); }
            catch (System.Data.SqlClient.SqlException) { MessageBox.Show("Вы не можете добавить запись на прошедшее время, либо стол уже занят."); }

        }

        private void dp_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            dt.Clear();
            dataGrid1.Visibility = Visibility.Hidden;
        }

        private void t4_GotFocus(object sender, RoutedEventArgs e)
        {
            dt.Clear();
            dataGrid1.Visibility = Visibility.Hidden;
        }
    }
}
