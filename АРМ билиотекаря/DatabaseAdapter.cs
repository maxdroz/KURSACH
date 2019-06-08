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
        private readonly object syncLock = new object();
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

        private void executeQuery(string query)
        {
            OleDbCommand com = new OleDbCommand(query, connection);
            com.ExecuteNonQuery();
        }

        private DataTable formDataTable(string query)
        {
            OleDbDataAdapter da = new OleDbDataAdapter(query, connection);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }
        
        //[MethodImpl(MethodImplOptions.Synchronized)]
         public DataTable getFilteredReaders(Reader reader , bool isDate)
         {
            lock (syncLock)
            {
                connection.Open();
                String query = "SELECT * FROM readers WHERE name LIKE '%" + reader.name + "%'" +
                    "AND surname LIKE '%" + reader.surname + "%' " +
                    "AND patronymic LIKE '%" + reader.patronymic + "%' " +
                    "AND phone_number LIKE '%" + reader.phone_number + "%' " +
                    "AND Код LIKE '%" + reader.id + "%'" +
                    "AND adress LIKE '%" + reader.adress + "%'";
                if (isDate)
                    query += "AND birthday = #" + reader.birthday.ToString(@"MM\/dd\/yyyy") + "#";
                connection.Close();

                return formDataTable(query);
            }
         }

        public DataTable getReaderBooks(int reader_id)
        {
            lock (syncLock)
            {
                connection.Open();
                String query = "SELECT debtors.book_id, debtors.issue_date, debtors.return_date, books.author, books.title, [books.book_language]" +
                    " FROM debtors INNER JOIN books ON debtors.book_id = books.Код " +
                    "WHERE debtors.reader_id = " + reader_id;
                connection.Close();
                return formDataTable(query);
            }
        }


        public DataTable getFilteredBooks(string author, string name, string language, string location, string id)
        {
            lock (syncLock) 
            {
                connection.Open();
                //TODO Разобрвться с исключением
                String query = "SELECT * from books WHERE author LIKE '%" + author + "%' " +
                    "AND title LIKE '%" + name + "%' " +
                    "AND book_language LIKE '%" + language + "%' " +
                    "AND location LIKE '%" + location + "%' " +
                    "AND Код LIKE '%" + id + "%'";

                connection.Close();
                return formDataTable(query);
            }
        }

        public void editReader(Reader reader)
        {
            lock (syncLock)
            {
                connection.Open();
                String query = "UPDATE readers SET " +
                    "name= '" + reader.name + "'," +
                    "surname= '" + reader.surname + "'," +
                    "patronymic= '" + reader.patronymic + "'," +
                    "birthday= '" + reader.birthday.ToString(@"dd.MM.yyyy") + "'," +
                    "phone_number= '" + reader.phone_number + "'," +
                    "adress= '" + reader.adress + "'" +
                    "WHERE Код =" + reader.id;
                executeQuery(query);
                connection.Close();
            }
        }
        public void addReader(Reader reader)
        {
            lock (syncLock)
            {
                connection.Open();
                String query = "INSERT INTO readers (" +
                    "name, " +
                    "surname, " +
                    "patronymic, " +
                    "phone_number, " +
                    "birthday, " +
                    "adress) VALUES ('" +
                    reader.name + "','" +
                    reader.surname + "','" +
                    reader.patronymic + "','" + 
                    reader.phone_number + "','" +
                    reader.birthday.ToString(@"dd.MM.yyyy") + "','" + 
                    reader.adress + "')";
                executeQuery(query);
                connection.Close();
            }
        }

        public int getBooksCountFromReader(int id)
        {
            String query = "SELECT COUNT(*) FROM debtors WHERE Код = " + id;
            OleDbCommand command = new OleDbCommand(query, connection);
            int count = Convert.ToInt32(command.ExecuteScalar());
            return count;
        }

        public bool areThereBookAtReader(int id)
        {
            lock (syncLock)
            {
                connection.Open();
                bool ans = getBooksCountFromReader(id) != 0;
                connection.Close();
                return ans;
            }
        }

        public DataTable getDebtors()
        {
            lock (syncLock)
            {
                connection.Open();
                String query = "SELECT debtors.Код," +
                    " issue_date," +
                    " return_date," +
                    " books.author," +
                    " books.title," +
                    " readers.name," +
                    " readers.surname," +
                    " readers.phone_number," +
                    " readers.adress FROM (debtors " +
                    "INNER JOIN books ON debtors.book_id=books.Код) " +
                    "INNER JOIN readers ON debtors.reader_id=readers.Код " + 
                    "WHERE debtors.return_date <= #" +
                    DateTime.Now.ToString(@"dd\/MM\/yyyy") + "# ";
                connection.Close();
                return formDataTable(query);
            }
        }

        public DataTable deleteReaderAndGetFilteredReaders(Reader reader, bool isDate, int id)
        {
            lock (syncLock)
            {
                connection.Open();
                String query = "DELETE FROM readers WHERE Код = " + id;
                executeQuery(query);
                connection.Close();
                return getFilteredReaders(reader, isDate);
            }
        }

        public void issueBookToReader(int readerId, int bookId, DateTime issueDate, DateTime returnDate)
        {
            lock (syncLock)
            {
                connection.Open();
                String query = "INSERT INTO debtors (" +
                    "book_id," +
                    "reader_id," +
                    "issue_date," +
                    "return_date" +
                    ") VALUES (" +
                    bookId + "," +
                    readerId + "," +
                    "'" + issueDate.ToString(@"dd.MM.yyyy") + "'," +
                    "'" + returnDate.ToString(@"dd.MM.yyyy") + "')";
                executeQuery(query);
                connection.Close();
            }
        }
    }
}
