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
            Assert.True(response.Result.IsSuccessful, "Expected the result to be successful but it is not!");
            Assert.Null(response.Result.Errors);
        }
        protected void AssertSingleErrorCase(BaseResponse response, string errorCode)
        {
            Assert.NotNull(response);
            Assert.NotNull(response.Result);
            Assert.False(response.Result.IsSuccessful);
            Assert.Single(response.Result.Errors);
            Assert.Equal(errorCode, response.Result.Errors[0].ErrorCode);
        }
        protected void AssertMultipleErrorCase(BaseResponse response, params string[] errorCodes)
        {
            Assert.NotNull(response);
            Assert.NotNull(response.Result);
            Assert.False(response.Result.IsSuccessful);
            Assert.Equal(errorCodes.Length, response.Result.Errors.Count);
            foreach (string errorCode in errorCodes)
            {
                Assert.Contains(response.Result.Errors, q => q.ErrorCode == errorCode);
            }
        }
    }
}
