namespace FunctionalTests
{
    public abstract class ApiTest
    {
    }

    public static class Routes
    {
        public const string ApiUrl = "http://localhost:80";
        public const string SaveImageGroup = ApiUrl + "/SaveImageGroup";
        public const string GetImageGroup = ApiUrl + "/GetImageGroup";
        public const string GetImage = ApiUrl + "/GetImage";
    }
}
