using Bookify.Application.Abstractions.Messaging;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace Bookify.Application.Abstractions.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IBaseCommand
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;
        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators ?? throw new ArgumentNullException(nameof(validators));
        }
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (!_validators.Any())
            {
                return await next();
            }

            var context = new ValidationContext<TRequest>(request);
            var failures = _validators
                .Select(v => v.Validate(context))
                .Where(result => result.Errors.Any())
                .SelectMany(result => result.Errors)
                .Select(ValidationFailure => new ValidationFailure(ValidationFailure.PropertyName, ValidationFailure.ErrorMessage))
                .ToList();
            if (failures.Any())
            {
                throw new ValidationException(failures);
            }
            return await next();
        }
    }
}
