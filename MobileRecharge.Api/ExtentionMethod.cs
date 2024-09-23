using Microsoft.AspNetCore.Hosting;
using MobileRecharge.Application.HttpService;

namespace MobileRecharge.Api
{
    public static class ExtentionMethod
    {
        public static void ConfigureServices(this IServiceCollection services, ConfigurationManager configuration)
        {
            // Add services to the container.

            services.Configure<AppSettings>(configuration.GetSection("AppSettings"));

            services.AddControllers();
            services.AddMvc();
            services.AddMvc().AddApplicationPart(typeof(Program).Assembly);

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddAutoMapper(typeof(MappingProfile));

            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(configuration.GetConnectionString(nameof(AppDbContext))!, b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)));

            services.AddHttpClient("Payment", client =>
            {
                client.BaseAddress = new Uri("https://localhost:7072/");
            });

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IBeneficiaryRepository, BeneficiaryRepository>();
            services.AddScoped<IRechargeTransactionRepository, RechargeTransactionRepository>();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IBeneficiaryService, BeneficiaryService>();
            services.AddScoped<IRechargeTransactionService, RechargeTransactionService>();
            services.AddScoped<IHttpService, HttpService>();

            services.AddMediatR(
            cfg =>
            {
                cfg.RegisterServicesFromAssemblies(typeof(Program).Assembly, typeof(IUserService).Assembly);
            });
        }

    }
}
