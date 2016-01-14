using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovelReader
{
    public static class NovelInfoDB
    {

        private static string insertNovelCommand = "INSERT INTO novel (novel_id, title,state,last_played_chapter_id,source,source_id,updated_chapter_count,rank) VALUES(@novel_id,@title,@state,@last_played_chapter_id,@source,@source_id,@updated_chapter_count,@rank)";
        private static string updateNovelCommand = "UPDATE novel SET state=@state,last_played_chapter_id=@last_played_chapter_id,updated_chapter_count=@updated_chapter_count,rank=@rank WHERE novel_id=@novel_id";
        private static string novelQueryCommand = "SELECT * FROM novel ORDER BY rank ASC";

        public static SqlConnection GetNovelDB(bool deleteIfExists = false)
        {
            //Console.WriteLine("Get Novel DB");
            try
            {
                string dbName = "NovelInfo";
                string novelFolderLocation = Configuration.Instance.NovelFolderLocation;
                string outputFolder = Path.Combine(Configuration.Instance.NovelFolderLocation, dbName);
                string mdfFilename = dbName + ".mdf";
                string dbFileName = Path.Combine(outputFolder, mdfFilename);
                string logFileName = Path.Combine(outputFolder, String.Format("{0}_log.ldf", dbName));
                bool createdNew = false;
                // Create Data Directory If It Doesn't Already Exist.
                if (!Directory.Exists(outputFolder))
                {
                    Directory.CreateDirectory(outputFolder);
                }

                // If the file exists, and we want to delete old data, remove it here and create a new database.
                if (File.Exists(dbFileName) && deleteIfExists)
                {
                    if (File.Exists(logFileName)) File.Delete(logFileName);
                    File.Delete(dbFileName);
                    CreateDatabase(dbName, dbFileName);
                    createdNew = true;
                }
                // If the database does not already exist, create it.
                else if (!File.Exists(dbFileName))
                {
                    CreateDatabase(dbName, dbFileName);
                    createdNew = true;
                }

                // Open newly created, or old database.
                string connectionString = String.Format(@"Data Source=(LocalDB)\v11.0;AttachDBFileName={1};Initial Catalog={0};Integrated Security=True;", dbName, dbFileName);
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                if (createdNew)
                    InitializeDBTables(connection);
                return connection;
            }
            catch
            {
                throw;
            }
        }

        public static bool CreateDatabase(string dbName, string dbFileName)
        {
            //Console.WriteLine("Create novel DB");
            try
            {
                string connectionString = String.Format(@"Data Source=(LocalDB)\v11.0;Initial Catalog=master;Integrated Security=True");
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand cmd = connection.CreateCommand();


                    DetachDatabase(dbName);

                    cmd.CommandText = String.Format("CREATE DATABASE {0} ON (NAME = N'{0}', FILENAME = '{1}')", dbName, dbFileName);
                    cmd.ExecuteNonQuery();
                }

                if (File.Exists(dbFileName)) return true;
                else return false;
            }
            catch
            {
                throw;
            }
        }

        public static bool DetachDatabase(string dbName)
        {
            try
            {
                string connectionString = String.Format(@"Data Source=(LocalDB)\v11.0;Initial Catalog=master;Integrated Security=True");
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand cmd = connection.CreateCommand();
                    cmd.CommandText = String.Format("exec sp_detach_db '{0}'", dbName);
                    cmd.ExecuteNonQuery();

                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        private static bool InitializeDBTables(SqlConnection connection)
        {
            try
            {
                SqlCommand cmd;

                cmd = new SqlCommand("CREATE TABLE novel " +
                                     "(novel_id INT PRIMARY KEY," +
                                     "title NVARCHAR(100) NOT NULL," +
                                     "state INT NOT NULL," +
                                     "source TEXT NOT NULL," +
                                     "source_id INT NOT NULL," +
                                     "last_played_chapter_id INT," +
                                     "updated_chapter_count INT NOT NULL," +
                                     "rank INT NOT NULL)", connection);
                cmd.ExecuteNonQuery();

                cmd = new SqlCommand("CREATE TABLE replacement_rule " +
                                     "(rule_id INT IDENTITY(0,1) PRIMARY KEY," +
                                     "type TEXT NOT NULL," +
                                     "original NVARCHAR(100) NOT NULL," +
                                     "replacement NVARCHAR(100))", connection);
                cmd.ExecuteNonQuery();

            }
            catch
            {
                Console.WriteLine("Table not created");
                throw;
            }
            return false;
        }

        public static void AddNovel(Novel n)
        {
            using (SqlConnection connection = GetNovelDB())
            {
                //"INSERT INTO novel (novel_id, title,state,last_played_chapter_id,source,source_id,updated_chapter_count,rank) VALUES(@novel_id,@title,@state,@last_played_chapter_id,@source,@source_id,@updated_chapter_count,@rank)";
                SqlCommand cmd = new SqlCommand(insertNovelCommand, connection);
                cmd.Parameters.AddWithValue("@novel_id", n.NovelTitle.GetHashCode());
                cmd.Parameters.AddWithValue("@title", n.NovelTitle);
                cmd.Parameters.AddWithValue("@state", (int)n.State);
                cmd.Parameters.AddWithValue("@last_played_chapter_id", n.LastPlayedChapterID);
                //cmd.Parameters.AddWithValue("@source", n.Source.ToString());
                //cmd.Parameters.AddWithValue("@source_id", n.SourceID);
                cmd.Parameters.AddWithValue("@updated_chapter_count", n.NewChapterCount);
                cmd.Parameters.AddWithValue("@rank", n.Rank);

                cmd.ExecuteNonQuery();
            }

        }
    }
}
