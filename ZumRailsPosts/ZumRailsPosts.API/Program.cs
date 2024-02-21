using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using ZumRailsPosts.API.Errors;
using ZumRailsPosts.API.Middleware;
using ZumRailsPosts.Common;
using ZumRailsPosts.Core.Infrastructure;
using ZumRailsPosts.Core.Logic;

namespace ZumRailsPosts.API
{
    public class Program
    {
        private static class CorsPolicy
        {
            public static readonly string AllowAll = "CorsPolicy.AllowAll";
        }

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Cors : allow all 
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(CorsPolicy.AllowAll,
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                               .AllowAnyMethod()
                               .AllowAnyHeader();
                    });
            });

            builder.Services.AddHttpClient();

            // Add services to the container.
            builder.Services.AddSingleton<MemoryCacheService>(); // simple cache, worker process life span 
            builder.Services.AddScoped<IPostsLogic, PostLogic>();
            builder.Services.AddScoped<IPostsRepository>(serviceProvider =>
            {
                var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
                var httpClient = httpClientFactory.CreateClient();
                var apiBaseUrl = builder.Configuration["Posts:BaseApiUrl"]; // Can use Options, Can customize configuration file
                var memoryCacheService = serviceProvider.GetRequiredService<MemoryCacheService>(); // Inject MemoryCacheService
                return new PostsRepository(httpClient, apiBaseUrl, memoryCacheService);
            });

            // Services
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Customizing the response format for validation errors.
            builder.Services.Configure<ApiBehaviorOptions>(options => {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var errors = actionContext.ModelState
                        .Where(x => x.Value.Errors.Count > 0)
                        .SelectMany(x => x.Value.Errors)
                        .Select(x => x.ErrorMessage)
                        .ToArray();

                    var errorResponse = new ApiValidationErrorResponse
                    {
                        Errors = errors
                    };

                    return new BadRequestObjectResult(errorResponse);
                };
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseExceptionMiddleware();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors(CorsPolicy.AllowAll);

            app.UseHttpsRedirection();
            
            app.UseAuthorization();
            
            app.MapControllers();

            app.Run();
        }
    }
}
