using Application.Abstractions.Authentication;
using Domain.Abstractions.Contracts;
using Domain.Entities.Users;
using Domain.Users;
using MediatR;
using SharedKernel;


namespace Application.UseCases.Users.Register
{
    internal sealed class RegisterUserCommandHandler(IGenericRepository<User> genericRepository, IPasswordHasher passwordHasher) : IRequestHandler<RegisterUserCommand,Result<Guid>>
    {
        public async Task<Result<Guid>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            if (await genericRepository.AnyAsync(u => u.Email == request.Email)) 
                return Result.Failure<Guid>(UserErrors.EmailNotUnique);

            var user = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Password = passwordHasher.Hash(request.Password)
            };
                          await  genericRepository.AddAsync(user);
             var reslt =  await genericRepository.SaveChangesAsync(cancellationToken);
             if(reslt < 0)
                return Result.Failure<Guid>(UserErrors.EmailNotUnique);
            return user.Id;

        }
    }
}
