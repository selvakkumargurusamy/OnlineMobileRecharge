using MobileRecharge.Application.HttpService;

namespace MobileRecharge.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        ConfigureServices(builder.Services, builder.Configuration);

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }

    public static void ConfigureServices(IServiceCollection services, ConfigurationManager configuration)
    {
        // Add services to the container.

        services.Configure<AppSettings>(configuration.GetSection("AppSettings"));

        services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddAutoMapper(typeof(MappingProfile));

        services.AddDbContext<AppDbContext>(options => options.UseSqlServer(configuration.GetConnectionString(nameof(AppDbContext))!));

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
