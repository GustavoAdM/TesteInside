using GerenciarPedidos.Data;
using GerenciarPedidos.Services.ItemPedido;
using GerenciarPedidos.Services.Pedido;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Comunicação entre Interface e o Service
builder.Services.AddScoped<PedidoInterface, PedidoService>();
builder.Services.AddScoped<ItemPedidoInterface, ItemPedidoService>();

//Carregando dados de conexão do appsettings.json

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
