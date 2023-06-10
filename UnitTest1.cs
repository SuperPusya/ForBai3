using NUnit.Framework;
using System.Configuration;
using System.Data;
using WpfApp1;
using Microsoft.Data.SqlClient;


namespace TestProject5
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }
        [Test]
        public void TestAddReservation()
        {
            // Arrange
            var connectionString = ConfigurationManager.ConnectionStrings["RestoranConnectionString"].ConnectionString;

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();

                // Act
                var sqlCommand = new SqlCommand("AddResProc", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;

                sqlCommand.Parameters.AddWithValue("@fio", "Иванов Иван Иванович");
                sqlCommand.Parameters.AddWithValue("@num", "89131234567");
                sqlCommand.Parameters.AddWithValue("@tab", 1);
                sqlCommand.Parameters.AddWithValue("@time_res", new DateTime(2023, 06, 10, 14, 0, 0));
                sqlCommand.Parameters.AddWithValue("@status", "Ожидает");
                sqlCommand.Parameters.AddWithValue("@fam", "Manager");

                sqlCommand.ExecuteNonQuery();

                // Assert
                var dataTable = new DataTable();
                var query = "SELECT COUNT(*) FROM res WHERE time_res = '2023-06-10 14:00:00'";
                using (var dataAdapter = new SqlDataAdapter(query, sqlConnection))
                {
                    dataAdapter.Fill(dataTable);
                }
                var count = Convert.ToInt32(dataTable.Rows[0][0]);

                Assert.AreEqual(1, count);
            }
        }
    }
}