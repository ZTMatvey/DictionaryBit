using DictionaryBit.TelegramInteraction;
using DictionaryBit.Data.Context;
using DictionaryBit.Data.Entities;
using DictionaryBit.Data.Repositories;
using DictionaryBit.Data.Repositories.Abstract;
using DictionaryBit.Data.Repositories.EFCore;
using DictionaryBit.Service;
using Microsoft.EntityFrameworkCore;
using DictionaryBit.Data.Interaction;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
services.AddHostedService<NotificationManager>();
var sqlConnectionString = builder.Configuration.GetConnectionString("DefaultDBConnection");
var config = new Config();
builder.Configuration.Bind("Config", config);
services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(sqlConnectionString));
services.AddControllers();
services.AddSingleton<ITelegramBot, PollingTelegramBot>();
services.AddScoped<RepositoryManager>();
services.AddScoped<NotificationManager>();
services.AddSingleton(config);

services.AddScoped<UserRepository, UserEFCoreRepository>();
services.AddScoped<DictionaryRepository, DictionaryEFCoreRepository>();
services.AddScoped<WordRepository, WordEFCoreRepository>();
services.AddScoped<WordInteraction>();

services.AddHttpContextAccessor();
services.AddDistributedMemoryCache();
services.AddSession(options =>
{
    options.Cookie.Name = ".DictionaryBit.Session";
    options.IdleTimeout = TimeSpan.FromMinutes(15);
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();

app.UseRouting();
app.UseSession();
var botClient = app.Services.GetRequiredService<ITelegramBot>();

botClient.GetBot().Wait();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Lifetime.ApplicationStopping.Register(() => botClient.Dispose());

app.Run();