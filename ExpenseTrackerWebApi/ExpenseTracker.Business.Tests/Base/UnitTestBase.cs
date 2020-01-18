using ExpenseTracker.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using Xunit.Abstractions;

namespace ExpenseTracker.Business.Tests
{
    public abstract class UnitTestBase : IDisposable
    {
        private LoggerFactory loggerFactory;
        protected ExpenseTrackerContext DbContext { get; private set; }

        public UnitTestBase(ITestOutputHelper testOutputHelper, string dbName = "defaultDb")
        {
            var options = new DbContextOptionsBuilder<ExpenseTrackerContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .EnableSensitiveDataLogging(true)
                .Options;

            DbContext = new ExpenseTrackerContext(options);

            DbContext.Database.EnsureCreated();

            loggerFactory = new LoggerFactory();
            loggerFactory.AddProvider(new XunitLoggerProvider(testOutputHelper));

            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .MinimumLevel.Debug()
                .WriteTo.Logger(l => l.WriteTo.Console(Serilog.Events.LogEventLevel.Debug))
                .CreateLogger();
        }

        public void Dispose() => DbContext.Database.EnsureDeleted();

        protected ILogger<T> GetLogger<T>()
        {
            return loggerFactory.CreateLogger<T>();
        }
    }

    public class XunitLoggerProvider : ILoggerProvider
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public XunitLoggerProvider(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        Microsoft.Extensions.Logging.ILogger ILoggerProvider.CreateLogger(string categoryName)
        {
            return new XunitLogger(_testOutputHelper, categoryName);
        }

        public void Dispose()
        { }
    }

    public class XunitLogger : Microsoft.Extensions.Logging.ILogger
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly string _categoryName;

        public XunitLogger(ITestOutputHelper testOutputHelper, string categoryName)
        {
            _testOutputHelper = testOutputHelper;
            _categoryName = categoryName;
        }

        public IDisposable BeginScope<TState>(TState state)
            => NoopDisposable.Instance;

        public bool IsEnabled(LogLevel logLevel)
            => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            _testOutputHelper.WriteLine($"{_categoryName} [{eventId}] {formatter(state, exception)}");
            if (exception != null)
                _testOutputHelper.WriteLine(exception.ToString());
        }

        private class NoopDisposable : IDisposable
        {
            public static NoopDisposable Instance = new NoopDisposable();
            public void Dispose()
            { }
        }
    }
}
