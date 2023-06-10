using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для AddDishPage.xaml
    /// </summary>
    public partial class AddDishPage : Page
    {
        string connectionString;
        DataTable dt = new DataTable();
        DataTable dt2 = new DataTable();
        SqlDataAdapter adapter;
        SqlDataAdapter adapter2;
        public AddDishPage()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["RestoranConnectionString"].ConnectionString;
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cm = new SqlCommand("select n_ing from ing", connection);
            adapter = new SqlDataAdapter(cm);
            connection.Open();
            adapter.Fill(dt);
            dataGrid1.ItemsSource = dt.DefaultView;

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            if (dlg.ShowDialog() == true && !string.IsNullOrWhiteSpace(dlg.FileName))
                t7.Text = dlg.FileName.ToString();
            t7.Focus();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            
                try
                {
                if (MessageBox.Show($"Вы внесли всю необходимую информацию о блюде? Следующий шаг - добавление списка ингредиентов", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    string connectionString;
                    connectionString = ConfigurationManager.ConnectionStrings["RestoranConnectionString"].ConnectionString;
                    SqlConnection connection = new SqlConnection(connectionString);
                    string sqlExpression = "AddDishProc";
                    connection.Open();
                    SqlCommand command = new SqlCommand(sqlExpression, connection);
                    if (t7.Text == "")
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.Add("@n_dish", System.Data.SqlDbType.VarChar).Value = t1.Text;
                        command.Parameters.Add("@desc_dish", System.Data.SqlDbType.VarChar).Value = t2.Text;
                        command.Parameters.Add("@cost_dish", System.Data.SqlDbType.Int).Value = t4.Text;
                        command.Parameters.Add("@calor", System.Data.SqlDbType.Int).Value = t5.Text;
                        command.Parameters.Add("@weght_dish", System.Data.SqlDbType.Int).Value = t6.Text;
                        command.Parameters.Add("@time_dish", System.Data.SqlDbType.Int).Value = t3.Text;
                        Manager.kodDish = command.ExecuteScalar().ToString();
                    }
                    else
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.Add("@n_dish", System.Data.SqlDbType.VarChar).Value = t1.Text;
                        command.Parameters.Add("@desc_dish", System.Data.SqlDbType.VarChar).Value = t2.Text;
                        command.Parameters.Add("@cost_dish", System.Data.SqlDbType.Int).Value = t4.Text;
                        command.Parameters.Add("@calor", System.Data.SqlDbType.Int).Value = t5.Text;
                        command.Parameters.Add("@weght_dish", System.Data.SqlDbType.Int).Value = t6.Text;
                        command.Parameters.Add("@time_dish", System.Data.SqlDbType.Int).Value = t3.Text;
                        command.Parameters.Add("@pic", System.Data.SqlDbType.VarChar).Value = t7.Text;
                        Manager.kodDish = command.ExecuteScalar().ToString();
                    }
                    can.Visibility = Visibility.Hidden;
                    can2.Visibility = Visibility.Visible;
                    t11.Text = t1.Text;
                    
                }
                }
                catch (System.FormatException)
                {
                    MessageBox.Show($"Заполните ВСЕ поля в соответствии с форматом: стоимость, калорийность, вес и время должны содержать только цифры.");
                }
            
            

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {
                SqlCommandBuilder commandbuilder = new SqlCommandBuilder(adapter);
                adapter.Update(dt);
                MessageBox.Show("Данные обновлены.");
            }
            catch (Exception) { MessageBox.Show("Попробуйте снова."); dt.RejectChanges(); }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
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
            SqlCommand sqlcomm = new SqlCommand("AddIngProc", sqlconn);
            //в SqlCommand задаем процедуру
            sqlcomm.CommandType = System.Data.CommandType.StoredProcedure;
            sqlcomm.Parameters.Add("@k_dish", System.Data.SqlDbType.Int).Value = Manager.kodDish;
            sqlcomm.Parameters.Add("@n_ing", System.Data.SqlDbType.VarChar).Value = n;
            //задаем значения процедур
            SqlDataAdapter sqladap = new SqlDataAdapter(sqlcomm);
            System.Data.DataTable dtable = new System.Data.DataTable();
            sqladap.Fill(dtable);
            lb.Items.Add($"{n}");
            }
            catch (System.NullReferenceException) { MessageBox.Show("Выберите строку в таблице."); }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }

        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
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
            SqlCommand sqlcomm = new SqlCommand("DeleteIngDishProc", sqlconn);
            //в SqlCommand задаем процедуру
            sqlcomm.CommandType = System.Data.CommandType.StoredProcedure;
            sqlcomm.Parameters.Add("@k_dish", System.Data.SqlDbType.Int).Value = Manager.kodDish;
            sqlcomm.Parameters.Add("@n_ing", System.Data.SqlDbType.VarChar).Value = name;
            //задаем значения процедур
            SqlDataAdapter sqladap = new SqlDataAdapter(sqlcomm);
            System.Data.DataTable dtable = new System.Data.DataTable();
            sqladap.Fill(dtable);
            lb.Items.RemoveAt(x);
            }
            catch (System.NullReferenceException) { MessageBox.Show("Выберите строку в таблице."); }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new PageDishs(1));
        }
    }
}
