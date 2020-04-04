using Flo.Repositories;
using GoodToDo.API.ToDo;
using GoodToDo.API.Users;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace GoodToDo.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            // add mongoDB user repository
            services.AddMongoDBRepo<User, Guid, UserRepo>(Configuration["GOODTODO_MONGODB"], "GoodToDo", "Users") // TODO: more configuration settings
                .AddScoped<IAuthoriser<User, Guid>, AllowAllAuthoriser<User, Guid>>() 
                .AddScoped<IValidator<User, Guid>, DataAnnotationValidator<User, Guid>>();

            // add identity 
            services.AddTransient<IUserStore<User>, MongoUserStore>(); // custom MongoDB UserStore

            services.AddIdentityCore<User>() // AddIdentityCore instead of AddIdentity as we have a custom UserStore, but no RoleStore
                .AddSignInManager();  // needed as we are calling AddIdentityCore not AddIdentity

            services.AddAuthentication(o => // needed as we are calling AddIdentityCore not AddIdentity
            {
                o.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
                o.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
                o.DefaultSignInScheme = IdentityConstants.ExternalScheme;
            }).AddIdentityCookies();

            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = "GoodToDo";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                options.SlidingExpiration = true;
                options.Cookie.HttpOnly = false;
                options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Lax; // allow API to store the cookie, despite different url from client on same domain
                options.Cookie.SecurePolicy = Microsoft.AspNetCore.Http.CookieSecurePolicy.SameAsRequest;

                options.Events.OnRedirectToLogin = context => // don't redirect, just give 401
                {
                    context.Response.StatusCode = 401;
                    return Task.CompletedTask;
                };
            });

            // add ToDo repo
            services.AddMongoDBRepo<ToDoItem, Guid, ToDoRepo>(Configuration["GOODTODO_MONGODB"], "GoodToDo", "ToDos") // TODO: more configuration settings
                .AddScoped<IAuthoriser<ToDoItem, Guid>, AllowAllAuthoriser<ToDoItem, Guid>>() // TODO: update to custom authoriser to prevent one user accessing another's ToDos
                .AddScoped<IValidator<ToDoItem, Guid>, DataAnnotationValidator<ToDoItem, Guid>>();

            // add swagger
            services.AddSwaggerDocument();

            // add CORS
            services.AddCors(options =>
                options.AddPolicy("localhost", builder => builder.WithOrigins(Configuration["GOODTODO_CLIENTURL"]) // TODO: add better configuration
                .AllowAnyHeader().AllowAnyMethod().AllowCredentials())); 


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("localhost");

            app.UseAuthentication();

            app.UseAuthorization();

            // Swagger
            app.UseOpenApi();
            app.UseSwaggerUi3();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
