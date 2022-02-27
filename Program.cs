using Microsoft.OpenApi.Models;
using PizzaStore.DB;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Yona API", Version = "v1", Description = "Keep track information for nothing..." });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
  app.UseDeveloperExceptionPage();
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Yona API");
});
// Methods
app.MapGet("/", () => "Hello World!");
// get all
app.MapGet("/pizzas", () => PizzaDB.GetPizzas());
// get by id
app.MapGet("/pizzas/{id}", (int id) => PizzaDB.GetPizza(id));
// create
app.MapPost("/pizzas", (Pizza pizza) => PizzaDB.CreatePizza(pizza));
// update
app.MapPut("/pizzas", (Pizza pizza) => PizzaDB.UpdatePizza(pizza));
// delete
app.MapDelete("/pizzas/{id}", (int id) => PizzaDB.DeletePizza(id));

app.Run();
app.UseCors("*"); 
