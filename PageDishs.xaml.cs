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
using static System.Net.Mime.MediaTypeNames;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для PageDishs.xaml
    /// </summary>
    public partial class PageDishs : Page
    {
        string connectionString;
        DataTable dt = new DataTable();
        DataTable dt2 = new DataTable();
        SqlDataAdapter adapter;
        SqlDataAdapter adapter2;
        public PageDishs(int x)
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["RestoranConnectionString"].ConnectionString;
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cm = new SqlCommand("SELECT dishs.k_dish,n_dish,STRING_AGG(n_ing,', ') as n_ing ,desc_dish, cost_dish, calor,weight_dish,time_dish,pic from ing_dishs left join dishs on dishs.k_dish = ing_dishs.k_dish  left join  ing on  ing.k_ing = ing_dishs.k_ing  GROUP BY dishs.k_dish,n_dish, desc_dish, cost_dish, calor,weight_dish,time_dish,pic ORDER BY n_dish ASC", connection);
            adapter = new SqlDataAdapter(cm);
            connection.Open();
            adapter.Fill(dt);
            LV.ItemsSource = dt.DefaultView;
            if (x == 0)
            {
                del.IsEnabled = true;
                add.IsEnabled = true;
                add2.IsEnabled = true;
            }
            else
            {
                del.Background = Brushes.DimGray;
                add.Background = Brushes.DimGray;
                add2.Background = Brushes.DimGray;
                del.IsEnabled = false;
                add.IsEnabled = false;
                add2.IsEnabled = false;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show($"Вы точно хотите удалить этого клиента из списка? Это безвозвратное действие", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                { 
                    try
                
                    {
                        if (LV.SelectedItems != null)
                        {
                            for (int i = 0; i < LV.SelectedItems.Count; i++)
                            {
                                DataRowView dataRowView = LV.SelectedItems[i] as DataRowView;
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
                    catch (System.Data.SqlClient.SqlException) { MessageBox.Show("Удалять можно только случайно добавленные блюда, которые не были задействованы в заказах"); dt.RejectChanges(); }

                }
                catch (System.Data.DBConcurrencyException) { MessageBox.Show("Введите первичный ключ ИЛИ перезайдите для выставления его автоматически"); dt.RejectChanges(); }
                catch (Exception) { MessageBox.Show("Что-то не так. Попробуйте снова. Вы выбрали строку для удаления?"); dt.RejectChanges(); }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddDishPage());

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            t7.Visibility = Visibility.Visible;
            b1.Visibility = Visibility.Visible;
            b2.Visibility = Visibility.Visible;


        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            if (dlg.ShowDialog() == true && !string.IsNullOrWhiteSpace(dlg.FileName))
                t7.Text = dlg.FileName.ToString();
            t7.Focus();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
           
            if (t7.Text != "")
            {
                DataRowView rowView = LV.SelectedValue as DataRowView;
                string x = rowView[0].ToString();
                string connectionString;
                connectionString = ConfigurationManager.ConnectionStrings["RestoranConnectionString"].ConnectionString;
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand sqlcomm = new SqlCommand("UpdatePicProc", connection);
                //в SqlCommand задаем процедуру
                sqlcomm.CommandType = System.Data.CommandType.StoredProcedure;
                sqlcomm.Parameters.Add("@img", System.Data.SqlDbType.VarChar).Value = t7.Text;
                sqlcomm.Parameters.Add("@kod", System.Data.SqlDbType.Int).Value = x;
                SqlDataAdapter sqladap = new SqlDataAdapter(sqlcomm);
                System.Data.DataTable dtable = new System.Data.DataTable();
                sqladap.Fill(dtable);
                Manager.MainFrame.Navigate(new PageDishs(0));

            }
            else
            {
                MessageBox.Show("Выберите фотографию");
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (TextFilter.Text.Length != 0)
                    dt.DefaultView.RowFilter = string.Format("[n_dish] LIKE '%{0}%'", TextFilter.Text);
                else dt.DefaultView.RowFilter = "";
            }
            catch (FormatException) { MessageBox.Show("Проверьте правильность введенных данных"); }
            catch (OverflowException) { MessageBox.Show("Переполнение"); }
            catch (Exception) { MessageBox.Show("Что-то пошло не так"); }
        }
    }
}
