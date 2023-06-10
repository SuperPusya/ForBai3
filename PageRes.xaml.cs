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
using System.Windows.Threading;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Windows.Controls;
using System.Windows.Media;
using System.IO;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для PageRes.xaml
    /// </summary>
    public partial class PageRes : Page
    {
        string connectionString;
        DataTable dt = new DataTable();
        DataTable dt2 = new DataTable();
        SqlDataAdapter adapter;
        SqlDataAdapter adapter2;
        DataTable dt3 = new DataTable();
        SqlDataAdapter adapter3;
        string f = Manager.famil;
        public PageRes(int x)
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["RestoranConnectionString"].ConnectionString;
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cm = new SqlCommand("select k_res,fio_res,num_res,tab.k_tab,location,time_res,sur_w,status from res,works,tab where res.k_tab=tab.k_tab and res.k_w=works.k_w", connection);
            adapter = new SqlDataAdapter(cm);
            SqlCommand cm2 = new SqlCommand("select distinct status from res", connection);
            adapter2 = new SqlDataAdapter(cm2);
            connection.Open();
            adapter.Fill(dt);
            adapter2.Fill(dt2);
            combo.ItemsSource = dt2.DefaultView;
            combo.DisplayMemberPath = "status";
            dataGrid1.ItemsSource = dt.DefaultView;
            t1.MaxLength = 150;
            t2.MaxLength = 11;
            t3.MaxLength = 3;
            t5.MaxLength = 50;
            if (x == 0)
            {
                b1.IsEnabled = true;
                b2.IsEnabled = true;
                b3.IsEnabled = true;
            }
            else {
                b1.Background = Brushes.DimGray;
                b2.Background = Brushes.DimGray;
                b3.Background = Brushes.DimGray;
                b1.IsEnabled = false;
                b2.IsEnabled = false;
                b3.IsEnabled = false;
            }


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
                connectionString = ConfigurationManager.ConnectionStrings["RestoranConnectionString"].ConnectionString;
                SqlConnection connection = new SqlConnection(connectionString);
                SqlCommand cm = new SqlCommand("select k_res,fio_res,num_res,tab.k_tab,location,time_res,sur_w,status from res,works,tab where res.k_tab=tab.k_tab and res.k_w=works.k_w", connection);
                adapter = new SqlDataAdapter(cm);
                connection.Open();
                adapter.Fill(dt);
                dataGrid1.ItemsSource = dt.DefaultView;
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
        }

        private void combo_DropDownClosed(object sender, EventArgs e)
        {
            try
            {
                if (combo.Text != "")
                {
                    dt.DefaultView.RowFilter = string.Format("[status] = '{0}' ", combo.Text);
                }
                else dt.DefaultView.RowFilter = "";
            }
            catch (Exception) { MessageBox.Show("Что-то пошло не так"); }
        }

        private void Button_Click(object sender, RoutedEventArgs e) // Удаление
        {
            try {
                DataRowView rowView = dataGrid1.SelectedValue as DataRowView;
                int kod = Convert.ToInt16(rowView[0].ToString());

                SqlConnection sqlconn = new SqlConnection(connectionString);
                //подключение бд
                SqlCommand sqlcomm = new SqlCommand("DeleteRes", sqlconn);
                //в SqlCommand задаем процедуру
                sqlcomm.CommandType = System.Data.CommandType.StoredProcedure;
                sqlcomm.Parameters.Add("@k1", System.Data.SqlDbType.Int).Value = kod;

                //задаем значения процедур


                SqlDataAdapter sqladap = new SqlDataAdapter(sqlcomm);
                System.Data.DataTable dtable = new System.Data.DataTable();
                sqladap.Fill(dtable);
                //помещаем результат процедуры
                dataGrid1.ItemsSource = dtable.DefaultView; //выводим результат
                MessageBox.Show("Удаление завершено. Перезайдите на страницу");
                Manager.MainFrame.Navigate(new PageRes(0));
            }
            catch (System.NullReferenceException) { MessageBox.Show("Выберите строку."); }
            catch (Exception) { MessageBox.Show("Что-то не так проверьте подключение."); }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e) // Изменение
        {
            try
            {
                DataRowView rowView = dataGrid1.SelectedValue as DataRowView;
                int kod = Convert.ToInt32(rowView[0]);

                SqlConnection sqlconn = new SqlConnection(connectionString);
                //подключение бд
                SqlCommand sqlcomm = new SqlCommand("EditRes", sqlconn);
                //в SqlCommand задаем процедуру
                sqlcomm.CommandType = System.Data.CommandType.StoredProcedure;
                sqlcomm.Parameters.Add("@kod", System.Data.SqlDbType.Int).Value = kod;
                sqlcomm.Parameters.Add("@fio", System.Data.SqlDbType.VarChar).Value = t1.Text;
                sqlcomm.Parameters.Add("@num", System.Data.SqlDbType.VarChar).Value = t2.Text;
                sqlcomm.Parameters.Add("@tab", System.Data.SqlDbType.Int).Value = t3.Text;
                sqlcomm.Parameters.Add("@time_res", System.Data.SqlDbType.DateTime).Value = Convert.ToDateTime(t4.Text);
                sqlcomm.Parameters.Add("@status", System.Data.SqlDbType.VarChar).Value = t5.Text;
                sqlcomm.Parameters.Add("@fam", System.Data.SqlDbType.VarChar).Value = f;
                //задаем значения процедур
                SqlDataAdapter sqladap = new SqlDataAdapter(sqlcomm);
                System.Data.DataTable dtable = new System.Data.DataTable();
                sqladap.Fill(dtable);
                //помещаем результат процедуры
                dataGrid1.ItemsSource = dtable.DefaultView; //выводим результат
                MessageBox.Show("Изменение завершено. Перезайдите на страницу");
                Manager.MainFrame.Navigate(new PageRes(0));
                can2.Visibility = Visibility.Visible;
                can.Visibility = Visibility.Hidden;
            }
            catch (System.FormatException) { MessageBox.Show("Вы заполнили не все поля, либо заполнили некорректно. Заполните поля в соответствии с форматом таблицы."); }
            catch (System.Data.SqlClient.SqlException) { MessageBox.Show("Выберите сотрудника! Либо проверьте подключение."); }
            catch (Exception) { MessageBox.Show("Проверьте, все ли поля заполнены, либо проверьте подключение."); }

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {
                DataRowView rowView = dataGrid1.SelectedValue as DataRowView;
                string proverka = rowView[6].ToString();
                if (proverka == f)
                {
                    t1.Text = rowView[1].ToString();
                    t2.Text = rowView[2].ToString();
                    t3.Text = rowView[3].ToString();
                    t4.Text = rowView[5].ToString();
                    t5.Text = rowView[7].ToString();
                    can2.Visibility = Visibility.Hidden;
                    can.Visibility = Visibility.Visible;
                }
                else
                {
                    MessageBox.Show("Вы можете изменять только свои записи");
                }

            }
            catch (System.NullReferenceException) { MessageBox.Show("Выберите строку для изменения."); }
            catch (Exception) { MessageBox.Show("Проверьте подключение"); }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (TextFilter.Text.Length != 0)
                    dt.DefaultView.RowFilter = string.Format("[fio_res] LIKE '%{0}%'", TextFilter.Text);
                else dt.DefaultView.RowFilter = "";
            }
            catch (FormatException) { MessageBox.Show("Проверьте правильность введенных данных"); }
            catch (OverflowException) { MessageBox.Show("Переполнение"); }
            catch (Exception) { MessageBox.Show("Что-то пошло не так"); }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            try
            {
                dt.DefaultView.RowFilter = "";
            }
            catch (Exception) { MessageBox.Show("Что-то пошло не так"); }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e) // Записи на сеголня
        {
            DateTime td = DateTime.Today;
            string today2 = td.ToString("dd/MM/yyyy") + " 00:00:00";
            string today3 = td.ToString("dd/MM/yyyy") + " 23:59:00";

            try
            {
                dt.DefaultView.RowFilter = string.Format("[time_res] >= '{0}' and [time_res] <= '{1}'", today2, today3);
            }
            catch (FormatException) { MessageBox.Show("Проверьте правильность введенных данных"); }
            catch (OverflowException) { MessageBox.Show("Переполнение"); }
            catch (Exception) { MessageBox.Show("Что-то пошло не так"); }
        }

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime? selectedDate = dp.SelectedDate;
            if (selectedDate.HasValue)
            {
                string formatted = selectedDate.Value.ToString("dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture);
                string today2 = formatted + " 00:00:00";
                string today3 = formatted + " 23:59:00";
                try
                {
                    dt.DefaultView.RowFilter = string.Format("[time_res] >= '{0}' and [time_res] <= '{1}'", today2, today3);
                }
                catch (FormatException) { MessageBox.Show("Проверьте правильность введенных данных"); }
                catch (OverflowException) { MessageBox.Show("Переполнение"); }
                catch (Exception) { MessageBox.Show("Что-то пошло не так"); }
            }

        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            DateTime td = DateTime.Today;

            Manager.MainFrame.Navigate(new AddResPage(td.ToString(), "12:00", 1));

        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            klients k = new klients();
            k.Show();
        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            try
            {
                Document document = new Document();
                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream("../../Document/sample.pdf", FileMode.Create));
                document.Open();
                /*    var logo = iTextSharp.text.Image.GetInstance(new FileStream(@"..\..\picter\i3.png", FileMode.Open));
                    logo.SetAbsolutePosition(440, 758);
                    writer.DirectContent.AddImage(logo); */
                BaseFont baseFont = BaseFont.CreateFont("C:\\Windows\\Fonts\\arial.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                iTextSharp.text.Font font = new iTextSharp.text.Font(baseFont, iTextSharp.text.Font.DEFAULTSIZE, iTextSharp.text.Font.NORMAL);
                PdfPTable table = new PdfPTable(dataGrid1.Columns.Count);
                PdfPCell cell = new PdfPCell(new Phrase("Бронирование", font));
                cell.Colspan = dataGrid1.Columns.Count;
                cell.HorizontalAlignment = 1;
                cell.Border = 0;
                table.AddCell(cell);
                string[] name = { "Код", "ФИО", "Номер", "Стол", "Расположение", "Дата", "Сотрудник", "Статус" };
                for (int j = 0; j < dataGrid1.Columns.Count; j++)
                {
                    cell = new PdfPCell(new Phrase(name[j], font));
                    cell.BackgroundColor = iTextSharp.text.BaseColor.PINK;
                    table.AddCell(cell);
                }
                for (int j = 0; j < dataGrid1.Items.Count; j++)
                {
                    for (int i = 0; i < dataGrid1.Columns.Count; i++)
                    {
                        TextBlock b = dataGrid1.Columns[i].GetCellContent(dataGrid1.Items[j]) as TextBlock;
                        if (b != null)
                        {
                            table.AddCell(new Phrase(b.Text, font));
                        }
                        else
                        {
                            table.AddCell(new Phrase("", font)); // или добавить значение по умолчанию
                        }
                    }
                }
                document.Add(table);
                document.Close();
                MessageBox.Show("Файл создан в папке Document");
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }


        } 

    }
}

