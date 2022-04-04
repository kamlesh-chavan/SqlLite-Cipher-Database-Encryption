using Microsoft.Extensions.Options;
using Microsoft.Data.Sqlite;

namespace DatabaseEncryption
{
    public class SqlCipherHelper : ISqlCipherHelper
    {
        private readonly AppSettings options;

        public SqlCipherHelper(IOptions<AppSettings> options)
        {
            this.options = options.Value;
        }

        public void DoEncription(string path)
        {
            DirectoryInfo d = new DirectoryInfo($@"{path}");
            FileInfo[] files = d.GetFiles("*.db");

            foreach (FileInfo file in files)
            {
                string enc_db_path = $@"{path}\\enc_{file.Name.Split('.')[0]}.sqlite";

                if (File.Exists(enc_db_path))
                    File.Delete(enc_db_path);

                SqliteConnection sqlConnection = new SqliteConnection($@"Data Source={file.FullName};"); 
                sqlConnection.Open();

                string sql = "PRAGMA cipher_version";
                SqliteCommand command = new SqliteCommand(sql, sqlConnection);
                command.ExecuteNonQuery();

                sql = "ATTACH DATABASE '" + enc_db_path + "' AS encrypted KEY '" + this.options.Password + "';";
                command = new SqliteCommand(sql, sqlConnection);
                command.ExecuteNonQuery();

                sql = $"PRAGMA encrypted.cipher_page_size = {this.options.CipherPageSize};";
                command = new SqliteCommand(sql, sqlConnection);
                command.ExecuteNonQuery();

                sql = $"PRAGMA encrypted.kdf_iter = {this.options.KdfIter};";
                command = new SqliteCommand(sql, sqlConnection);
                command.ExecuteNonQuery();

                sql = $"PRAGMA encrypted.cipher_hmac_algorithm = {this.options.CipherHmacAlgorithm};";
                command = new SqliteCommand(sql, sqlConnection);
                command.ExecuteNonQuery();

                sql = $"PRAGMA encrypted.cipher_kdf_algorithm  = {this.options.CipherKdfAlgorithm};";
                command = new SqliteCommand(sql, sqlConnection);
                command.ExecuteNonQuery();

                sql = "SELECT sqlcipher_export('encrypted')";
                command = new SqliteCommand(sql, sqlConnection);
                command.ExecuteNonQuery();

                sql = "DETACH DATABASE encrypted;";
                command = new SqliteCommand(sql, sqlConnection);
                command.ExecuteNonQuery();

                sqlConnection.Close();

            }
        }
    }
}
