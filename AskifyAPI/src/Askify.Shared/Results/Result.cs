namespace Askify.Shared.Results;

public class Result<TValue, TError>
{
    private readonly TValue? _value;
    private readonly TError? _error;

    private readonly bool _isSuccess;

    private Result(TValue value)
    {
        _isSuccess = true;
        _value = value;
    }

    private Result(TError error)
    {
        _isSuccess = false;
        _error = error;
    }

    public static implicit operator Result<TValue, TError>(TValue value) => new(value);
    public static implicit operator Result<TValue, TError>(TError error) => new(error);

    public TResult Match<TResult>(Func<TValue, TResult> success, Func<TError, TResult> failure)
    {
        return _isSuccess ? success(_value!) : failure(_error!);
    }
}