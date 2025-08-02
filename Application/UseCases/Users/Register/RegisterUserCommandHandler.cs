using Application.Abstractions.Authentication;
using Domain.Abstractions.Contracts;
using Domain.Entities.Users;
using Domain.Users;
using MediatR;
using SharedKernel;


namespace Application.UseCases.Users.Register
{
    internal sealed class RegisterUserCommandHandler(
                          IGenericRepository<User> genericRepository, 
                          IPasswordHasher passwordHasher,
                          IEmailSender emailSender) :
                          IRequestHandler<RegisterUserCommand,Result<User>>
    {
        private readonly IEmailSender _emailSender = emailSender;
        public async Task<Result<User>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            if (await genericRepository.AnyAsync(u => u.Email == request.Email)) 
                return Result.Failure<User>(UserErrors.EmailNotUnique);

            var user = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Password = passwordHasher.Hash(request.Password)
            };
             var users =  await genericRepository.AddAsync(user);
             var reslt =  await genericRepository.SaveChangesAsync(cancellationToken);
             if(reslt < 0)

           return Result.Failure<User>(UserErrors.EmailNotUnique);
           await _emailSender.SendEmailAsync(request.Email , "Email Verification" , "Click On Link To Confirem Email");
           return Result.Success(users);

        }
    }
}
