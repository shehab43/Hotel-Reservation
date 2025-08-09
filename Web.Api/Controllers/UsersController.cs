using Application.UseCases.EmailVerification.Query.GetById;
using Application.UseCases.Users.Command.Register;

using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Controllers
{
    [Route("Users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        public sealed record Request(string Email, string FirstName, string LastName, string Password);
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        public async Task<IResult> Register([FromBody]Request request)
        {
            var command =new RegisterUserCommand(request.Email, request.FirstName, request.LastName, request.Password);
            var result = await _mediator.Send(command);
           
            return result.IsSuccess ? Results.Ok(result.Value) : result.Problem();
        }
        [HttpGet("VerifyEmail")]
        public async Task<IResult> VerifyEmail(Guid Token)
        {
            var command = new EmailVerificationCommand(Token);
            var result = await _mediator.Send(command);
          return  result.IsSuccess ?
                         Results.Ok(result.Value) : result.Problem();

        }
    }
}
