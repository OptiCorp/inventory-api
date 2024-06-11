using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Logging;
using Microsoft.Identity.Web;
using Serilog;
using Inventory.Models;
using Inventory.Services;
using Inventory.Utilities;
using Inventory.Validations.CategoryValidations;
using Inventory.Validations.DocumentTypeValidations;
using Inventory.Validations.DocumentValidations;
using Inventory.Validations.ItemTemplateValidations;
using Inventory.Validations.ItemValidations;
using Inventory.Validations.ListValidations;
using Inventory.Validations.LocationValidations;
using Inventory.Validations.PreCheckValidations;
using Inventory.Validations.SizeValidations;
using Inventory.Validations.VendorValidations;
using Microsoft.OpenApi.Models;
using Infrastructure.Persistence.ServiceBus;
using Azure.Messaging.ServiceBus;

namespace Inventory;

public class Startup(IConfiguration configuration)
{
    private IConfiguration Configuration { get; } = configuration;

    public void ConfigureServices(IServiceCollection services)
    {

        services.AddMvc(options =>
        {
            options.SuppressAsyncSuffixInActionNames = false;
        });

        IdentityModelEventSource.ShowPII = true;
        ConfigureAuthenticationAndAuthorization(services);

        services.AddIdentityServer()
            .AddSigningCredentials();
        // Add CORS services
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAllHeaders",
                builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

        });

        var azureCredentials = new DefaultAzureCredential();

        services.AddSingleton<ServiceBusClient>(serviceProvider =>
        {
            string serviceBusNamespace = Configuration["ServiceBus:Namespace"] ?? throw new Exception("Missing namespace in configuration");
            return new ServiceBusClient($"{serviceBusNamespace}.servicebus.windows.net", azureCredentials);
        });

        services.AddSwaggerGen(c =>
        {
            c.EnableAnnotations();
            c.AddSecurityRequirement(new OpenApiSecurityRequirement() {
                 {
                    new OpenApiSecurityScheme {
                        Reference = new OpenApiReference {
                            Type = ReferenceType.SecurityScheme,
                            Id = "oauth2"
                        },
                        Scheme = "oauth2",
                        Name = "oauth2",
                        In = ParameterLocation.Header,
                    },
                    new List <string> ()
                }
            });
            c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,
                Name = "oauth2",
                Description = "OAuth2.0 auth code with implicit grant",
                Flows = new OpenApiOAuthFlows
                {
                    Implicit = new OpenApiOAuthFlow
                    {
                        AuthorizationUrl = new Uri("https://login.microsoftonline.com/91020923-529b-458c-8796-98af46bf6003/oauth2/v2.0/authorize"),
                        TokenUrl = new Uri("https://login.microsoftonline.com/91020923-529b-458c-8796-98af46bf6003/oauth2/v2.0/token"),
                        Scopes = new Dictionary<string, string>{
                            { "api://a053137b-4efd-46a8-bf41-7ed3fc49f3be/Inventory.ReadAll", "access inventory api" }
                        }
                    }
                },

            });
        });

        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IItemService, ItemService>();
        services.AddScoped<IListService, ListService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<ILocationService, LocationService>();
        services.AddScoped<IVendorService, VendorService>();
        services.AddScoped<IDocumentService, DocumentService>();
        services.AddScoped<IItemTemplateService, ItemTemplateService>();
        services.AddScoped<ISizeService, SizeService>();
        services.AddScoped<IPreCheckService, PreCheckService>();
        services.AddScoped<IDocumentTypeService, DocumentTypeService>();
        services.AddScoped<ILogEntryService, LogEntryService>();
        services.AddScoped<IUserUtilities, UserUtilities>();
        services.AddScoped<IGeneralUtilities, GeneralUtilities>();
        services.AddScoped<ICategoryCreateValidator, CategoryCreateValidator>();
        services.AddScoped<ICategoryUpdateValidator, CategoryUpdateValidator>();
        services.AddScoped<IItemCreateValidator, ItemCreateValidator>();
        services.AddScoped<IItemUpdateValidator, ItemUpdateValidator>();
        services.AddScoped<IItemTemplateCreateValidator, ItemTemplateCreateValidator>();
        services.AddScoped<IItemTemplateUpdateValidator, ItemTemplateUpdateValidator>();
        services.AddScoped<IListCreateValidator, ListCreateValidator>();
        services.AddScoped<IListUpdateValidator, ListUpdateValidator>();
        services.AddScoped<ILocationCreateValidator, LocationCreateValidator>();
        services.AddScoped<ILocationUpdateValidator, LocationUpdateValidator>();
        services.AddScoped<IVendorCreateValidator, VendorCreateValidator>();
        services.AddScoped<IVendorUpdateValidator, VendorUpdateValidator>();
        services.AddScoped<IPreCheckCreateValidator, PreCheckCreateValidator>();
        services.AddScoped<IPreCheckUpdateValidator, PreCheckUpdateValidator>();
        services.AddScoped<ISizeCreateValidator, SizeCreateValidator>();
        services.AddScoped<ISizeUpdateValidator, SizeUpdateValidator>();
        services.AddScoped<IDocumentTypeCreateValidator, DocumentTypeCreateValidator>();
        services.AddScoped<IDocumentTypeUpdateValidator, DocumentTypeUpdateValidator>();
        services.AddScoped<IDocumentUploadValidator, DocumentUploadValidator>();

        // services.AddHostedService<UserCreateHandler>();
        // services.AddHostedService<UserUpdateHandler>();
        // services.AddHostedService<UserDeleteHandler>();
        services.AddHostedService<ServiceBusChecklistTemplateProcessor>();

        services.AddControllers();



        // Add DbContext
        var connectionString = GetSecretValueFromKeyVault(Configuration["AzureKeyVault:ConnectionStringSecretName"]);


        services.AddDbContext<InventoryDbContext>(options => options.UseSqlServer(connectionString));


        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
    }

    private void ConfigureAuthenticationAndAuthorization(IServiceCollection services)
    {

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddMicrosoftIdentityWebApi(options =>
            {
                Configuration.Bind("AzureAd", options);
                options.TokenValidationParameters.NameClaimType = "name";
            }, options => { Configuration.Bind("AzureAd", options); });


        services.AddAuthorizationBuilder()
            .AddPolicy("AuthZPolicy", policyBuilder =>
                policyBuilder.Requirements.Add(new ScopeAuthorizationRequirement { RequiredScopesConfigurationKey = "AzureAd.Scopes" }));

        services.AddControllersWithViews()
            .AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );
        services.AddRazorPages();
    }



    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, InventoryDbContext dbContext)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            // app.UseSwagger();
            // app.UseSwaggerUI();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseSerilogRequestLogging();

        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "swagger");
            options.OAuthAppName("Swagger Client");
            options.OAuthClientId("a053137b-4efd-46a8-bf41-7ed3fc49f3be");
            options.OAuthClientSecret(Configuration["AzureAd:ClientSecret"]);
            options.OAuthUseBasicAuthenticationWithAccessCodeGrant();
        });

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", Serilog.Events.LogEventLevel.Warning)
            .WriteTo.Console()
            .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        dbContext.Database.Migrate();

        app.UseHttpsRedirection();

        app.UseRouting();

        // Enable CORS
        app.UseCors("AllowAllHeaders");

        app.UseAuthentication();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }

    private string? GetSecretValueFromKeyVault(string? secretName)
    {
        var keyVaultUrl = Configuration["AzureKeyVault:VaultUrl"];
        var credential = new DefaultAzureCredential();
        if (keyVaultUrl == null) return null;
        var client = new SecretClient(new Uri(keyVaultUrl), credential);
        var secret = client.GetSecret(secretName);
        return secret.Value.Value;
    }

}