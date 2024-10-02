namespace SimpleDB;

using System.Reflection;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.FileProviders;
using SimpleDB.Records;
using System.Linq;

public class SQLiteDatabase : IDatabaseRepository<Cheep>
{
    private static readonly string databasePath = "./sqliteDB.db";
    private static readonly SQLiteDatabase instance = new();

    public readonly SqliteConnection connection;

    static SQLiteDatabase()
    {

    }
    private SQLiteDatabase()
    {
        connection = new SqliteConnection($"Data Source={databasePath}");

        connection.Open();
        var command = connection.CreateCommand();
        command.CommandText = "SELECT COUNT(name) FROM sqlite_master WHERE type='table' AND name=@table_name";
        command.Parameters.AddWithValue("@table_name", "message");
        if (command.ExecuteScalar()!.ToString() == "0")
        {
            Console.WriteLine("Creating new database");
            RunSQLFile("data/schema.sql");
            RunSQLFile("data/dump.sql");
        }
    }

    public static SQLiteDatabase Instance
    {
        get
        {
            return instance;
        }
    }

    private void RunSQLFile(string path)
    {
        var command = connection.CreateCommand();
        var fileProvider = new EmbeddedFileProvider(Assembly.GetExecutingAssembly());
        using var stream = fileProvider.GetFileInfo(path).CreateReadStream();
        using var StreamReader = new StreamReader(stream);
        command.CommandText = StreamReader.ReadToEnd();
        command.ExecuteNonQuery();
    }

    /// <summary>
    /// Read cheeps from the database of a specific author.
    /// </summary>
    /// <param name="username">The author to read cheeps from</param>
    /// <param name="page">The query page</param>
    /// <returns>Task list of Cheeps from author</returns>
    public Task<List<Cheep>> Read(string username, int? page = 1)
    {
        var limit = 32;
        // Offset rows based on the page number and limit of rows to read
        var offset = page == 1 ? 0 : limit * (page - 1);

        var command = connection.CreateCommand();
        command.CommandText = @"
        SELECT u.username, m.text, m.pub_date
        FROM message m
        JOIN user u ON m.author_id = u.user_id
        WHERE u.username = @username
        LIMIT @limit OFFSET @limit * @offset
        ";
        command.Parameters.AddWithValue("@username", username);
        command.Parameters.AddWithValue("@limit", limit);
        command.Parameters.AddWithValue("@offset", offset);

        var reader = command.ExecuteReader();
        return Task.FromResult(ReturnCheeps(reader));
    }

    /// <summary>
    /// Read unordered cheeps from the database.
    /// </summary>
    /// <param name="page"></param>
    /// <returns>Task List of Cheeps</returns>
    public Task<List<Cheep>> Read(int? page = 1)
    {
        var limit = 32;
        // Offset rows based on the page number and limit of rows to read
        var offset = page == 1 ? 0 : limit * (page - 1);

        var command = connection.CreateCommand();
        command.CommandText = @"SELECT u.username, m.text, m.pub_date FROM message m
        JOIN user u ON m.author_id = u.user_id
        LIMIT @limit OFFSET @offset
        ";
        // Add LIMIT and OFFSET to the query
        command.Parameters.AddWithValue("@limit", limit);
        command.Parameters.AddWithValue("@offset", offset);

        var reader = command.ExecuteReader();
        return Task.FromResult(ReturnCheeps(reader));
    }

    /// <summary>
    /// Return a list of Cheeps from the SqliteDataReader.
    /// </summary>
    /// <param name="reader">SqliteDataReader from command</param>
    /// <returns>List of Cheeps</returns>
    private static List<Cheep> ReturnCheeps(SqliteDataReader reader)
    {
        List<Cheep> data = [];
        while (reader.Read())
        {
            data.Add(new(reader.GetString(0), reader.GetString(1), reader.GetInt64(2)));
        }
        return data;
    }

    public void Store(Cheep record)
    {
        var transaction = connection.BeginTransaction(System.Data.IsolationLevel.Serializable);
        var command = connection.CreateCommand();
        command.CommandText = "SELECT user_id FROM user WHERE username = @username";
        command.Parameters.AddWithValue("@username", record.Author);
        var reader = command.ExecuteReader();
        int userId;
        if (reader.HasRows)
        {
            reader.Read();
            userId = reader.GetInt32(0);
        }
        else
        {
            command = connection.CreateCommand();
            command.CommandText = "insert into user (username, email) values (@username, \"not implemented\");";
            command.Parameters.AddWithValue("@username", record.Author);
            command.ExecuteNonQuery();

            command = connection.CreateCommand();
            command.CommandText = "SELECT user_id FROM user WHERE username = @username";
            command.Parameters.AddWithValue("@username", record.Author);
            reader = command.ExecuteReader();
            reader.Read();
            userId = reader.GetInt32(0);
        }

        command = connection.CreateCommand();
        command.CommandText = "insert into message (author_id, text, pub_date) values (@id, @message, @timestamp);";
        command.Parameters.AddWithValue("@id", userId);
        command.Parameters.AddWithValue("@message", record.Message);
        command.Parameters.AddWithValue("@timestamp", record.Timestamp);
        command.ExecuteNonQuery();

        transaction.Commit();
    }


    public static void Clear()
    {
        File.Delete(databasePath);
    }
}
