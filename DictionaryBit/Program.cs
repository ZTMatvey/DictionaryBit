using DictionaryBit.TelegramInteraction;
using DictionaryBit.TelegramInteraction.Operations.Commands;
using DictionaryBit.TelegramInteraction.Operations.Commands.AddDictionary;
using DictionaryBit.TelegramInteraction.Operations.Commands.AddWord;
using DictionaryBit.Data.Context;
using DictionaryBit.Data.Entities;
using DictionaryBit.Data.Repositories;
using DictionaryBit.Data.Repositories.Abstract;
using DictionaryBit.Data.Repositories.EFCore;
using DictionaryBit.Service;
using Microsoft.EntityFrameworkCore;
using DictionaryBit.TelegramInteraction.Operations.Commands.UseDictionary;
using DictionaryBit.Data.Interaction;
using DictionaryBit.TelegramInteraction.Operations.Commands.GetData;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
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

services.AddTransient<CommandBase, AddDictionaryCommand>();
services.AddTransient<CommandBase, AddDictionaryNameCommand>();
services.AddTransient<CommandBase, AddWordCommand>();
services.AddTransient<CommandBase, AddWordDescriptionCommand>();
services.AddTransient<CommandBase, AddWordForeignCommand>();
services.AddTransient<CommandBase, AddWordNativeCommand>();
services.AddTransient<CommandBase, DefaultCommand>();
services.AddTransient<CommandBase, GetDictionariesCommand>();
services.AddTransient<CommandBase, GetWordsCommand>();
services.AddTransient<CommandBase, SaveWordCommand>();
services.AddTransient<CommandBase, SetUsedDictionaryCommand>();
services.AddTransient<CommandBase, RemoveUsedDictionaryCommand>();
services.AddTransient<CommandBase, SymbolsWarningCommand>();
services.AddTransient<CommandBase, StartCommand>();
services.AddTransient<CommandBase, TestCommand>();
services.AddTransient<CommandBase, UseDictionaryCommand>();

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