using Microsoft.AspNet.OData;
using System;
using System.Linq;

namespace ExpenseTracker.Web.Api
{
    public class UsersController : ODataController
    {
        [EnableQuery]
        public IQueryable<User> Get()
        {
            return new string[] { "Alice", "Bob", "Chloe", "Dorothy", "Emma", "Fabian", "Garry", "Hannah", "Julian" }
                .Select(v => new User { Username = v, Id = Guid.NewGuid().ToString() })
                .AsQueryable();
        }
    }
}
