namespace Services.Results;

internal record FluentInvalidServiceResult(ValidationResult ValidationResult)
    : FluentValidationErrorServiceResult<object>(ValidationResult);

internal record FluentValidationErrorServiceResult<T> : ValidationErrorServiceResult<T> where T : class
{
    public FluentValidationErrorServiceResult(ValidationResult validationResult)
        : base([])
        => Errors = validationResult.Errors.ConvertAll(q => KeyValuePair.Create(q.PropertyName, q.ErrorMessage));
}
