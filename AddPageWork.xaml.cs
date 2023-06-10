using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
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
using System.Configuration;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для AddPageWork.xaml
    /// </summary>
    public partial class AddPageWork : Page
    {
        string connectionString;
        DataTable dt = new DataTable();
        DataTable dt2 = new DataTable();
        SqlDataAdapter adapter;
        SqlDataAdapter adapter2;
        public AddPageWork()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["RestoranConnectionString"].ConnectionString;
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cm = new SqlCommand("select distinct post from works", connection);
            adapter = new SqlDataAdapter(cm);
            connection.Open();
            adapter.Fill(dt);
            combo.ItemsSource = dt.DefaultView;
            t1.MaxLength = 50;
            t2.MaxLength = 50;
            t3.MaxLength = 50;
            t4.MaxLength = 50;
            t5.MaxLength = 11;
            t6.MaxLength = 4;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (t1.Text == "" || t2.Text == "" || t3.Text == "" || t4.Text == "" || t5.Text == "")
                {
                    MessageBox.Show("Заполните все поля в соответствии с форматом!");
                }
                else
                {
                    string fam = t1.Text;
                    string name = t2.Text;
                    string mid = t3.Text;
                    string num = t4.Text;
                    string post = t5.Text;
                    string pass = t6.Text;
                        SqlConnection sqlconn = new SqlConnection(connectionString);
                        //подключение бд
                        SqlCommand sqlcomm = new SqlCommand("TestProcedure1", sqlconn);
                        //в SqlCommand задаем процедуру
                        sqlcomm.CommandType = System.Data.CommandType.StoredProcedure;
                        sqlcomm.Parameters.Add("@sur_w", System.Data.SqlDbType.VarChar).Value = fam;
                        sqlcomm.Parameters.Add("@name_w", System.Data.SqlDbType.VarChar).Value = name;
                    sqlcomm.Parameters.Add("@mid_w", System.Data.SqlDbType.VarChar).Value = mid;
                    sqlcomm.Parameters.Add("@num_w", System.Data.SqlDbType.VarChar).Value = num;
                    sqlcomm.Parameters.Add("@post", System.Data.SqlDbType.VarChar).Value = post;
                    sqlcomm.Parameters.Add("@pass", System.Data.SqlDbType.VarChar).Value = pass;
                        //задаем значения процедур
                        SqlDataAdapter sqladap = new SqlDataAdapter(sqlcomm);
                        System.Data.DataTable dtable = new System.Data.DataTable();
                        sqladap.Fill(dtable);
                    //помещаем результат процедуры
                    Manager.MainFrame.Navigate(new PageWork());
                    MessageBox.Show("Добавление завершено.");
                    
                }
            }

            catch (System.FormatException) { MessageBox.Show("Заполните все поля в соответствии с форматом!"); }
            catch (System.InvalidOperationException) { MessageBox.Show("Введите дату и время"); }
            catch (System.Data.SqlClient.SqlException) { MessageBox.Show("Вы не можете добавить запись на прошедшее время, либо стол уже занят."); }

        }

        private void combo_DropDownClosed(object sender, EventArgs e)
        {
            t5.Text = combo.Text;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
