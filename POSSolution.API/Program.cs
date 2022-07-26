using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using POSSolution.Application;
using POSSolution.Core.Models;
using POSSolution.Infrastructure;
using POSSolution.Infrastructure.ModelRepository;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options => options.AddPolicy("posPolicy", policy =>
policy.AllowAnyOrigin()
.AllowAnyMethod()
.AllowAnyHeader()));

builder.Services.AddControllers(options =>
{
    options.OutputFormatters.RemoveType<SystemTextJsonOutputFormatter>();
    options.OutputFormatters.Add(new SystemTextJsonOutputFormatter(new JsonSerializerOptions(JsonSerializerDefaults.Web)
    {
        ReferenceHandler = null,
    }));
});

builder.Services.AddScoped<IRepository<Country>, Repository<Country>>();
builder.Services.AddDbContext<POSContext>(options =>
                     options.UseSqlServer(builder.Configuration.GetConnectionString("PosSolutionConnection")));
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "POSSolution.API", Version = "v1" });
});


var app = builder.Build();


if(app.Environment.IsDevelopment())
{
  app.UseDeveloperExceptionPage();
  app.UseSwagger();
  app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "POSSolution.API v1"));
}
//app.UseCors(ob => ob.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod());
app.UseCors("posPolicy");
app.UseRouting();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
  endpoints.MapControllers();
});
app.Run();
