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
    /// Логика взаимодействия для PageTab.xaml
    /// </summary>
    public partial class PageTab : Page
    {
        string connectionString;
        DataTable dt = new DataTable();
        DataTable dt2 = new DataTable();
        SqlDataAdapter adapter;
        SqlDataAdapter adapter2;
        public PageTab()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["RestoranConnectionString"].ConnectionString;
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cm = new SqlCommand("select * from tab", connection);
            adapter = new SqlDataAdapter(cm);
            SqlCommand cm2 = new SqlCommand("select distinct location from tab", connection);
            adapter2 = new SqlDataAdapter(cm2);
            connection.Open();
            adapter.Fill(dt);
            adapter2.Fill(dt2);
            combo.ItemsSource = dt2.DefaultView;
            combo.DisplayMemberPath = "location";
            dataGrid1.ItemsSource = dt.DefaultView;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dataGrid1.SelectedItems != null)
                {
                    for (int i = 0; i < dataGrid1.SelectedItems.Count; i++)
                    {
                        DataRowView dataRowView = dataGrid1.SelectedItems[i] as DataRowView;
                        if (dataRowView != null)
                        {
                            DataRow dataRow = (DataRow)dataRowView.Row;
                            dataRow.Delete();
                        }
                    }
                    SqlCommandBuilder commandbuilder = new SqlCommandBuilder(adapter);
                    adapter.Update(dt);
                }

            }
            catch (System.Data.SqlClient.SqlException) { MessageBox.Show("Удалять можно только столы никогда не состоящие на брони."); dt.RejectChanges(); }
            catch (Exception) { MessageBox.Show("Что-то не так. Попробуйте снова. Возможно необходимо вставить первичный ключ, для автоматического выставления - перезайдите в окно."); dt.RejectChanges(); }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                SqlCommandBuilder commandbuilder = new SqlCommandBuilder(adapter);
                adapter.Update(dt);
                MessageBox.Show("Данные обновлены.");
            }
            catch (Exception) { MessageBox.Show("Попробуйте снова."); dt.RejectChanges(); }
        }

        private void combo_DropDownClosed(object sender, EventArgs e)
        {

            try
            {
                if (combo.Text != "")
                {
                    dt.DefaultView.RowFilter = string.Format("[location] = '{0}' ", combo.Text);
                }
                else dt.DefaultView.RowFilter = "";
            }

            catch (Exception) { MessageBox.Show("Что-то пошло не так"); }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {
                dt.DefaultView.RowFilter = "";
            }
            catch (Exception) { MessageBox.Show("Что-то пошло не так"); }
        }
    }
}
