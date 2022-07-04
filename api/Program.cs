using api.Logic;
using data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddTransient<PipelineLogic>();
builder.Services.AddTransient<UtilLogic>();
builder.Services.AddTransient<ApplicationLogic>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DeployerContext>();

builder.Services.AddCors(p => p.AddPolicy("main", builder =>
{
    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
}));

var app = builder.Build();

app.UseCors("main");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
