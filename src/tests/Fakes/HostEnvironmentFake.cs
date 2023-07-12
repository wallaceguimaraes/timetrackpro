using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;
using tests.Fakes;

namespace test.Fakes
{
    public class HostEnvironmentFake : IWebHostEnvironment
    {
        public string EnvironmentName { get; set; }
        public string ApplicationName { get; set; }
        public string WebRootPath { get; set; }
        public IFileProvider WebRootFileProvider { get; set; }
        public string ContentRootPath { get; set; }
        public IFileProvider ContentRootFileProvider { get; set; }

        public HostEnvironmentFake()
        {
            EnvironmentName = "Testing";
            ApplicationName = typeof(StartupFake).Assembly.GetName().Name;
            ContentRootPath = AppContext.BaseDirectory.Replace(@".test\bin\Debug\netcoreapp6.0", "");
            WebRootPath = ContentRootPath;
            ContentRootFileProvider = new PhysicalFileProvider(ContentRootPath);
            WebRootFileProvider = new PhysicalFileProvider(ContentRootPath);
        }
    }
}