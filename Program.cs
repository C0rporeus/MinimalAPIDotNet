using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
// using PizzaStore.DB;
using PizzaStore.Models;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("Pizzas") ?? "Data Source=pizza.db";
// data base in memory
// builder.Services.AddDbContext<PizzaDb>(options => options.UseInMemoryDatabase("items"));
builder.Services.AddSqlite<PizzaDb>(connectionString);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { 
      Title = "Yona API", 
      Version = "v1", 
      Description = "Keep track information for nothing..." });
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
app.MapGet("/pizzas", async (PizzaDb db) => await db.Pizzas.ToListAsync());
// get by id
app.MapGet("/pizza/{id}", async (PizzaDb db, int id) => await db.Pizzas.FindAsync(id));
// create
app.MapPost("/pizza", async (PizzaDb db, Pizza pizza) => {
  await db.Pizzas.AddAsync(pizza);
  await db.SaveChangesAsync();
  return Results.Created($"/pizza/{pizza.Id}", pizza);
});
// update
app.MapPut("/pizza/{id}", async (PizzaDb db, Pizza updatepizza, int id) => {
  var pizza = await db.Pizzas.FindAsync(id);
  if(pizza is null) return Results.NotFound();
  pizza.Name = updatepizza.Name;
  pizza.Description = updatepizza.Description;
  await db.SaveChangesAsync();
  return Results.NoContent();
});
// delete
app.MapDelete("/pizza/{id}", async (PizzaDb db, int id) => {
  var pizza = await db.Pizzas.FindAsync(id);
  if(pizza is null){
    return Results.NotFound();
  }
  db.Pizzas.Remove(pizza);
  await db.SaveChangesAsync();
  return Results.Ok();
});


app.Run();
app.UseCors("*"); 