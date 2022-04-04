using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Options;
using System.Data.SQLite;
//using SQLite;

namespace DatabaseEncryption
{
    public class SqliteDbHelper : ISqliteDbHelper
    {
        private readonly AppSettings options;

        public SqliteDbHelper(IOptions<AppSettings> options)
        {
            this.options = options.Value;
        }

        public bool CreateDbWithData()
        {
            bool result = true;
            try
            {
                //C:\Users\Kamlesh Chavan\Downloads\sqliteDbExample.sqlite
                if (!System.IO.File.Exists($@"{this.options.DbPath}"))
                {
                    Console.WriteLine("Just entered to create Sync DB");
                    SQLiteConnection.CreateFile($@"{this.options.DbPath}");

                    using (var sqlite = new SQLiteConnection($@"Data Source={this.options.DbPath}"))
                    {
                        sqlite.Open();

                        using (var cmd = new SQLiteCommand(sqlite))
                        {
                            cmd.CommandText = "create table highscores (name varchar(20), score int)";
                            cmd.ExecuteNonQuery();

                            cmd.CommandText = "INSERT INTO highscores(name, score) VALUES('xyz',5)";
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                return result;
            }
            catch (Exception e)
            {
                return result;
            }
        }

        public bool ReadData()
        {
            bool result = false;
            try
            {
                //C:\Users\Kamlesh Chavan\Downloads\sqliteDbExample.sqlite
                SQLiteConnection conn = new SQLiteConnection($@"Data Source={this.options.DbPath}");
                conn.Open();

                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = "select * from highscores";

                SQLiteDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    result =  true;
                    break;
                }

                reader.Close();
                return result;
            }
            catch (Exception e)
            {
                return result;
            }
        }
    }
}
