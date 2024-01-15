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
// using Inventory.Validation.UserValidations;
using Inventory.Validations.CategoryValidations;
using Inventory.Validations.DocumentTypeValidations;
using Inventory.Validations.ItemTemplateValidations;
using Inventory.Validations.ItemValidations;
using Inventory.Validations.ListValidations;
using Inventory.Validations.LocationValidations;
using Inventory.Validations.PreCheckValidations;
using Inventory.Validations.SizeValidations;
// using Inventory.Validations.UserRoleValidations;
// using Inventory.Validations.UserValidations;
using Inventory.Validations.VendorValidations;

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
            // services.AddScoped<IUserRoleService, UserRoleService>();
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
            
            services.AddScoped<IUserUtilities, UserUtilities>();
            
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
            // services.AddScoped<IUserCreateValidator, UserCreateValidator>();
            // services.AddScoped<IUserUpdateValidator, UserUpdateValidator>();
            // services.AddScoped<IUserRoleCreateValidator, UserRoleCreateValidator>();
            // services.AddScoped<IUserRoleUpdateValidator, UserRoleUpdateValidator>();
            services.AddScoped<IPreCheckCreateValidator, PreCheckCreateValidator>();
            services.AddScoped<IPreCheckUpdateValidator, PreCheckUpdateValidator>();
            services.AddScoped<ISizeCreateValidator, SizeCreateValidator>();
            services.AddScoped<ISizeUpdateValidator, SizeUpdateValidator>();
            services.AddScoped<IDocumentTypeCreateValidator, DocumentTypeCreateValidator>();
            services.AddScoped<IDocumentTypeUpdateValidator, DocumentTypeUpdateValidator>();
            
            services.AddHostedService<UserCreateHandler>();
            services.AddHostedService<UserUpdateHandler>();
            services.AddHostedService<UserDeleteHandler>();

            services.AddControllers();



            // Add DbContext
            // var connectionString = GetSecretValueFromKeyVault(Configuration["AzureKeyVault:ConnectionStringSecretName"]);
            
            var connectionString = "Server=tcp:dbserver-inventory-prod.database.windows.net,1433;Initial Catalog=db-inventory-prod_Copy;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;Authentication='Active Directory Default';";


            // services.AddDbContext<InventoryDbContext>(options =>
            //     options.UseInMemoryDatabase("temporaryDatabase"));

            services.AddDbContext<InventoryDbContext>(options => options.UseSqlServer(connectionString));



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

            // dbContext.Database.Migrate();

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
