namespace EnglishTrainingCenter.Domain.Exceptions;

/// <summary>
/// Base exception for domain business rules
/// </summary>
public class DomainException : Exception
{
    public DomainException(string message) : base(message) { }
    public DomainException(string message, Exception innerException) : base(message, innerException) { }
}

/// <summary>
/// Exception when entity is not found
/// </summary>
public class EntityNotFoundException : DomainException
{
    public EntityNotFoundException(string entityName, int id)
        : base($"{entityName} with ID {id} not found") { }
}

/// <summary>
/// Exception for duplicate entries
/// </summary>
public class DuplicateEntityException : DomainException
{
    public DuplicateEntityException(string message) : base(message) { }
}

/// <summary>
/// Exception for invalid operations
/// </summary>
public class InvalidOperationException : DomainException
{
    public InvalidOperationException(string message) : base(message) { }
}

/// <summary>
/// Exception for validation errors
/// </summary>
public class ValidationException : DomainException
{
    public ValidationException(string message) : base(message) { }
    public ValidationException(string message, Dictionary<string, string[]> errors) 
        : base(message)
    {
        Errors = errors;
    }

    public Dictionary<string, string[]>? Errors { get; }
}
