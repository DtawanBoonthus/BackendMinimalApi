namespace BackendMinimalApi.Endpoints;

public static class TestAuthorizationEndpoints
{
    public static IEndpointRouteBuilder MapTestAuthorization(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/testAuthorizationEndpoints");

        group.MapGet("/getTest", () => Results.Ok(new { status = 200, message = "OK" }))
            .WithName("GetTest")
            .WithTags("TestAuthorization")
            .RequireAuthorization();
        
        return endpoints;
    }
}