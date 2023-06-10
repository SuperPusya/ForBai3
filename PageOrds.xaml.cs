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
using System.Windows.Threading;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Printing;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;


namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для PageOrds.xaml
    /// </summary>
    public partial class PageOrds : Page
    {
        string connectionString;
        DataTable dt = new DataTable();
        DataTable dt2 = new DataTable();
        DataTable dt3 = new DataTable();
        SqlDataAdapter adapter;
        SqlDataAdapter adapter2;
        SqlDataAdapter adapter3;

        public PageOrds(int x)
        {
            InitializeComponent();

            if (x == 0)
            {
                bOp3.Visibility = Visibility.Hidden;
                bOp4.Visibility = Visibility.Hidden;
                bOp1.Visibility = Visibility.Visible;
                bOp2.Visibility = Visibility.Visible;
            }
            else
            {
                bOp3.Visibility = Visibility.Hidden;
                bOp4.Visibility = Visibility.Hidden;
                bOp1.Visibility = Visibility.Hidden;
                bOp2.Visibility = Visibility.Hidden;
                dob.Visibility = Visibility.Hidden;
                pd.Visibility = Visibility.Visible;
                
            }

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

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            /* ОБНОВЛЕНИЕ ДАННЫХ ДОБАВИТЬ  ТАЙМЕР
            SqlCommandBuilder commandbuilder = new SqlCommandBuilder(adapter);
            adapter.Update(dt);
            dt.Clear();
            adapter.Fill(dt);
            dataGrid1.ItemsSource = dt.DefaultView; */

        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
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
                SqlCommand sqlcomm = new SqlCommand("UpdateStatusOrd2", sqlconn);
                //в SqlCommand задаем процедуру
                sqlcomm.CommandType = System.Data.CommandType.StoredProcedure;
                sqlcomm.Parameters.Add("@k_ord", System.Data.SqlDbType.Int).Value = n;
                //задаем значения процедур
                SqlDataAdapter sqladap = new SqlDataAdapter(sqlcomm);
                System.Data.DataTable dtable = new System.Data.DataTable();
                sqladap.Fill(dtable);
                Manager.MainFrame.Navigate(new PageOrds(0));



                DataRowView row = (DataRowView)dataGrid1.SelectedItem;

                // Извлечь данные, которые вам нужны, из объекта DataRowView
                string sur_w = row["sur_w"].ToString();
                string dishes = row["dishes"].ToString();
                string time_ord = row["time_ord"].ToString();
                int k_tab = Convert.ToInt32(row["k_tab"]);

                decimal cost_ord = Convert.ToDecimal(row["cost_ord"]);

                // Создать шаблон для чека, используя классы FlowDocument и TextBlock, и заполнить его данными
                FlowDocument doc = new FlowDocument();

                System.Windows.Documents.Paragraph p0 = new System.Windows.Documents.Paragraph();
                TextBlock tb0 = new TextBlock();
                tb0.FontSize = 30;
                tb0.Text = "________________________________________________________";
                p0.Inlines.Add(tb0);
                doc.Blocks.Add(p0);


                System.Windows.Documents.Paragraph p1 = new System.Windows.Documents.Paragraph();
                TextBlock tb1 = new TextBlock();
                tb1.FontSize = 15;
                tb1.Text = "Официант: " + sur_w;
                p1.Inlines.Add(tb1);
                doc.Blocks.Add(p1);

                System.Windows.Documents.Paragraph p2 = new System.Windows.Documents.Paragraph();
                TextBlock tb2 = new TextBlock();
                tb2.FontSize = 15;
                tb2.Text = "Блюда: " + dishes;
                p2.Inlines.Add(tb2);
                doc.Blocks.Add(p2);

                System.Windows.Documents.Paragraph p3 = new System.Windows.Documents.Paragraph();
                TextBlock tb3 = new TextBlock();
                tb3.FontSize = 15;
                tb3.Text = "Стоимость: " + cost_ord.ToString() + " руб";
                p3.Inlines.Add(tb3);
                doc.Blocks.Add(p3);

                System.Windows.Documents.Paragraph p4 = new System.Windows.Documents.Paragraph();
                TextBlock tb4 = new TextBlock();
                tb4.FontSize = 15;
                tb4.Text = "Время заказа: " + time_ord.ToString();
                p4.Inlines.Add(tb4);
                doc.Blocks.Add(p4);

                System.Windows.Documents.Paragraph p5 = new System.Windows.Documents.Paragraph();
                TextBlock tb5 = new TextBlock();
                tb5.FontSize = 15;
                tb5.Text = "Столик: " + k_tab.ToString();
                p5.Inlines.Add(tb5);
                doc.Blocks.Add(p5);

                System.Windows.Documents.Paragraph p6 = new System.Windows.Documents.Paragraph();
                TextBlock tb6 = new TextBlock();
                tb6.FontSize = 15;
                tb6.Text = "Ресторан ________. Приходите еще!" + k_tab.ToString();
                p6.Inlines.Add(tb6);
                doc.Blocks.Add(p6);

                System.Windows.Documents.Paragraph p7 = new System.Windows.Documents.Paragraph();
                TextBlock tb7 = new TextBlock();
                tb7.FontSize = 30;
                tb7.Text = "________________________________________________________";
                p7.Inlines.Add(tb7);
                doc.Blocks.Add(p7);

                // Создать PrintDialog и установить свойства печати
                PrintDialog dialog = new PrintDialog();
                if (dialog.ShowDialog() == true)
                {
                    dialog.PrintTicket.PageOrientation = PageOrientation.Portrait;
                    dialog.PrintTicket.PageBorderless = PageBorderless.None;

                    // Печать содержимого шаблона на принтере
                    IDocumentPaginatorSource paginator = doc;
                    dialog.PrintDocument(paginator.DocumentPaginator, "Чек");
                }
            }
            catch (System.NullReferenceException) { MessageBox.Show("Выберите строку в таблице."); }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
        }

        private void dataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            bOp3.IsEnabled = true;
            bOp4.IsEnabled = true;
            bOp1.IsEnabled = true;
            bOp2.IsEnabled = true;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (TextFilter.Text.Length != 0)
                    dt.DefaultView.RowFilter = string.Format("[k_tab] = {0}", TextFilter.Text);
                else dt.DefaultView.RowFilter = "";
            }
            catch (FormatException) { MessageBox.Show("Проверьте правильность введенных данных"); }
            catch (OverflowException) { MessageBox.Show("Переполнение"); }
            catch (Exception) { MessageBox.Show("Что-то пошло не так"); }
        }


        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddOrdsPage());
        }

        private void bOp1_Click(object sender, RoutedEventArgs e)
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
                SqlCommand sqlcomm = new SqlCommand("UpdateStatusOrd1", sqlconn);
                //в SqlCommand задаем процедуру
                sqlcomm.CommandType = System.Data.CommandType.StoredProcedure;
                sqlcomm.Parameters.Add("@k_ord", System.Data.SqlDbType.Int).Value = n;
                //задаем значения процедур
                SqlDataAdapter sqladap = new SqlDataAdapter(sqlcomm);
                System.Data.DataTable dtable = new System.Data.DataTable();
                sqladap.Fill(dtable);
                Manager.MainFrame.Navigate(new PageOrds(0));

            }
            catch (System.NullReferenceException) { MessageBox.Show("Выберите строку в таблице."); }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
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
                    dt.DefaultView.RowFilter = string.Format("[time_ord] >= '{0}' and [time_ord] <= '{1}'", today2, today3);
                }
                catch (FormatException) { MessageBox.Show("Проверьте правильность введенных данных"); }
                catch (OverflowException) { MessageBox.Show("Переполнение"); }
                catch (Exception) { MessageBox.Show("Что-то пошло не так"); }
            }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            try
            {
                Document document = new Document();
                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream("../../Document/ordRES.pdf", FileMode.Create));
                document.Open();
                /*    var logo = iTextSharp.text.Image.GetInstance(new FileStream(@"..\..\picter\i3.png", FileMode.Open));
                    logo.SetAbsolutePosition(440, 758);
                    writer.DirectContent.AddImage(logo); */
                BaseFont baseFont = BaseFont.CreateFont("C:\\Windows\\Fonts\\arial.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                iTextSharp.text.Font font = new iTextSharp.text.Font(baseFont, iTextSharp.text.Font.DEFAULTSIZE, iTextSharp.text.Font.NORMAL);
                PdfPTable table = new PdfPTable(dataGrid1.Columns.Count);
                PdfPCell cell = new PdfPCell(new Phrase("Заказы", font));
                cell.Colspan = dataGrid1.Columns.Count;
                cell.HorizontalAlignment = 1;
                cell.Border = 0;
                table.AddCell(cell);
                string[] name = { "Код", "Сотрудник", "Стол", "Время", "Блюда", "Стоимость", "Статус" };
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

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {

        }
    }
    }

