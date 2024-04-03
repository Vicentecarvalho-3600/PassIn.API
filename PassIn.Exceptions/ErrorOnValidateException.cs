namespace PassIn.Exceptions;
public class ErrorOnValidateException : PassInException
{
    public ErrorOnValidateException(string message) : base(message)
    {
    }
}
