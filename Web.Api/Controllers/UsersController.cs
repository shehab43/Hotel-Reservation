using Application.UseCases.Users.Register;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        public sealed record Request(string Email, string FirstName, string LastName, string Password);
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Result<Guid>> Register(Request request)
        {
            var command =new RegisterUserCommand(request.FirstName, request.LastName, request.Email, request.Password);
            var result = await _mediator.Send(command);
            return (Result<Guid>)result.Match(Results.Ok, CustomResults.Problem);
        }
    }
}
