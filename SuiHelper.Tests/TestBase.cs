using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using SuiHelper.Services;
using SuiHelper.Services.Manager;

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
            services.AddSingleton<IWebHostEnvironment, MockWebHostEnvironment>();
            services.AddSingleton<IBillService, BillService>();
            services.AddSingleton<IBillManager, BillManager>();
            
            ServiceProvider = services.BuildServiceProvider();
            
            System.Text.Encoding.RegisterProvider (System.Text.CodePagesEncodingProvider.Instance);
        }
        
        public void Dispose()
        {
        }
    }
    
    public class MockWebHostEnvironment: IWebHostEnvironment
    {
        public string ApplicationName { get; set; }
        public IFileProvider ContentRootFileProvider { get; set; }
        public string ContentRootPath { get; set; } = AppDomain.CurrentDomain.BaseDirectory;
        public string EnvironmentName { get; set; }
        public IFileProvider WebRootFileProvider { get; set; }
        public string WebRootPath { get; set; }
    }
}