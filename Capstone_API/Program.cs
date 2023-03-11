
using Capstone_API.Models;
using Capstone_API.Service.Implement;
using Capstone_API.Service.Interface;
using Capstone_API.UOW_Repositories.UnitOfWork;

var builder = WebApplication.CreateBuilder(args);
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

//Add automapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddHttpClient();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<CapstoneDataContext>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IExcelService, ExcelService>();
builder.Services.AddScoped<ISubjectService, SubjectService>();
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<ILecturerService, LecturerService>();
builder.Services.AddScoped<IAreaSlotWeightService, AreaSlotWeightService>();
builder.Services.AddScoped<IDistanceService, DistanceService>();
builder.Services.AddScoped<IBuildingService, BuildingService>();
builder.Services.AddScoped<ISettingModelService, SettingModelService>();
builder.Services.AddScoped<ISlotPreferenceService, SlotPreferenceLevelService>();
builder.Services.AddScoped<ISubjectPreferenceLevelService, SubjectPreferenceLevelService>();
builder.Services.AddScoped<ITimeSlotCompatibilityService, TimeSlotCompatibilityService>();
builder.Services.AddScoped<ITimeSlotConflictService, TimeSlotConflictService>();
builder.Services.AddScoped<ITimeSlotService, TimeSlotService>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
//add hostingEnvironment service
var hostingEnvironment = builder.Services.BuildServiceProvider()?.GetService<IWebHostEnvironment>();
builder.Services.AddSingleton(hostingEnvironment);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:3000");
                      });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(MyAllowSpecificOrigins);
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

