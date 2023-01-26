

namespace NewCallback.EndpointDefinitions
{
    public static class ClientTestEndpointDefinitions
    {
        public static void DefineClientTestEndpoints(this IEndpointRouteBuilder app)
        {
            var tests = app.MapGroup("/tests");
            tests.MapGet("first", GetFirst);
        }

        internal static IResult GetFirst()
        {
            return TypedResults.Ok("Yes");
        }

    }
}
