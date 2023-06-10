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
using static System.Net.Mime.MediaTypeNames;


namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для PageWork.xaml
    /// </summary>
    public partial class PageWork : Page
    {
        string connectionString;
        DataTable dt = new DataTable();
        DataTable dt2 = new DataTable();
        SqlDataAdapter adapter;
        SqlDataAdapter adapter2;
        public PageWork()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["RestoranConnectionString"].ConnectionString;
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cm = new SqlCommand("select * from works", connection);
            adapter = new SqlDataAdapter(cm);
            SqlCommand cm2 = new SqlCommand("select distinct post from works", connection);
            adapter2 = new SqlDataAdapter(cm2);
            connection.Open();
            adapter.Fill(dt);
            adapter2.Fill(dt2);
            combo.ItemsSource = dt2.DefaultView;
            combo.DisplayMemberPath = "post";
            dataGrid1.ItemsSource = dt.DefaultView;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddPageWork());
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                DataRowView rowView = dataGrid1.SelectedValue as DataRowView;
                    t1.Text = rowView[1].ToString();
                    t2.Text = rowView[2].ToString();
                    t3.Text = rowView[3].ToString();
                    t4.Text = rowView[4].ToString();
                    t5.Text = rowView[5].ToString();
                t6.Text = rowView[6].ToString();
                can.Visibility = Visibility.Hidden;
                    can2.Visibility = Visibility.Visible;
            

            }
            catch (System.NullReferenceException) { MessageBox.Show("Выберите строку для изменения."); }
            catch (Exception) { MessageBox.Show("Проверьте подключение"); }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {
                if (t1.Text == "" || t2.Text == "" || t3.Text == "" || t4.Text == "" || t5.Text == "" || t6.Text == "")
                {
                    MessageBox.Show("Заполните все поля в соответствии с форматом!");
                }
                else
                {
                    DataRowView rowView = dataGrid1.SelectedValue as DataRowView;
                    int kod = Convert.ToInt32(rowView[0]);

                    SqlConnection sqlconn = new SqlConnection(connectionString);
                    //подключение бд
                    SqlCommand sqlcomm = new SqlCommand("EditWork", sqlconn);
                    //в SqlCommand задаем процедуру
                    sqlcomm.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlcomm.Parameters.Add("@kod", System.Data.SqlDbType.Int).Value = kod;
                    sqlcomm.Parameters.Add("@fam", System.Data.SqlDbType.VarChar).Value = t1.Text;
                    sqlcomm.Parameters.Add("@nam", System.Data.SqlDbType.VarChar).Value = t2.Text;
                    sqlcomm.Parameters.Add("@mid", System.Data.SqlDbType.VarChar).Value = t3.Text;
                    sqlcomm.Parameters.Add("@post", System.Data.SqlDbType.VarChar).Value = t4.Text;
                    sqlcomm.Parameters.Add("@num", System.Data.SqlDbType.VarChar).Value = t5.Text;
                    sqlcomm.Parameters.Add("@pass", System.Data.SqlDbType.VarChar).Value = t6.Text;
                    //задаем значения процедур
                    SqlDataAdapter sqladap = new SqlDataAdapter(sqlcomm);
                    System.Data.DataTable dtable = new System.Data.DataTable();
                    sqladap.Fill(dtable);
                    //помещаем результат процедуры
                    MessageBox.Show("Изменение завершено.");
                    Manager.MainFrame.Navigate(new PageWork());
                    dataGrid1.ItemsSource = dt.DefaultView;
                    can.Visibility = Visibility.Visible;
                    can2.Visibility = Visibility.Hidden;
                }
            }
            catch (System.FormatException) { MessageBox.Show("Вы заполнили не все поля, либо заполнили некорректно. Заполните поля в соответствии с форматом таблицы."); }
            catch (System.Data.SqlClient.SqlException) { MessageBox.Show("Выберите сотрудника! Либо проверьте подключение."); }
            catch (Exception) { MessageBox.Show("Проверьте, все ли поля заполнены, либо проверьте подключение."); }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show($"Вы точно хотите снять этого сотрудника с должности? Это безвозвратное действие", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
               try
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
                    catch (System.Data.SqlClient.SqlException) { MessageBox.Show("Сотрудник снят с должности."); Manager.MainFrame.Navigate(new PageWork()); }

               }
               catch (System.Data.DBConcurrencyException) { MessageBox.Show("Введите первичный ключ ИЛИ перезайдите для выставления его автоматически"); dt.RejectChanges(); }
                 catch (Exception) { MessageBox.Show("Что-то не так. Попробуйте снова. Вы выбрали строку для удаления?"); dt.RejectChanges(); }
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (TextFilter.Text.Length != 0)
                    dt.DefaultView.RowFilter = string.Format("[sur_w] LIKE '%{0}%'", TextFilter.Text);
                else dt.DefaultView.RowFilter = "";
            }
            catch (FormatException) { MessageBox.Show("Проверьте правильность введенных данных"); }
            catch (OverflowException) { MessageBox.Show("Переполнение"); }
            catch (Exception) { MessageBox.Show("Что-то пошло не так"); }
        }

        private void ComboBox_DropDownClosed(object sender, EventArgs e)
        {
            try
            {
                if (combo.Text != "")
                {
                    dt.DefaultView.RowFilter = string.Format("[post] = '{0}' ", combo.Text);
                }
                else dt.DefaultView.RowFilter = "";
            }
            catch (Exception) { MessageBox.Show("Что-то пошло не так"); }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {

            try
            {
                dt.DefaultView.RowFilter = "";
            }
            catch (Exception) { MessageBox.Show("Что-то пошло не так"); }
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            SumCheck s = new SumCheck();
            s.Show();
        }
    }
}
