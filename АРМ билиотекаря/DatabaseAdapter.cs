using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Data;
using MySql.Data.MySqlClient;
using System.IO;

namespace АРМ_билиотекаря
{
    class DatabaseAdapter
    {
        private readonly object syncLock = new object();
        private static DatabaseAdapter instance;
        MySqlConnection connection;
        public static string connectionStringTemplate = @"server={0};database={1};uid={2};pwd={3};";
        public static string connectionString = @"";
        private DatabaseAdapter()
        {
            connection = new MySqlConnection(connectionString);
        }

        public void createTables()
        { 
        //{
        //    String queryBooks = 
        //        "CREATE TABLE IF NOT EXISTS books( " +
        //        "Код int AUTO_INCREMENT not null primary key, " +
        //        "author varchar(50), " +
        //        "title varchar(50), " +
        //        "book_language varchar(50), " +
        //        "location varchar(50) " +
        //        ")";

        //    String queryReaders =
        //        "CREATE TABLE IF NOT EXISTS readers(" +
        //        "Код int AUTO_INCREMENT not null primary key," +
        //        "name varchar(50)," +
        //        "surname varchar(50)," +
        //        "patronymic varchar(50)," +
        //        "birthday DateTime," +
        //        "phone_number varchar(50)," +
        //        "adress varchar(50)" +
        //        ")";

        //    String queryDebtors =
        //       "CREATE TABLE IF NOT EXISTS debtors(" +
        //       "Код int AUTO_INCREMENT not null primary key," +
        //       "book_id int," +
        //       "reader_id int," +
        //       "issue_date DateTime," +
        //       "return_date DateTime" +
        //       ")";
            lock (syncLock)
            {
                connection.Open();
                MySqlScript script = new MySqlScript(connection, File.ReadAllText(@"create.sql"));
                script.Delimiter = "$$";
                script.Execute();     
                connection.Close();
            }
        }

        public void setBDPath(String address, String databaseName, String username, String password)
        {
            connectionString = String.Format(connectionStringTemplate, address, databaseName, username, password);
            connection = new MySqlConnection(connectionString);
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
            MySqlCommand com = new MySqlCommand(query, connection);
            com.ExecuteNonQuery();
        }

        private DataTable formDataTable(string query)
        {
            MySqlDataAdapter da = new MySqlDataAdapter(query, connection);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }
        
         public DataTable getFilteredReaders(Reader reader, bool isDate)
         {
            lock (syncLock)
            {
                connection.Open();
                String query = "SELECT * FROM reader WHERE name LIKE '%" + reader.name + "%' AND " +
                    "surname LIKE '%" + reader.surname + "%' AND " +
                    "patronymic LIKE '%" + reader.patronymic + "%' AND " +
                    "phone_number LIKE '%" + reader.phone_number + "%' AND " +
                    "id LIKE '%" + reader.id + "%' AND " +
                    "address LIKE '%" + reader.address + "%'";
                if (isDate)
                    query += " AND birthday = '" + reader.birthday.ToString(@"yyyy-MM-dd") + "' ";
                connection.Close();
                return formDataTable(query);
            }
         }

        public void returnBook(int debtId)
        {
            lock (syncLock)
            {
                connection.Open();
                String query = "DELETE FROM record WHERE id = " + debtId;
                executeQuery(query);
                connection.Close();
            }
        }

        public DataTable getReaderBooks(int readerId)
        {
            lock (syncLock) {
                connection.Open();
                String query = "select record.id, book.title, CONCAT(author.surname, ' ', author.name, ' ', author.patronymic) as author, language.language, record.issue_date, record.return_date from record inner join book on record.id_book = book.id inner join author on book.id_author = author.id inner join language on book.id_language = language.id where record.id_reader = " + readerId;
                DataTable result = formDataTable(query);
                connection.Close();
                return result;
            }
        }

        public DataTable getFilteredBooks(Book book, string id)
        {
            lock (syncLock) 
            {
                connection.Open();
                String query = String.Format("select " +
                    "book.id, book.title, CONCAT(author.surname, ' ', author.name, ' ', author.patronymic) as author, language.language, genre.genre, publishing_house.title as publishing_house, cover.cover_description as cover from book " +
                    "inner join author on book.id_author = author.id " +
                    "inner join language on book.id_language = language.id " +
                    "inner join genre on book.id_genre = genre.id " +
                    "inner join publishing_house on book.id_publishing_house = publishing_house.id " +
                    "inner join cover on book.id_cover = cover.id " +
                    "WHERE " +
                    "(CONCAT(author.surname, ' ', author.name, ' ', author.patronymic) LIKE '%{0}%') AND " +
                    "(book.title LIKE '%{1}%') AND " +
                    "(language.language LIKE '%{2}%') AND" +
                    "(book.id LIKE '%{3}%')", book.author, book.title, book.language, id);
                //String query = "SELECT * from books WHERE author LIKE '%" + book.author + "%' " +
                //    "AND title LIKE '%" + book.title + "%' " +
                //    "AND book_language LIKE '%" + book.language + "%' " +
                //    "AND location LIKE '%" + book.location + "%' " +
                //    "AND Код LIKE '%" + id + "%'";
                DataTable result = formDataTable(query);
                connection.Close();
                return result;
            }
        }

        public DataTable getFilteredBooksNotTaken(Book book, string id)
        {
            lock (syncLock)
            {
                connection.Open();
                String query = String.Format("select " +
                   "book.id, book.title, CONCAT(author.surname, ' ', author.name, ' ', author.patronymic) as author, language.language, genre.genre, publishing_house.title as publishing_house, cover.cover_description as cover from book " +
                   "inner join author on book.id_author = author.id " +
                   "inner join language on book.id_language = language.id " +
                   "inner join genre on book.id_genre = genre.id " +
                   "inner join publishing_house on book.id_publishing_house = publishing_house.id " +
                   "inner join cover on book.id_cover = cover.id " +
                   "WHERE " +
                   "(CONCAT(author.surname, ' ', author.name, ' ', author.patronymic) LIKE '%{0}%') AND " +
                   "(book.title LIKE '%{1}%') AND " +
                   "(language.language LIKE '%{2}%') AND" +
                   "(book.id LIKE '%{3}%') AND " +
                   "(book.id NOT IN (select id_book from record))", book.author, book.title, book.language, id);
                //String query = "SELECT * from books WHERE author LIKE '%" + book.author + "%' " +
                //    "AND title LIKE '%" + book.title + "%' " +
                //    "AND book_language LIKE '%" + book.language + "%' " +
                //    "AND Код LIKE '%" + id + "%'" +
                //    //"AND NOT ISNUMERIC(location)";
                //    "AND Код NOT IN (SELECT book_id FROM debtors)";
                DataTable result = formDataTable(query);
                connection.Close();
                return result;
            }
        }

        public void editReader(Reader reader)
        {
            lock (syncLock)
            {
                connection.Open();
                String query = "UPDATE reader SET " +
                    "name= '" + reader.name + "'," +
                    "surname= '" + reader.surname + "'," +
                    "patronymic= '" + reader.patronymic + "'," +
                    "birthday= '" + reader.birthday.ToString(@"yyyy-MM-dd") + "'," +
                    "phone_number= '" + reader.phone_number + "'," +
                    "address= '" + reader.address + "'" +
                    "WHERE id =" + reader.id;
                executeQuery(query);
                connection.Close();
            }
        }
        
        public void addReader(Reader reader)
        {
            lock (syncLock)
            {
                connection.Open();
                String query = "INSERT INTO reader (" +
                    "name, " +
                    "surname, " +
                    "patronymic, " +
                    "phone_number, " +
                    "birthday, " +
                    "address) VALUES ('" +
                    reader.name + "','" +
                    reader.surname + "','" +
                    reader.patronymic + "','" + 
                    reader.phone_number + "','" +
                    reader.birthday.ToString(@"yyyy-MM-dd") + "','" + 
                    reader.address + "')";
                executeQuery(query);
                connection.Close();
            }
        }

        public int getBooksCountFromReader(int id)
        {
            String query = "SELECT COUNT(*) FROM debtors WHERE reader_id = " + id;
            MySqlCommand command = new MySqlCommand(query, connection);
            int count = Convert.ToInt32(command.ExecuteScalar());
            return 0;
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
                return new DataTable();
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
                return new DataTable();
                return getFilteredReaders(reader, isDate);
            }
        }

        public void expandIssueDate(int debitId, DateTime newDate) 
        {
            lock (syncLock)
            {
                connection.Open();
                String query = "UPDATE record SET return_date= '" +
                    newDate.ToString(@"yyyy-MM-dd") + "'" +
                    "WHERE id = " + debitId;
                executeQuery(query);
                connection.Close();
            }
        }

        public void issueBookToReader(int readerId, int bookId, DateTime issueDate, DateTime returnDate)
        {
            lock (syncLock)
            {
                connection.Open();
                String query = "INSERT INTO record (" +
                    "id_reader," +
                    "id_book," +
                    "issue_date," +
                    "return_date" +
                    ") VALUES ('" +
                    readerId + "','" +
                    bookId + "'," +
                    "'" + issueDate.ToString(@"yyyy-MM-dd") + "'," +
                    "'" + returnDate.ToString(@"yyyy-MM-dd") + "')";
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
                    "')";
                connection.Close();
                return;
                executeQuery(query);
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
                    "' WHERE Код = " + bookId;
                connection.Close();
                return;
                executeQuery(query);
            }
        }

        public void deleteBook(int bookId)
        {
            connection.Open();
            String query = "DELETE FROM books WHERE Код = " + bookId;
            connection.Close();
            return;
            executeQuery(query);
        }

        private int getBooksReadersCount(int bookId)
        {
            String query = "SELECT COUNT(*) FROM debtors WHERE book_id = " + bookId;
            MySqlCommand command = new MySqlCommand(query, connection);
            int count = Convert.ToInt32(command.ExecuteScalar());
            return 0;
            return count;
        }

        public bool isBookAtReader(int bookId)
        {
            lock (syncLock)
            {
                connection.Open();
                bool ans = getBooksReadersCount(bookId) != 0;
                connection.Close();
                return false;
                return ans;
            }
        }

        public DataTable getCommonData(string tableName)
        {
            lock (syncLock)
            {
                connection.Open();
                String query = "SELECT * FROM " + tableName;
                var covers = formDataTable(query);
                connection.Close();
                return covers;
            }
        }
        public DataTable getPublishers()
        {
            lock (syncLock)
            {
                connection.Open();
                String query = "SELECT publishing_house.title, city.city_name FROM publishing_house INNER JOIN city ON publishing_house.id_city = city.id";
                var covers = formDataTable(query);
                connection.Close();
                return covers;
            }
        }
    }
}
