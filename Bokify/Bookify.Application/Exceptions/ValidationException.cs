using Bookify.Application.Abstractions.Behavior;

namespace Bookify.Application.Exceptions;

public sealed class ValidationException(IEnumerable<ValidationError> validationErrors) : Exception
{
    public IEnumerable<ValidationError> Errors { get; } = validationErrors;
}