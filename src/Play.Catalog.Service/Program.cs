using Play.Catalog.Service.Entities;
using Play.Common.MongoDB;
using Play.Common.Settings;

ServiceSettings serviceSettings;
var builder = WebApplication.CreateBuilder(args);

serviceSettings = builder.Configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>()!;

builder.Services.AddMongo().AddMongoRepository<Item>("items");

builder.Services.AddControllers(options =>
{
    options.SuppressAsyncSuffixInActionNames = false;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
