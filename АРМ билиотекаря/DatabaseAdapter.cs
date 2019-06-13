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
        public static string connectionStringTemplate = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=";
        public static string connectionString = @"";
        private DatabaseAdapter()
        {
            connection = new OleDbConnection(connectionString);
        }
        
        public void createTables()
        {
            String queryBooks = 
                "CREATE TABLE books( " +
                "[Код] AUTOINCREMENT not null primary key, " +
                "author varchar(50), " +
                "title varchar(50), " +
                "book_language varchar(50), " +
                "location varchar(50) " +
                ")";

            String queryReaders =
                "CREATE TABLE readers(" +
                "Код AUTOINCREMENT not null primary key," +
                "name varchar(50)," +
                "surname varchar(50)," +
                "patronymic varchar(50)," +
                "birthday DateTime," +
                "phone_number varchar(50)," +
                "adress varchar(50)" +
                ")";

            String queryDebtors =
               "CREATE TABLE debtors(" +
               "Код AUTOINCREMENT not null primary key," +
               "book_id int," +
               "reader_id int," +
               "issue_date DateTime," +
               "return_date DateTime" +
               ")";
            connection.Open();
            try
            {
                lock (syncLock)
                {
                    executeQuery(queryBooks);
                    executeQuery(queryReaders);
                    executeQuery(queryDebtors);
                }
            }
            catch(System.Data.OleDb.OleDbException e)
            {

            }
            connection.Close();
        }

        public void setBDPath(String path)
        {
            connectionString = connectionStringTemplate + path;
            connection = new OleDbConnection(connectionString);
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

        public void returnBook(int debtId, string message)
        {
            lock (syncLock)
            {
                connection.Open();
                String query1 = "UPDATE books SET location = '" + message + "' WHERE Код = (SELECT book_id FROM debtors WHERE Код = " + debtId + ")";
                executeQuery(query1);

                String query = "DELETE FROM debtors WHERE Код = " + debtId;
                executeQuery(query);
                connection.Close();
            }
        }

        public DataTable getReaderBooks(int readerId)
        {
            lock (syncLock)
            {
                connection.Open();
                String query = "SELECT debtors.Код, debtors.book_id, debtors.issue_date, debtors.return_date, books.author, books.title, [books.book_language]" +
                    " FROM debtors INNER JOIN books ON debtors.book_id = books.Код " +
                    "WHERE debtors.reader_id = " + readerId;
                connection.Close();
                return formDataTable(query);
            }
        }


        public DataTable getFilteredBooks(Book book, string id)
        {
            lock (syncLock) 
            {
                connection.Open();
                String query = "SELECT * from books WHERE author LIKE '%" + book.author + "%' " +
                    "AND title LIKE '%" + book.title + "%' " +
                    "AND book_language LIKE '%" + book.language + "%' " +
                    "AND location LIKE '%" + book.location + "%' " +
                    "AND Код LIKE '%" + id + "%'";

                connection.Close();
                return formDataTable(query);
            }
        }

        public DataTable getFilteredBooksNotTaken(Book book, string id)
        {
            lock (syncLock)
            {
                connection.Open();
                String query = "SELECT * from books WHERE author LIKE '%" + book.author + "%' " +
                    "AND title LIKE '%" + book.title + "%' " +
                    "AND book_language LIKE '%" + book.language + "%' " +
                    "AND location LIKE '%" + book.location + "%' " +
                    "AND Код LIKE '%" + id + "%'" +
                    //"AND NOT ISNUMERIC(location)";
                    "AND Код NOT IN (SELECT book_id FROM debtors)";

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
            String query = "SELECT COUNT(*) FROM debtors WHERE reader_id = " + id;
            OleDbCommand command = new OleDbCommand(query, connection);
            int count = Convert.ToInt32(command.ExecuteScalar());
            return count;
        }

        public bool isThereBookAtReader(int id)
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
                    "book_id," +
                    "reader_id," +
                    "books.author," +
                    "books.title," +
                    "readers.name," +
                    "readers.surname," +
                    "readers.phone_number," +
                    "readers.adress, " +
                    "issue_date," +
                    "return_date " +
                    "FROM (debtors " +
                    "INNER JOIN books ON debtors.book_id=books.Код) " +
                    "INNER JOIN readers ON debtors.reader_id=readers.Код " +
                    "WHERE debtors.return_date <= NOW()";
                    //"WHERE debtors.return_date <= #" +
                    //DateTime.Now.ToString(@"dd\/MM\/yyyy") + "# ";
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

        public void expandIssueDate(int debitId, DateTime newDate) 
        {
            lock (syncLock)
            {
                connection.Open();
                String query = "UPDATE debtors SET return_date= '" +
                    newDate.ToString(@"dd.MM.yyyy") + "'" +
                    "WHERE Код = " + debitId;
                executeQuery(query);
                connection.Close();
            }
        }

        public void issueBookToReader(int readerId, int bookId, DateTime issueDate, DateTime returnDate)
        {
            lock (syncLock)
            {
                connection.Open();
                String query1 = "UPDATE books SET location = " + readerId + " WHERE Код = " + bookId;
                executeQuery(query1);

                String query = "INSERT INTO debtors (" +
                    "book_id," +
                    "reader_id," +
                    "issue_date," +
                    "return_date" +
                    ") VALUES ('" +
                    bookId + "','" +
                    readerId + "'," +
                    "'" + issueDate.ToString(@"dd.MM.yyyy") + "'," +
                    "'" + returnDate.ToString(@"dd.MM.yyyy") + "')";
                executeQuery(query);
                connection.Close();
            }
        }

        public void addBook(Book book)
        {
            lock (syncLock)
            {
                connection.Open();

                String query = "INSERT INTO books (" +
                    "author," +
                    "title," +
                    "book_language," +
                    "location" +
                    ") values (" +
                    "'" + book.author +
                    "','" + book.title + 
                    "','" + book.language +
                    "', '" + book.location +
                    "')";
                executeQuery(query);

                connection.Close();
            }
        }

        public void editBook(Book book, int bookId)
        {
            lock (syncLock)
            {
                connection.Open();
                String query = "UPDATE books SET " +
                    "author = '" + book.author + "'," +
                    "title = '" + book.title + "'," +
                    "book_language = '" + book.language + "'," +
                    "location = '" + book.location + 
                    "' WHERE Код = " + bookId;
                executeQuery(query);

                connection.Close();
            }
        }

        public void deleteBook(int bookId)
        {
            connection.Open();
            String query = "DELETE FROM books WHERE Код = " + bookId;
            executeQuery(query);
            connection.Close();
        }

        private int getBooksReadersCount(int bookId)
        {
            String query = "SELECT COUNT(*) FROM debtors WHERE book_id = " + bookId;
            OleDbCommand command = new OleDbCommand(query, connection);
            int count = Convert.ToInt32(command.ExecuteScalar());
            return count;
        }

        public bool isBookAtReader(int bookId)
        {
            connection.Open();
            bool ans = getBooksReadersCount(bookId) != 0;
            connection.Close();
            return ans;
        }
    }
}
