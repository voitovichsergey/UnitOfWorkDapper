using BookStore.Infrastructure.Business;
using BookStore.Infrastructure.Data;
using BookStore.Services.Interfaces;
using BookStoreApp.Helpers;
using Dapper.UnitOfWork;
using Dapper.UnitOfWork.Interfaces;

const string ConnectionString = "Data Source=bookstore.db";
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddSingleton<IUnitOfWork>(new UnitOfWork<BookStoreConnection>(ConnectionString));
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<ISaleOrderService, SaleOrderService>();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
