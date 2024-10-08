﻿

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

        public static IServiceCollection AddKeycloakAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            // https://dev.to/kayesislam/integrating-openid-connect-to-your-application-stack-25ch
            services
                .AddAuthentication()
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = Convert.ToBoolean($"{configuration["Keycloak:require-https"]}");
                    x.MetadataAddress = $"{configuration["Keycloak:server-url"]}/realms/myrealm/.well-known/openid-configuration";
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        RoleClaimType = "groups",
                        NameClaimType = $"{configuration["Keycloak:name_claim"]}",
                        ValidAudience = $"{configuration["Keycloak:audience"]}",
                        // https://stackoverflow.com/questions/60306175/bearer-error-invalid-token-error-description-the-issuer-is-invalid
                        ValidateIssuer = Convert.ToBoolean($"{configuration["Keycloak:validate-issuer"]}"),
                    };
                });

            //services.AddAuthorization(o =>
            //{
            //    o.DefaultPolicy = new AuthorizationPolicyBuilder()
            //        .RequireAuthenticatedUser()
            //        .RequireClaim("email_verified", "true")
            //        .Build();
            //});

            return services;
        }

        public static IServiceCollection AddSwaggerApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(c =>
            {
                // See https://stackoverflow.com/questions/66265594/oauth-implementation-in-asp-net-core-using-swagger
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Mobile Recharge API v1.0", Version = "v1" });
                c.AddSecurityDefinition("OAuth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        Implicit = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri($"{configuration["Keycloak:server-url"]}/realms/myrealm/protocol/openid-connect/auth"),
                        }
                    }
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement{
                        {
                            new OpenApiSecurityScheme{
                                Reference = new OpenApiReference{
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "OAuth2" //The name of the previously defined security scheme.
                                }
                            },
                            new string[] {}
                        }
                    });
            });

            return services;
        }
    }
}
