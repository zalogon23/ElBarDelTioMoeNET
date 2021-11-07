using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Microsoft.IdentityModel.Tokens;
using backend.Graphs.Queries;
using backend.Graphs.Mutations;

namespace backend
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
      services.AddCors(o => o.AddPolicy("AllOrigin", builder =>
      {
        builder.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
      }));

      var key = Encoding.ASCII.GetBytes(Configuration.GetValue<string>("JWTConfiguration:SecretKey"));

      services.AddAuthentication(x =>
      {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
      }).AddJwtBearer(x =>
      {
        x.RequireHttpsMetadata = false;
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters
        {
          ValidateIssuerSigningKey = true,
          IssuerSigningKey = new SymmetricSecurityKey(key),
          ValidateIssuer = false,
          ValidateAudience = false,
          ValidateLifetime = true,
          ClockSkew = new TimeSpan(0)
        };
      });

      services.Configure<MongoDBConfiguration>(
        Configuration.GetSection(nameof(MongoDBConfiguration)));
      services.Configure<JWTConfiguration>(
              Configuration.GetSection(nameof(JWTConfiguration)));

      services.AddSingleton<IMongoDbConfiguration>(sp =>
        sp.GetRequiredService<IOptions<MongoDBConfiguration>>().Value);
      services.AddSingleton<IJWTConfiguration>(sp =>
        sp.GetRequiredService<IOptions<JWTConfiguration>>().Value);

      services.AddSingleton<BeveragesServices>();
      services.AddSingleton<KeywordsServices>();
      services.AddSingleton<ClassificationsServices>();
      services.AddSingleton<UsersServices>();
      services.AddSingleton<RefreshTokensServices>();
      services.AddSingleton<IngredientsServices>();
      services.AddSingleton<InstructionsServices>();
      services.AddSingleton<FavoritesServices>();

      services.AddScoped<Query>();
      services.AddScoped<Mutation>();

      services.AddControllers();
      services.AddSwaggerGen(c =>
      {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "backend", Version = "v1" });
      });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      app.UseCors("AllOrigin");
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "backend v1"));
      }

      app.UseHttpsRedirection();

      app.UseRouting();

      app.UseAuthentication();

      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });
    }
  }
}
