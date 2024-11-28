namespace Askify.Api.Endpoints.Users;

public static class UserEndpoints
{
    public static string Users => "Users";
    public static string BasePath => $"{RootPath.BasePath}/{Users.ToLowerInvariant()}";
    public static string SignUp => $"{RootPath.BasePath}/users/sign-up";
    public static string SignIn => $"{RootPath.BasePath}/users/sign-in";
}