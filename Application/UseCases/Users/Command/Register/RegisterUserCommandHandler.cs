using Application.Abstractions.Authentication;
using Domain.Abstractions.Contracts;
using Domain.Entities.EmaiVerification;
using Domain.Entities.Users;
using Domain.Users;
using MediatR;
using SharedKernel;


namespace Application.UseCases.Users.Command.Register
{
    internal sealed class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand,Result<User>>
    {
        private readonly IGenericRepository<User> _genericRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IEmailSender _emailSender;
        private readonly IEmailVerfication _emailVerfication;
        private readonly IEmailVerificationLinkFactory _emailVerificationLink;

        public RegisterUserCommandHandler(
            IGenericRepository<User> genericRepository,
            IPasswordHasher passwordHasher,
            IEmailSender emailSender,
            IEmailVerfication emailVerfication,
            IEmailVerificationLinkFactory emailVerificationLink)
        {
            _genericRepository = genericRepository;
            _passwordHasher = passwordHasher;
            _emailSender = emailSender;
            _emailVerfication = emailVerfication;
            _emailVerificationLink = emailVerificationLink;
        }
        public async Task<Result<User>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            if (await _genericRepository.AnyAsync(u => u.Email == request.Email)) 
                return Result.Failure<User>(UserErrors.EmailNotUnique);

            var user = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Password = _passwordHasher.Hash(request.Password)
            };
             var users =  await _genericRepository.AddAsync(user);
             var reslt =  await _genericRepository.SaveChangesAsync(cancellationToken);
             if(reslt < 0)
                 return Result.Failure<User>(UserErrors.EmailNotUnique);
            var Token = new EmailVerificationToken
            { 
                Id =  Guid.NewGuid(),
                UserId = users.Id,
                CreateOnUtc = DateTime.UtcNow,
                ExpirationDate = DateTime.UtcNow.AddDays(1)     
            };
            await  _emailVerfication.add(Token);
            await  _emailVerfication.SaveChanges();
            var Verification = _emailVerificationLink.Create(Token.Id);
            await _emailSender.SendEmailAsync(request.Email , 
                                             "Email Verification" ,
                                             $"Click On Link To Confirem Email<a href='{Verification}'>click here </a>");
           return Result.Success(users);

        }
    }
}
