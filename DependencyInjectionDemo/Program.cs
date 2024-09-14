using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using DependencyInjectionDemo.Logic;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// it is a good thing to have in one place then you can find where everything in one place if needed to change
// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
// this adds DemoLogic to services list, which means we can ask for it whathewer we need a dependency
// when you add a dependency to a dependency injection, there is 3 different options in microsoft built-in
// AddTransient means that everytime you ask for the item you get a new instance, that is default and standard and most common way of doing
// usually you want new instance every single time
builder.Services.AddTransient<DemoLogic>();
// Singleton gets instance of the class same every single time the same and for entire application it is not changed
// create one instance and everyone on web uses for example the same instance all the time
// do not overuse this, because if user has other password or other they cannot use it because they get your password in appsettings
// Singleton is the memory for the life of web server, but transient goes once and then goes way with garbage collector
// with singletons memory is constantly in use because they are not deleted and can clogg the memory up
builder.Services.AddSingleton<DemoLogic>();
// it is a singleton per person, now this apps instance has it is owns singleton per person
builder.Services.AddScoped<DemoLogic>();
// dependency injection via interface
// always use interface with dependency injection 
// very easy to remove dependency when doing unit testing
// with this one change BetterDemoLogic we change entire application if it is used it a lot of places
// this reduce coupling because you can use IDemoLogic in other classes and then just change here for example insted BetterDemoLogic to DemoLogic and
// everything works the same with different logic thats nice that is so simple so use interfaces
builder.Services.AddTransient<IDemoLogic, BetterDemoLogic>();

builder.Host.UseSerilog((context, config) =>
{
    config.WriteTo.Console();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();

// so for dependencies you centralize them and you made sure you done then once 
// also developers sometimes check that class is not changing often and thinks that can do it static
// using static it is going to be a bad thing in most cases, it is very small use case
// it is bad with this call because you cannot implement interface, you cannot instanciate,
// you cannot inherit from other classes and instanciate from other classes and then use them
// so it limits to just that one class, which is kinda limited, but with singleton you can instanciate and use interface and other things, because it is full class
// so Singleton is used more than static
// 