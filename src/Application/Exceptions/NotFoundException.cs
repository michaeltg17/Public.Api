namespace Application.Exceptions
{
    public class NotFoundException(string entityName, long id) : AppException($"{entityName} with id '{id}' was not found.")
    {
    }
}
