using ExpenseTracker.Common.Interfaces.Models;
using ExpenseTracker.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace ExpenseTracker.UOW.Tests
{
    public abstract class UnitTestBase
    {
        protected Mock<ExpenseTrackerContext> context { get; private set; }

        public UnitTestBase()
        {
            var contextOptions = new Mock<DbContextOptions<ExpenseTrackerContext>>();
            contextOptions.Setup(o => o.ContextType).Returns(typeof(ExpenseTrackerContext));

            var builder = new Mock<DbContextOptionsBuilder<ExpenseTrackerContext>>();
            builder.Setup(o => o.Options).Returns(contextOptions.Object);

            context = new Mock<ExpenseTrackerContext>(builder.Object.Options);

            var db = new Mock<DatabaseFacade>(context.Object);
            db.Setup(o => o.BeginTransaction());
            db.Setup(o => o.CommitTransaction());
            db.Setup(o => o.RollbackTransaction());

            context.Setup(o => o.Database).Returns(db.Object);
        }

        protected ILogger<T> GetLogger<T>() => new Mock<ILogger<T>>().Object;

        protected ExpenseTrackerContext GetContext() => context.Object;

        protected void AssertSuccessCase(IBaseResponse response)
        {
            Assert.NotNull(response);
            Assert.NotNull(response.Result);
            Assert.True(response.Result.IsSuccessful, "Expected the result to be successful but it is not!");
            Assert.Null(response.Result.Errors);
        }
        protected void AssertFailCase(IBaseResponse response)
        {
            Assert.NotNull(response);
            Assert.NotNull(response.Result);
            Assert.False(response.Result.IsSuccessful, "Expected the result to be failure but it is not!");
            Assert.NotNull(response.Result.Errors);
            Assert.NotEmpty(response.Result.Errors);
        }
    }
}
