using API_Ripasso2;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<Acesso_Dati>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/api/prodtti", (Acesso_Dati dati) =>
{
    var list = dati.GetProducts();
    return list;
}).WithName("Visualizza Prodotti");

app.MapGet("/api/prodotti/{id}", (int id, Acesso_Dati dati) =>
{
    var prodotto = dati.GetProdotto(id);

    return prodotto;
}).WithName("Visualizza Prodotto");

app.MapPost("/api/prodotti", (Prodotti prodotto, Acesso_Dati dati) =>
{
    dati.InserisciProdotto(prodotto);
}).WithName("Inserisci Prodotto");

app.MapDelete("/api/prodotti/{id}", (int Id, Acesso_Dati dati) =>
{
    dati.CancellaProdotto(Id);
}).WithName("Cancella Prodotto");

app.Run();
