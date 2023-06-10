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
using System.Windows.Shapes;
using System.Configuration;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для SumCheck.xaml
    /// </summary>
    public partial class SumCheck : Window
    {
        string connectionString;
        DataTable dt = new DataTable();
        DataTable dt2 = new DataTable();
        SqlDataAdapter adapter;
        SqlDataAdapter adapter2;
        public SumCheck()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["RestoranConnectionString"].ConnectionString;
            SqlConnection connection = new SqlConnection(connectionString);
            combo.Items.Add("Январь");
            combo.Items.Add("Февраль");
            combo.Items.Add("Март");
            combo.Items.Add("Апрель");
            combo.Items.Add("Май");
            combo.Items.Add("Июнь");
            combo.Items.Add("Июль");
            combo.Items.Add("Август");
            combo.Items.Add("Сентябрь");
            combo.Items.Add("Октябрь");
            combo.Items.Add("Ноябрь");
            combo.Items.Add("Декабрь");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection sqlconn = new SqlConnection(connectionString);
            //подключение бд
            SqlCommand sqlcomm = new SqlCommand("SumCheckWork2", sqlconn);
            //в SqlCommand задаем процедуру
            sqlcomm.CommandType = System.Data.CommandType.StoredProcedure;
            sqlcomm.Parameters.Add("@mn", System.Data.SqlDbType.VarChar).Value = combo.Text;
            //задаем значения процедур
            SqlDataAdapter sqladap = new SqlDataAdapter(sqlcomm);
            System.Data.DataTable dtable = new System.Data.DataTable();
            sqladap.Fill(dtable);
            //помещаем результат процедуры
            dataGrid1.ItemsSource = dtable.DefaultView; //выводим результат
          //  dataGrid1.ItemsSource = dt.DefaultView;
        }
    }
}
