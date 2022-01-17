var builder = WebApplication.CreateBuilder(args);
var allowSpecificOrigins = "_myAllowSpecificOrigins";

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.WebHost.UseKestrel();
builder.WebHost.UseUrls("http://*:8080");
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: allowSpecificOrigins,
                      builder =>
                      {
                          builder.WithOrigins("http://192.168.1.20") // builder.WithOrigins("http://192.168.1.20")
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowAnyOrigin();
                          
                      });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();

app.UseCors(allowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

app.Run();
