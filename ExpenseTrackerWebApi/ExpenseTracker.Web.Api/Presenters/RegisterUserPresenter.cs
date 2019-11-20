using ExpenseTracker.Web.Api.Core.Dto.UseCaseResponses;
using ExpenseTracker.Web.Api.Core.Interfaces;
using ExpenseTracker.Web.Api.Serialization;
using System.Net;

namespace ExpenseTracker.Web.Api.Presenters
{
    public sealed class RegisterUserPresenter : IOutputPort<RegisterUserResponse>
    {
        public JsonContentResult ContentResult { get; }

        public RegisterUserPresenter()
        {
            ContentResult = new JsonContentResult();
        }

        public void Handle(RegisterUserResponse response)
        {
            ContentResult.StatusCode = (int)(response.Success ? HttpStatusCode.OK : HttpStatusCode.BadRequest);
            ContentResult.Content = JsonSerializer.SerializeObject(response);
        }
    }
}
