using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Data;

namespace АРМ_билиотекаря
{
    class DatabaseAdapter
    {
        private static DatabaseAdapter instance;
        OleDbConnection connection;
        public static string connectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=|DataDirectory|\BD.mdb";
        private DatabaseAdapter()
        {
            connection = new OleDbConnection(connectionString);
            //Connection.Open();
        }
        
        public static DatabaseAdapter getInstance()
        {
            if(instance == null)
            {
                instance = new DatabaseAdapter();
            }
            return instance;
        }
        private DataTable formDataTable(string query)
        {
            OleDbDataAdapter da = new OleDbDataAdapter(query, connection);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        public DataTable getFilteredReaders(string name, string surname, string patronymic, string phone_number, bool isDate, DateTime date, string reader_id)
        {
            connection.Open();
            String query = "SELECT * FROM readers WHERE name LIKE '%" + name + "%'" +
                "AND surname LIKE '%" + surname + "%' " +
                "AND patronymic LIKE '%" + patronymic + "%' " + 
                "AND phone_number LIKE '%" + phone_number + "%' " +
                "AND Код LIKE '%" + reader_id + "%'";
            if (isDate)
                query += "AND birthday = #" + date.ToString(@"MM\/dd\/yyyy") + "#";
            //Random r = new Random();
            //int f = r.Next(1, 10);
            connection.Close();

            return formDataTable(query);
        }

        public DataTable getReaderBooks(int reader_id)
        {
            connection.Open();
            String query = "SELECT debtors.book_id, debtors.issue_date, debtors.return_date, books.author, books.title, [books.language]" +
                " FROM debtors INNER JOIN books ON debtors.book_id = books.Код " +
                "WHERE debtors.reader_id = " + reader_id;
            connection.Close();
            return formDataTable(query);
        }


        public DataTable getFilteredBooks(string author, string name, string language, string location)
        {
            connection.Open();
            //TODO Разобрвться с исключением
            String query = "SELECT * from books WHERE author LIKE '%" + author + "'% " +
                "AND name LIKE '%" + name + "%' " +
                "AND language LIKE '%" + language + "'% " +
                "AND location LIKE '%" + location + "'% ";
            
            connection.Close();
            return formDataTable(query);
        }
    }
}
