namespace FunctionalTests
{
    public abstract class ApiTest
    {
    }

    public static class Routes
    {
        public const string ApiUrl = "https://localhost:5001";
        public const string SaveImageGroup = ApiUrl + "/SaveImageGroup";
        public const string GetImageGroup = ApiUrl + "/GetImageGroup";
        public const string GetImage = ApiUrl + "/GetImage";
    }
}
