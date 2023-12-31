using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using FluentValidation;
using Inventory.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Logging;
using Microsoft.Identity.Web;
using Serilog;
using Inventory.Models;
using Inventory.Services;
using Inventory.Utilities;
using Inventory.Utilities.DocumentationUtilities;

namespace inventory

{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
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

            services.AddSwaggerGen(c =>
            {
                c.EnableAnnotations();
            });
            
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IItemService, ItemService>();
            services.AddScoped<IListService, ListService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ILocationService, LocationService>();
            services.AddScoped<IVendorService, VendorService>();
            services.AddScoped<IDocumentationService, DocumentationService>();
            
            services.AddScoped<IUserUtilities, UserUtilities>();
            services.AddScoped<IItemUtilities, ItemUtilities>();
            services.AddScoped<IListUtilities, ListUtilities>();
            services.AddScoped<ICategoryUtilities, CategoryUtilities>();
            services.AddScoped<ILocationUtilities, LocationUtilities>();
            services.AddScoped<IVendorUtilities, VendorUtilities>();
            services.AddScoped<IDocumentationUtilities, DocumentationUtilities>();
            
            services.AddValidatorsFromAssemblyContaining<IAssemblyMarker>();

            services.AddHostedService<UserCreateHandler>();
            services.AddHostedService<UserUpdateHandler>();
            services.AddHostedService<UserDeleteHandler>();

            services.AddControllers();



            // Add DbContext
            var connectionString = GetSecretValueFromKeyVault(Configuration["AzureKeyVault:ConnectionStringSecretName"]);


            services.AddDbContext<InventoryDbContext>(options =>
                options.UseSqlServer(connectionString
            ));




            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
        }

        private void ConfigureAuthenticationAndAuthorization(IServiceCollection services)
        {

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                        .AddMicrosoftIdentityWebApi(options =>
                        {
                            Configuration.Bind("AzureAd", options);
                            options.TokenValidationParameters.NameClaimType = "name";
                        }, options => { Configuration.Bind("AzureAd", options); });


            services.AddAuthorization(config =>
            {
                config.AddPolicy("AuthZPolicy", policyBuilder =>
                policyBuilder.Requirements.Add(new ScopeAuthorizationRequirement() { RequiredScopesConfigurationKey = $"AzureAd.Scopes" }));
            });

            services.AddControllersWithViews();
            services.AddRazorPages();
        }



        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, InventoryDbContext dbContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseSerilogRequestLogging();

            app.UseSwagger();
            app.UseSwaggerUI();

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", Serilog.Events.LogEventLevel.Warning)
                .WriteTo.Console()
                .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            dbContext.Database.Migrate();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

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

        private string GetSecretValueFromKeyVault(string secretName)
        {
            var keyVaultUrl = Configuration["AzureKeyVault:VaultUrl"];
            var credential = new DefaultAzureCredential();
            var client = new SecretClient(new Uri(keyVaultUrl), credential);
            var secret = client.GetSecret(secretName);
            return secret.Value.Value;
        }

    }
}
