using Microsoft.AspNetCore.Mvc.Testing;

namespace MobileRecharge.FunctionalUnitTest
{
    public class CustomWepApiFactory : WebApplicationFactory<Program>
    {
        private readonly string _testContainerConnectionString;

        public CustomWepApiFactory(string testContainerConnectionString)
        {
            _testContainerConnectionString = testContainerConnectionString;
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);

            builder.ConfigureAppConfiguration(config =>
            {
                config.AddInMemoryCollection(new Dictionary<string, string?>
            {
            {"ConnectionStrings:AppDbContext",_testContainerConnectionString }
            });
            });
        }
    }
}
