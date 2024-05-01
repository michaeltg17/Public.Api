using Microsoft.AspNetCore.Mvc;

[assembly: ApiController]
namespace Api
{
    public class Program
    {
        static void Main(string[] args)
        {
            Startup.Run(args);
        }
    }
}