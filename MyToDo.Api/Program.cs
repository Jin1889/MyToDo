using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MyToDo.Api;
using MyToDo.Api.Context;
using MyToDo.Api.Context.Repository;
using MyToDo.Api.Extensions;
using MyToDo.Api.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//自己写的controller要在swagger中测试，需要添加下面这行代码，AddSwaggerGen也需要改造
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "MyToDo.Api", Version = "v1" });
});

//由于.NET6没有startup.cs
//这里是新的写法,配置数据库等相关信息
builder.Services.AddDbContext<MyToDoContext>(option =>
{
    var connectionString = builder.Configuration.GetConnectionString("ToDoConnection");
    option.UseSqlite(connectionString);
}).AddUnitOfWork<MyToDoContext>()
.AddCustomRepository<ToDo, ToDoRepository>()
.AddCustomRepository<Memo, MemoRepository>()
.AddCustomRepository<User, UserRepository>();

//注册服务
builder.Services.AddTransient<IToDoService, ToDoService>();
builder.Services.AddTransient<IMemoService, MemoService>();
builder.Services.AddTransient<ILoginService, LoginService>();

//添加AutoMapper
var autoMapperConfig = new MapperConfiguration(config =>
{
    config.AddProfile(new AutoMapperProFile());
});
builder.Services.AddSingleton(autoMapperConfig.CreateMapper());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MyToDo.Api v1"));
    //加入以下三个方法才能正常使用swagger对数据库进行操作
    app.UseRouting();
    app.UseAuthorization();
    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
        endpoints.MapSwagger();
    });
}

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
       new WeatherForecast
       (
           DateTime.Now.AddDays(index),
           Random.Shared.Next(-20, 55),
           summaries[Random.Shared.Next(summaries.Length)]
       ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.Run();

internal record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}