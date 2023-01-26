namespace NewCallback.EndpointDefinitions;

/// <summary>
/// Standardize endpoint definitions
/// <para>From on https://www.youtube.com/watch?v=4ORO-KOufeU</para>
/// </summary>
public interface IEndpointDefinition
{
    void DefineServices(IServiceCollection services);
    void DefineEndpoints(WebApplication app);
}