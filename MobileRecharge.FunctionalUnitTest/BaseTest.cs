using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MobileRecharge.Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testcontainers.MsSql;

namespace MobileRecharge.FunctionalUnitTest
{
    public abstract class BaseTest : IAsyncLifetime
    {
        private readonly MsSqlContainer _dbContainer = new MsSqlBuilder()
            .WithImage("mcr.microsoft.com/mssql/server:2019-latest").Build();


        private CustomWepApiFactory? _wepApiFactory { get; set; }

        public async Task InitializeAsync()
        {
            await _dbContainer.StartAsync();
            _wepApiFactory = new CustomWepApiFactory(_dbContainer.GetConnectionString());
            using var scope = _wepApiFactory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            await dbContext.Database.MigrateAsync();
        }

        public async Task DisposeAsync()
        {
            await _dbContainer.StopAsync();

            if (_wepApiFactory != null)
                await _wepApiFactory.DisposeAsync();
        }

        protected HttpClient CreateClient() => _wepApiFactory!.CreateClient();
    }
}
