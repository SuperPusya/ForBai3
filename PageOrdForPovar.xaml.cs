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
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Windows.Threading;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для PageOrdForPovar.xaml
    /// </summary>
    public partial class PageOrdForPovar : Page
    {
        string connectionString;
        DataTable dt = new DataTable();
        DataTable dt2 = new DataTable();
        DataTable dt3 = new DataTable();
        SqlDataAdapter adapter;
        SqlDataAdapter adapter2;
        SqlDataAdapter adapter3;
        public PageOrdForPovar()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["RestoranConnectionString"].ConnectionString;
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cm = new SqlCommand("SELECT o.k_ord, STRING_AGG(d.n_dish, ', ') WITHIN GROUP (ORDER BY do.k_dish) AS dishes, w.sur_w AS sur_w, o.time_ord, o.cost_ord, o.status_ord, o.k_tab FROM ords o LEFT JOIN dishs_ord do ON o.k_ord = do.k_ord LEFT JOIN dishs d ON do.k_dish = d.k_dish LEFT JOIN works w ON o.k_w = w.k_w GROUP BY o.k_ord, w.sur_w, o.time_ord, o.cost_ord, o.status_ord, o.k_tab ORDER BY o.time_ord DESC;", connection);
            adapter = new SqlDataAdapter(cm);
            SqlCommand cm2 = new SqlCommand("SELECT distinct status_ord from ords", connection);
            adapter2 = new SqlDataAdapter(cm2);
            connection.Open();
            adapter.Fill(dt);
            adapter2.Fill(dt2);
            dataGrid1.ItemsSource = dt.DefaultView;
            combo.ItemsSource = dt2.DefaultView;
            combo.DisplayMemberPath = "status_ord";

            var td = DateTime.Now;
            var today = Convert.ToDateTime(td.AddHours(-3));
            try
            {
                dt.DefaultView.RowFilter = string.Format("[time_ord] >= '{0}'", today);
            }
            catch (FormatException) { MessageBox.Show("Проверьте правильность введенных данных"); }
            catch (OverflowException) { MessageBox.Show("Переполнение"); }
            catch (Exception) { MessageBox.Show("Что-то пошло не так"); }

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(60);

            // обработчик события Tick, который вызывается каждый раз через указанный интервал времени
            timer.Tick += Timer_Tick;

            // запуск таймера
            timer.Start();
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            try
            {
                // выполнение запроса к базе данных и получение новых данных
                string connectionString = ConfigurationManager.ConnectionStrings["RestoranConnectionString"].ConnectionString; // строка подключения к БД
                string selectQuery = "SELECT o.k_ord, STRING_AGG(d.n_dish, ', ') WITHIN GROUP (ORDER BY do.k_dish) AS dishes, w.sur_w AS sur_w, o.time_ord, o.cost_ord, o.status_ord, o.k_tab FROM ords o LEFT JOIN dishs_ord do ON o.k_ord = do.k_ord LEFT JOIN dishs d ON do.k_dish = d.k_dish LEFT JOIN works w ON o.k_w = w.k_w GROUP BY o.k_ord, w.sur_w, o.time_ord, o.cost_ord, o.status_ord, o.k_tab ORDER BY o.time_ord DESC;";
                SqlConnection connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand(selectQuery, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                // назначение DataTable свойству ItemsSource DataGrid
                var td = DateTime.Now;
                var today = Convert.ToDateTime(td.AddHours(-3));
                try
                {
                    dt.DefaultView.RowFilter = string.Format("[time_ord] >= '{0}'", today);
                }
                catch (FormatException) { MessageBox.Show("Проверьте правильность введенных данных"); }
                catch (OverflowException) { MessageBox.Show("Переполнение"); }
                catch (Exception) { MessageBox.Show("Что-то пошло не так"); }
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                dt.DefaultView.RowFilter = "";
            }
            catch (Exception) { MessageBox.Show("Что-то пошло не так"); }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            var td = DateTime.Now;
            var today = Convert.ToDateTime(td.AddHours(-3));
            try
            {
                dt.DefaultView.RowFilter = string.Format("[time_ord] >= '{0}'", today);
            }
            catch (FormatException) { MessageBox.Show("Проверьте правильность введенных данных"); }
            catch (OverflowException) { MessageBox.Show("Переполнение"); }
            catch (Exception) { MessageBox.Show("Что-то пошло не так"); }
        }

        private void dataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            bOp3.IsEnabled = true;
            bOp4.IsEnabled = true;
        }

        private void ComboBox_DropDownClosed(object sender, EventArgs e)
        {
            try
            {
                if (combo.Text != "")
                {
                    dt.DefaultView.RowFilter = string.Format("[status_ord] = '{0}' ", combo.Text);
                }
                else dt.DefaultView.RowFilter = "";
            }
            catch (Exception) { MessageBox.Show("Что-то пошло не так"); }
        }

        private void bOp3_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataRowView rowView = dataGrid1.SelectedValue as DataRowView;
                int n = Convert.ToInt32(rowView[0]);
                string connectionString;
                connectionString = ConfigurationManager.ConnectionStrings["RestoranConnectionString"].ConnectionString;
                SqlConnection connection = new SqlConnection(connectionString);
                SqlDataAdapter adapter = new SqlDataAdapter();
                DataTable table = new DataTable();
                SqlConnection sqlconn = new SqlConnection(connectionString);
                //подключение бд
                SqlCommand sqlcomm = new SqlCommand("UpdateStatusOrd3", sqlconn);
                //в SqlCommand задаем процедуру
                sqlcomm.CommandType = System.Data.CommandType.StoredProcedure;
                sqlcomm.Parameters.Add("@k_ord", System.Data.SqlDbType.Int).Value = n;
                //задаем значения процедур
                SqlDataAdapter sqladap = new SqlDataAdapter(sqlcomm);
                System.Data.DataTable dtable = new System.Data.DataTable();
                sqladap.Fill(dtable);
                Manager.MainFrame.Navigate(new PageOrdForPovar());
            }
            catch (System.NullReferenceException) { MessageBox.Show("Выберите строку в таблице."); }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }

        }

        private void bOp4_Click(object sender, RoutedEventArgs e)
        { try { 
            DataRowView rowView = dataGrid1.SelectedValue as DataRowView;
            int n = Convert.ToInt32(rowView[0]);
            string connectionString;
            connectionString = ConfigurationManager.ConnectionStrings["RestoranConnectionString"].ConnectionString;
            SqlConnection connection = new SqlConnection(connectionString);
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable table = new DataTable();
            SqlConnection sqlconn = new SqlConnection(connectionString);
            //подключение бд
            SqlCommand sqlcomm = new SqlCommand("UpdateStatusOrd4", sqlconn);
            //в SqlCommand задаем процедуру
            sqlcomm.CommandType = System.Data.CommandType.StoredProcedure;
            sqlcomm.Parameters.Add("@k_ord", System.Data.SqlDbType.Int).Value = n;
            //задаем значения процедур
            SqlDataAdapter sqladap = new SqlDataAdapter(sqlcomm);
            System.Data.DataTable dtable = new System.Data.DataTable();
            sqladap.Fill(dtable);
            Manager.MainFrame.Navigate(new PageOrdForPovar());
        }
            catch (System.NullReferenceException) { MessageBox.Show("Выберите строку в таблице."); }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
        }
    }
}
