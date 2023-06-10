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
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для AddOrdsPage.xaml
    /// </summary>
    public partial class AddOrdsPage : Page
    {
        string connectionString;
        DataTable dt = new DataTable();
        DataTable dt2 = new DataTable();
        DataTable dt3 = new DataTable();
        SqlDataAdapter adapter;
        SqlDataAdapter adapter2;
        SqlDataAdapter adapter3;

        public AddOrdsPage()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["RestoranConnectionString"].ConnectionString;
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cm = new SqlCommand("select * from tab ORDER BY location ASC", connection);
            adapter = new SqlDataAdapter(cm);
            SqlCommand cm2 = new SqlCommand("select n_dish from dishs ORDER BY n_dish ASC", connection);
            adapter2 = new SqlDataAdapter(cm2);
            SqlCommand cm3 = new SqlCommand("select sur_w from works ORDER BY sur_w ASC", connection);
            adapter3 = new SqlDataAdapter(cm3);

            connection.Open();
            adapter.Fill(dt);
            adapter2.Fill(dt2);
            adapter3.Fill(dt3);
            dataGrid2.ItemsSource = dt.DefaultView;
            dataGrid1.ItemsSource = dt2.DefaultView;
            combo.ItemsSource = dt3.DefaultView;
            combo.DisplayMemberPath = "sur_w";


        }

        private void Далее_Click(object sender, RoutedEventArgs e)
        {
            try {
                if (combo.Text != "")
                {
                    DataRowView rowView = dataGrid2.SelectedValue as DataRowView;
                    int k = Convert.ToInt32(rowView[0].ToString());
                    var td = DateTime.Now;
                    string status = "Передан на кухню";

                    string connectionString;
                    connectionString = ConfigurationManager.ConnectionStrings["RestoranConnectionString"].ConnectionString;
                    SqlConnection connection = new SqlConnection(connectionString);
                    string sqlExpression = "AddOrd1";
                    connection.Open();
                    SqlCommand command = new SqlCommand(sqlExpression, connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    //в SqlCommand задаем процедуру
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add("@status_ord", System.Data.SqlDbType.NVarChar).Value = status;
                    command.Parameters.Add("@k_tab", System.Data.SqlDbType.Int).Value = k;
                    command.Parameters.Add("@time_ord", System.Data.SqlDbType.DateTime).Value = td;
                    command.Parameters.Add("@cost_ord", System.Data.SqlDbType.Int).Value = 0;
                    command.Parameters.Add("@sur_w", System.Data.SqlDbType.NVarChar).Value = combo.Text;
                    Manager.kodDish = command.ExecuteScalar().ToString();
                    can2.Visibility = Visibility.Hidden;
                    can1.Visibility = Visibility.Visible;
                }
                else
                {
                    MessageBox.Show("Выберите сотрудника (себя)");
                }
            }
            catch (System.NullReferenceException) { MessageBox.Show("Выберите строку в таблице."); }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            can2.Visibility = Visibility.Visible;
            can1.Visibility = Visibility.Hidden;
        }

        private void dataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                DataRowView rowView = dataGrid1.SelectedValue as DataRowView;
                string n = rowView[0].ToString();
                string connectionString;
                connectionString = ConfigurationManager.ConnectionStrings["RestoranConnectionString"].ConnectionString;
                SqlConnection connection = new SqlConnection(connectionString);
                SqlDataAdapter adapter = new SqlDataAdapter();
                DataTable table = new DataTable();
                SqlConnection sqlconn = new SqlConnection(connectionString);
                //подключение бд
                SqlCommand sqlcomm = new SqlCommand("AddDishsOrd1", sqlconn);
                //в SqlCommand задаем процедуру
                sqlcomm.CommandType = System.Data.CommandType.StoredProcedure;
                sqlcomm.Parameters.Add("@k_ord", System.Data.SqlDbType.Int).Value = Manager.kodDish;
                sqlcomm.Parameters.Add("@n_dish", System.Data.SqlDbType.VarChar).Value = n;
                //задаем значения процедур
                SqlDataAdapter sqladap = new SqlDataAdapter(sqlcomm);
                System.Data.DataTable dtable = new System.Data.DataTable();
                sqladap.Fill(dtable);
                lb.Items.Add($"{n}");
            }
            catch (System.NullReferenceException) { MessageBox.Show("Выберите строку в таблице."); }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                string name = lb.SelectedItem.ToString();
            int x = lb.SelectedIndex;
            string connectionString;
            connectionString = ConfigurationManager.ConnectionStrings["RestoranConnectionString"].ConnectionString;
            SqlConnection connection = new SqlConnection(connectionString);
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable table = new DataTable();
            SqlConnection sqlconn = new SqlConnection(connectionString);
            //подключение бд
            SqlCommand sqlcomm = new SqlCommand("DeleteDishsOrd", sqlconn);
            //в SqlCommand задаем процедуру
            sqlcomm.CommandType = System.Data.CommandType.StoredProcedure;
            sqlcomm.Parameters.Add("@k_ord", System.Data.SqlDbType.Int).Value = Manager.kodDish;
            sqlcomm.Parameters.Add("@n_dish", System.Data.SqlDbType.VarChar).Value = name;
            //задаем значения процедур
            SqlDataAdapter sqladap = new SqlDataAdapter(sqlcomm);
            System.Data.DataTable dtable = new System.Data.DataTable();
            sqladap.Fill(dtable);
            lb.Items.RemoveAt(x);
            }
            catch (System.NullReferenceException) { MessageBox.Show("Выберите строку в таблице."); }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
        }
    }
}
