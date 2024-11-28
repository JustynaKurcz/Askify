namespace Askify.Shared.Endpoints;

public interface IEndpointDefinition
{
    void DefineEndpoints(IEndpointRouteBuilder endpoints);
}