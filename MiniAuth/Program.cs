using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace MiniAuth
{
    /// <summary>
    /// Application entry point.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Entry point method.
        /// </summary>
        /// <param name="args">Application arguments.</param>
        public static void Main(string[] args)
        {
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build()
                .Run();
        }
    }
}
