using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace VirtualPark.DataAccess.Test;

public class SqliteInMemoryDbContext
{
    private static readonly SqliteConnection _connection = new("Data Source=:memory:");

    internal static TestDbContext BuildTestDbContext()
    {
        if(_connection.State != System.Data.ConnectionState.Open)
        {
            _connection.Open();
        }

        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseSqlite(_connection)
            .Options;

        var context = new TestDbContext(options);
        context.Database.EnsureCreated();

        return context;
    }
}
