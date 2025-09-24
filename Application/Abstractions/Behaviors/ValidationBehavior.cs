using FluentValidation;
using FluentValidation.Results;
using MediatR;
using SharedKernel;
using System.Linq;

namespace Application.Abstractions.Behaviors
{
    public sealed class ValidationBehavior<TRequest, TResponse> : 
                        IPipelineBehavior<TRequest,Result<TResponse>>
                        where TRequest : IRequest<Result<TResponse>>

    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<Result<TResponse>> Handle(
                                                TRequest request,
                                                RequestHandlerDelegate<Result<TResponse>> next,
                                                CancellationToken cancellationToken)
        {
            ValidationFailure[] validationFailures =await ValidateAsync(request, _validators);
            if (validationFailures.Length == 0)
            {
                return await next();

            }
            return Result.Failure<TResponse>(CreateValidationError(validationFailures));

        }

        private async Task<ValidationFailure[]> ValidateAsync(TRequest request, IEnumerable<IValidator<TRequest>> validators)
        {
            if (validators.Any())
            {
                return [];
            }
            var context = new ValidationContext<TRequest>(request);

            ValidationResult[] validationResults = await Task.WhenAll(
                validators.Select(validator => validator.ValidateAsync(context)));

            ValidationFailure[] validationFailures = validationResults
                .Where(validationResult => !validationResult.IsValid)
                .SelectMany(validationResult => validationResult.Errors)
                .ToArray();
            return validationFailures;

        }
        private static ValidationError CreateValidationError(ValidationFailure[] validationFailures) =>
            new(validationFailures.Select(f => Error.Problem(f.ErrorCode, f.ErrorMessage)).ToArray());

       
    }
}