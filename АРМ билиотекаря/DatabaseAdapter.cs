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
    public class DatabaseAdapter
    {
        private readonly object syncLock = new object();
        private static DatabaseAdapter instance;
        MySqlConnection connection;
        public static string connectionStringTemplate = @"server={0};database={1};uid={2};pwd={3};CharSet=utf8;";
        public static string connectionString = @"";
        private DatabaseAdapter()
        {
            connection = new MySqlConnection(connectionString);
        }

        public void createTables()
        {
            lock (syncLock)
            {
                connection.Open();
                string text = File.ReadAllText(@"create.sql");
                string[] texts = text.Split(new[]{ "//"}, StringSplitOptions.RemoveEmptyEntries);
                foreach(string scr in texts) {
                    MySqlScript script = new MySqlScript(connection, scr + "//");
                    script.Delimiter = "//";
                    script.Execute();

                }

                MySqlScript script1 = new MySqlScript(connection, @"
                        delimiter //

                    ");
                script1.Execute();
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
            if (instance == null)
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

        public DataTable formDataTable(string query)
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
            lock (syncLock)
            {
                connection.Open();
                String query = "select record.id as record_id, book.id, book.title, CONCAT(author.surname, ' ', author.name, ' ', author.patronymic) as author, language.language, record.issue_date, record.return_date from record inner join book on record.id_book = book.id inner join author on book.id_author = author.id inner join language on book.id_language = language.id where record.id_reader = " + readerId;
                DataTable result = formDataTable(query);
                connection.Close();
                return result;
            }
        }

        private string booksQuery(string author = "", string title = "", string language = "", string id = "")
        {
            return String.Format("select " +
                    "book.id, " +
                    "book.title, " +
                    "CONCAT(author.surname, ' ', author.name, ' ', author.patronymic) as author, " +
                    "language.language, " +
                    "genre.genre, " +
                    "publishing_house.title as publishing_house, " +
                    "cover.cover_description as cover, " +
                    "era.era," +
                    "type_of_literature.type_of_literature," +
                    "book_size.name as book_size," +
                    "font_size.name as font_size," +
                    "author.id as author_id, " +
                    "language.id as language_id, " +
                    "genre.id as genre_id," +
                    "publishing_house.id as publishing_house_id, " +
                    "cover.id as cover_id, " +
                    "era.id as era_id, " +
                    "type_of_literature.id as type_of_literature_id, " +
                    "book_size.id as book_size_id, " +
                    "font_size.id as font_size_id " +
                    "from book " +
                    "inner join author on book.id_author = author.id " +
                    "inner join language on book.id_language = language.id " +
                    "inner join genre on book.id_genre = genre.id " +
                    "inner join publishing_house on book.id_publishing_house = publishing_house.id " +
                    "inner join cover on book.id_cover = cover.id " +
                    "inner join era on book.id_era = era.id " +
                    "inner join type_of_literature on book.id_type_of_literature = type_of_literature.id " +
                    "inner join book_size on book.id_book_size = book_size.id " +
                    "inner join font_size on book.id_font_size = font_size.id " +
                    "WHERE " +
                    "(CONCAT(author.surname, ' ', author.name, ' ', author.patronymic) LIKE '%{0}%') AND " +
                    "(book.title LIKE '%{1}%') AND " +
                    "(language.language LIKE '%{2}%') AND" +
                    "(book.id LIKE '%{3}%')", author, title, language, id);
        }

        public DataTable getFilteredBooks(Book book, string id)
        {
            lock (syncLock)
            {
                connection.Open();
                String query = booksQuery(book.author, book.title, book.language, id);
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

        public DataTable getDebtors()
        {
            lock (syncLock)
            {
                connection.Open();
                String query = "SELECT " +
                    "record.id as id," +
                    "book.id as id_book, " +
                    "record.id_reader as id_reader, " +
                    "reader.name as name, " +
                    "reader.surname as surname, " +
                    "reader.phone_number as phone_number, " +
                    "reader.address as address, " +
                    "book.title as title," +
                    "return_date, " +
                    "issue_date, " +
                    "CONCAT(author.surname, ' ', author.name, ' ', author.patronymic) as author" +
                    " FROM record inner join book on record.id_book = book.id " +
                    "inner join reader on reader.id = record.id_reader " +
                    "inner join author on book.id_author = author.id " +
                    "WHERE (record.return_date <= now())";
                DataTable answer = formDataTable(query);
                connection.Close();
                return answer;
            }
        }

        public DataTable deleteReaderAndGetFilteredReaders(Reader reader, bool isDate, int id)
        {
            lock (syncLock)
            {
                connection.Open();
                bool error = false;
                try
                {
                    String query = "DELETE FROM reader WHERE id = " + id;
                    executeQuery(query);
                }
                catch (Exception ex)
                {
                    error = true;
                }
                finally
                {
                    connection.Close();
                }
                if (error)
                {
                    return null;
                }
                else
                {
                    return getFilteredReaders(reader, isDate);
                }
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

        public void issueBookToReader(int readerId, int bookId, DateTime issueDate, DateTime returnDate, int librarianId)
        {
            lock (syncLock)
            {
                connection.Open();
                String query = "INSERT INTO record (" +
                    "id_reader," +
                    "id_book," +
                    "issue_date," +
                    "return_date," +
                    "id_librarian" +
                    ") VALUES ('" +
                    readerId + "','" +
                    bookId + "'," +
                    "'" + issueDate.ToString(@"yyyy-MM-dd") + "'," +
                    "'" + returnDate.ToString(@"yyyy-MM-dd") + "'," +
                    librarianId + ")";
                executeQuery(query);
                connection.Close();
            }
        }

        public void addBook(string title, int author, int language, int genre, int ph, int cover,
            int era,
            int type,
            int font,
            int size)
        {
            lock (syncLock)
            {
                connection.Open();

                String query = "INSERT INTO book (" +
                    "title," +
                    "id_author," +
                    "id_language," +
                    "id_genre," +
                    "id_publishing_house," +
                    "id_cover," +
                    "id_era," +
                    "id_type_of_literature," +
                    "id_font_size," +
                    "id_book_size" +
                    ") values ('" + title +
                    "', " + author +
                    ", " + language +
                    ", " + genre +
                    ", " + ph +
                    ", " + cover +
                    ", " + era +
                    ", " + type +
                    ", " + font +
                    ", " + size +
                    ")";
                executeQuery(query);
                connection.Close();
            }
        }

        public void editBook(string title, int author, int language, int genre, int ph, int cover,
            int era,
            int type,
            int font,
            int size,
            int id)
        {
            lock (syncLock)
            {
                connection.Open();
                String query = String.Format("update book set " +
                    "title = '{0}', id_author = '{1}', id_language = '{2}', id_genre = '{3}', id_publishing_house = '{4}', id_cover = '{5}', id_era = '{6}', id_type_of_literature = '{7}', id_font_size = '{8}', id_book_size = '{9}'  where id = '{10}'", title, author, language, genre, ph, cover, era, type, font, size, id);

                executeQuery(query);
                connection.Close();
            }
        }

        public bool deleteBook(int bookId)
        {
            connection.Open();
            bool error = false;
            try
            {
                String query = "DELETE FROM book WHERE id = " + bookId;
                executeQuery(query);
            }
            catch (Exception ex)
            {
                error = true;
            }
            finally
            {
                connection.Close();
            }
            return error;
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
                String query = "SELECT publishing_house.title, city.city_name, city.id as city_id, " +
                    "publishing_house.id as id " +
                    "FROM publishing_house INNER JOIN city ON publishing_house.id_city = city.id";
                var covers = formDataTable(query);
                connection.Close();
                return covers;
            }
        }

        public DataTable addCommon(string table, string data)
        {
            lock (syncLock)
            {
                connection.Open();
                String query = String.Format("insert into {0} values(null, '{1}')", table, data);
                executeQuery(query);
                connection.Close();
                return getCommonData(table);
            }
        }
        public DataTable editCommon(string table, string tableName, string value, int id)
        {
            lock (syncLock)
            {
                connection.Open();
                String query = String.Format("update {0} set {1} = '{2}' where id = {3}", table, tableName, value, id);
                executeQuery(query);
                connection.Close();
                return getCommonData(table);
            }
        }
        public DataTable deleteCommon(string table, int id)
        {
            lock (syncLock)
            {
                connection.Open();
                String query = String.Format("delete from {0} where id = '{1}'", table, id);
                bool error = false;
                try
                {
                    executeQuery(query);
                }
                catch (Exception ex)
                {
                    error = true;
                }
                finally
                {
                    connection.Close();
                }
                if (error)
                {
                    return null;
                }
                else
                {
                    return getCommonData(table);
                }
            }
        }

        public DataTable authorFullName()
        {
            lock (syncLock)
            {
                connection.Open();
                String query = "SELECT id, CONCAT(surname, ' ', name, ' ', patronymic) as name FROM author";
                var covers = formDataTable(query);
                connection.Close();
                return covers;
            }
        }

        public UserResolution resolveUser(string name, string surname, string password)
        {
            lock (syncLock)
            {
                connection.Open();
                String query = String.Format("SELECT id, password_hash FROM librarian WHERE name = '{0}' AND surname = '{1}'", name, surname);
                var users = formDataTable(query);
                int? librarian = null;
                for (int i = 0; i < users.Rows.Count; i++)
                {
                    var row = users.Rows[i];
                    if (row.Field<string>("password_hash") == password)
                    {
                        librarian = row.Field<int>("id");
                        break;
                    }
                }
                if (librarian != null)
                {
                    connection.Close();
                    return new Success((int)librarian, false);
                }

                query = String.Format("SELECT id, password_hash FROM admin WHERE name = '{0}' AND surname = '{1}'", name, surname);
                var admins = formDataTable(query);
                connection.Close();
                int? admin = null;
                for (int i = 0; i < admins.Rows.Count; i++)
                {
                    var row = admins.Rows[i];
                    if (row.Field<string>("password_hash") == password)
                    {
                        admin = row.Field<int>("id");
                        break;
                    }
                }
                if(admin != null)
                {
                    return new Success((int)admin, true);
                }
                if(users.Rows.Count + admins.Rows.Count != 0)
                {
                    return new WrongPassword();
                } else
                {
                    return new UserNotFound();
                }
            }
        }

        public DataTable getAllUsers()
        {
            lock (syncLock)
            {
                connection.Open();
                String query = "SELECT librarian.id, librarian.name, librarian.surname, librarian.password_hash, FALSE as is_admin, CONCAT(librarian.id_admin, ': ' ,admin.name, ' ', admin.surname) as author FROM librarian LEFT JOIN admin ON librarian.id_admin = admin.id UNION SELECT id, name, surname, password_hash, TRUE as is_admin, NULL FROM admin";
                var users = formDataTable(query);
                connection.Close();
                return users;
            }
        }

        private bool findUser(string name, string surname)
        {
            lock (syncLock)
            {
                connection.Open();
                String query1 = String.Format("SELECT COUNT(*) FROM librarian WHERE name = '{0}' AND surname = '{1}'", name, surname);
                String query2 = String.Format("SELECT COUNT(*) FROM admin     WHERE name = '{0}' AND surname = '{1}'", name, surname);
                var count = Int32.Parse(formDataTable(query1).Rows[0][0].ToString()) + Int32.Parse(formDataTable(query2).Rows[0][0].ToString());
                connection.Close();
                return count != 0;
            }
        }

        public bool editUser(bool isAdmin, int id, string name, string surname, string password, bool isPasswordUpdate)
        {
            if(!isPasswordUpdate && findUser(name, surname))
            {
                return false;
            }
            lock (syncLock)
            {
                connection.Open();
                String table;
                if (isAdmin)
                {
                    table = "admin";
                } else
                {
                    table = "librarian";
                }
                String query = String.Format("UPDATE {4} SET name = '{0}', surname = '{1}', password_hash = '{2}' WHERE id = {3}", name, surname, password, id, table);
                executeQuery(query);
                connection.Close();
            }
            return true;
        }
        public void deleteUser(bool isAdmin, int id)
        {
            lock (syncLock)
            {
                connection.Open();
                String table;
                if (isAdmin)
                {
                    table = "admin";
                }
                else
                {
                    table = "librarian";
                }
                String query = String.Format("DELETE FROM {1} WHERE id = {0}", id, table);
                executeQuery(query);
                connection.Close();
            }
        }

        public bool addUser(bool isAdmin, string name, string surname, string password, int userAuthorId)
        {
            if (findUser(name, surname))
            {
                return false;
            }
            lock (syncLock)
            {
                connection.Open();
                String table;
                if (isAdmin)
                {
                    table = String.Format("INSERT INTO admin (name, surname, password_hash) VALUES ('{0}', '{1}', '{2}')", name, surname, password);
                }
                else
                {
                    table = String.Format("INSERT INTO librarian (name, surname, password_hash, id_admin) VALUES ('{0}', '{1}', '{2}', {3})", name, surname, password, userAuthorId);
                }
                executeQuery(table);
                connection.Close();
            }
            return true;
        }

        public DataTable getExcelBooks()
        {
            lock (syncLock)
            {
                connection.Open();
                var booksQueryStr = booksQuery();
                var query = String.Format("SELECT " +
                    "sub.id as book_id, title, author, language, genre, publishing_house, cover, era, type_of_literature, book_size, font_size, DATE_FORMAT(issue_date, '%d.%m.%Y') AS issue_date, DATE_FORMAT(return_date, '%d.%m.%Y') AS return_date, name, surname, patronymic, DATE_FORMAT(birthday, '%d.%m.%Y') AS birthday, phone_number, address " +
                    "FROM ({0}) AS sub " +
                    "LEFT JOIN record ON sub.id = record.id_book " +
                    "LEFT JOIN reader ON record.id_reader = reader.id " +
                    "ORDER BY book_id ASC", booksQueryStr);
                DataTable result = formDataTable(query);
                connection.Close();
                return result;
            }
        }

        public interface UserResolution
        {
        }

        public class UserNotFound : UserResolution
        {
        }

        public class WrongPassword : UserResolution
        {
        }

        public class Success : UserResolution
        {
            public readonly int userId;
            public readonly bool isAdmin;

            public Success(int userId, bool isAdmin)
            {
                this.userId = userId;
                this.isAdmin = isAdmin;
            }
        }
    }
}
