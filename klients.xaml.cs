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
    /// Логика взаимодействия для klients.xaml
    /// </summary>
    public partial class klients : Window
    {
        string connectionString;
        DataTable dt = new DataTable();
        SqlDataAdapter adapter;
        public klients()
        {
            InitializeComponent();
            try { 
            connectionString = ConfigurationManager.ConnectionStrings["RestoranConnectionString"].ConnectionString;
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cm = new SqlCommand("SELECT TOP 3 fio_res,num_res FROM res GROUP BY fio_res, num_res ORDER BY COUNT(fio_res) DESC", connection);
            adapter = new SqlDataAdapter(cm);
            connection.Open();
            adapter.Fill(dt);
            dataGrid1.ItemsSource = dt.DefaultView;
        }
        catch (Exception ex) { MessageBox.Show(ex.ToString()); }
}
    }
}
