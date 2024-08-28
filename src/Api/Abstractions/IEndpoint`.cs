namespace Api.Abstractions;

public interface IEndpoint<T> where T : IEndpointRouteBuilder
{
    void MapEndpoint(T app);
}
