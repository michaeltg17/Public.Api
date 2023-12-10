namespace Application.Exceptions
{
    public class NotFoundException(string message) : ApiException(message)
    {
    }
}
