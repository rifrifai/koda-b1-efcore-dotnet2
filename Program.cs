using API.Data;
using API.Files;
using API.Middlewares;
using API.Repository;
using API.Services;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddOpenApi();
builder.Services.AddProblemDetails(
    opt =>
    {
        opt.CustomizeProblemDetails = context =>
        {
            context.ProblemDetails.Instance ??= context.HttpContext.Request.Path.Value;
            context.ProblemDetails.Extensions["traceId"] = context.HttpContext.TraceIdentifier;
        };
    }
);
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
// builder.Services.AddSingleton<MemoryProductCatalog>();
// builder.Services.AddSingleton<IProductRepository, MemoryProductRepository>();
// builder.Services.AddSingleton<IProductService, ProductServices>();
builder.Services.AddScoped<IProductRepository, SqlProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IProductImageStorage, LocalProductImageStore>();

var connString = builder.Configuration.GetConnectionString("ecommercedb") ?? throw new InvalidOperationException("ConnectionString : Belum dikonfigurasi");
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(connString);
});
// builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ecommercedb")));


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
