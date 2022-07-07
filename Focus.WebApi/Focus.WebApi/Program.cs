using Focus.WebApi.Configurations;
using Autofac;
using Infrastructure.AutofacManager;
using Infrastructure.Utilities;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authorization;
using Focus.WebApi.Filters;
using NLog.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);
//var Configuration = new ConfigurationBuilder().Add(new MemoryConfigurationSource()).Build();
// public ILifetimeScope AutofacContainer { get; private set; }

// Add services to the container.
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerSetup();
builder.Services.AddJwtAuthSetup(builder.Configuration);
//builder.Services.AddMiniProfiler();
//ע��session
builder.Services.AddDistributedMemoryCache();//�����ڴ滺��
builder.Services.AddSession(options =>
{
options.IdleTimeout = TimeSpan.FromMinutes(30);//����session����ʱ��
options.Cookie.IsEssential = true;
});
//ע��Autofac
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())//Step 1�������滻
    .ConfigureContainer<ContainerBuilder>(builder =>
{
    builder.RegisterModule();
});

//ȫ��ע�������Filter
builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(SQLProfilerFilter));
    options.Filters.Add(typeof(CustomerExceptionFilter));
    options.Filters.Add(typeof(CustomerActionFilter));
    options.Filters.Add(typeof(CustomerResultFilter));
});
//�����־
builder.Services.AddLogging(m => { m.AddNLog("config/NLog.config"); });

//��ӿ���
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", cors =>
    {
        cors.AllowAnyOrigin();
        cors.AllowAnyHeader();
        //cors.AllowAnyMethod();
    });
});

//��ӱ���Ĭ�ϻ���
builder.Services.AddMemoryCache();
var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerSetup();
}

app.UseStaticHttpContext();

app.UseStaticFiles();//��̬��Դ
//app.UseCors();// ��������
app.UseCors("AllowSpecificOrigin");
app.UseSession();//ʹ��session
app.UseHttpsRedirection();

app.UseAuthentication();//JWT ��Ȩ��֤

app.UseAuthorization();

app.MapControllers();
//app.UseMiniProfiler();
//app.MapPost("/todo", [AllowAnonymous] (string Name) => $"Hello {Name}");
//app.MapGet("/Get/m", () => "Hello World");
app.Run();
