using Microsoft.AspNetCore.Authorization.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//builder.Services.AddMvc()

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddAuthentication(opt =>
//{
//    opt.AddScheme<>();
//});

builder.Services.AddAuthorization(options =>
{
    //options.DefaultPolicy
    options.AddPolicy("Permission", policy => policy.RequireRole());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.Use((context)=>next)

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();