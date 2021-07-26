using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SuiHelper.Helper;
using SuiHelper.Services;

namespace SuiHelper.Tests
{
    public class TestBase : IDisposable
    {
        protected IServiceProvider ServiceProvider;
        
        public TestBase()
        {
            CreateServiceCollection();
        }

        private void CreateServiceCollection()
        {
            var configurationBuilder = new ConfigurationBuilder();
            
            var configuration = configurationBuilder.Build();

            var services = new ServiceCollection();
            services.AddSingleton<BillService>();
            
            ServiceProvider = services.BuildServiceProvider();
        }
        
        public void Dispose()
        {
        }
    }
}