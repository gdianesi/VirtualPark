using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace VirtualPark.DataAccess.Test;

internal sealed class DbContextBuilder
{
    public static (SqlContext Context, SqliteConnection Connection) BuildTestDbContext()
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();

        var options = new DbContextOptionsBuilder<SqlContext>()
            .UseSqlite(connection)
            .Options;

        var context = new SqlContext(options);
        context.Database.EnsureCreated();

        return (context, connection);
    }
}
