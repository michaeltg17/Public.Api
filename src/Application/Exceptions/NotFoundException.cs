namespace Application.Exceptions
{
    public class NotFoundException(string entityName, long id) : ApiException($"{entityName} with id '{id}' was not found.")
    {
    }
}
