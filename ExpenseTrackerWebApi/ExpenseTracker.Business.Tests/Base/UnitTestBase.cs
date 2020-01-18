using ExpenseTracker.Models.Base;
using ExpenseTracker.Persistence.Context;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using Xunit;
using Xunit.Abstractions;

namespace ExpenseTracker.Business.Tests
{
    public abstract class UnitTestBase : IDisposable
    {
        private LoggerFactory loggerFactory;
        protected ExpenseTrackerContext DbContext { get; private set; }

        public UnitTestBase(ITestOutputHelper testOutputHelper)
        {
            var builder = new DbContextOptionsBuilder<ExpenseTrackerContext>();

            var connectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = ":memory:" };
            var connectionString = connectionStringBuilder.ToString();
            var connection = new SqliteConnection(connectionString);
            connection.Open();
            builder.UseSqlite(connection);

            DbContext = new ExpenseTrackerContext(builder.Options);
            DbContext.Database.EnsureCreated();

            loggerFactory = new LoggerFactory();
            loggerFactory.AddProvider(new XunitLoggerProvider(testOutputHelper));
        }

        public void Dispose() => DbContext.Database.EnsureDeleted();

        protected ILogger<T> GetLogger<T>()
        {
            return loggerFactory.CreateLogger<T>();
        }

        protected void AssertSuccessCase(BaseResponse response)
        {
            Assert.NotNull(response);
            Assert.NotNull(response.Result);
            Assert.True(response.Result.IsSuccessful);
            Assert.Null(response.Result.Errors);
        }
    }
}
