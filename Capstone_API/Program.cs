
using Capstone_API.Models;
using Capstone_API.Service.Implement;
using Capstone_API.Service.Interface;
using Capstone_API.UOW_Repositories.UnitOfWork;

var builder = WebApplication.CreateBuilder(args);

//Add automapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<CapstoneDataContext>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ISubjectService, SubjectService>();
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<ILecturerService, LecturerService>();
builder.Services.AddScoped<IExcelService, ExcelService>();


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

