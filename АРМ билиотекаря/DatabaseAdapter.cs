using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;

namespace АРМ_билиотекаря
{
    class DatabaseAdapter
    {
        static DatabaseAdapter obj;
        OleDbConnection Connection;
        public static string connectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=|DataDirectory|\BD.mdb";
        private DatabaseAdapter()
        {
            Connection = new OleDbConnection(connectionString);
            Connection.Open();
        }
        
        DatabaseAdapter getInstance()
        {
            if(obj == null)
            {
                obj = new DatabaseAdapter();
            }
            return obj;
        }


    }
}
