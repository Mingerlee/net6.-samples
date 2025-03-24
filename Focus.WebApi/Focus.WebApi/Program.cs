using Focus.WebApi.Configurations;
using Autofac;
using Infrastructure.AutofacManager;
using Infrastructure.Utilities;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authorization;
using Focus.WebApi.Filters;
using NLog.Extensions.Logging;
using Focus.WebApi.Extensions;
using Focus.WebApi;

var builder = WebApplication.CreateBuilder(args);
//var Configuration = new ConfigurationBuilder().Add(new MemoryConfigurationSource()).Build();
// public ILifetimeScope AutofacContainer { get; private set; }

// Add services to the container.
builder.Services.AddHttpContextAccessor();
//builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerSetup();//注册Swagger配置
builder.Services.AddJwtAuthSetup(builder.Configuration);//注册JWT配置
builder.Services.AddAutoMapperSetup();//注册AutoMapper
//builder.Services.AddMiniProfiler();
//注册session
builder.Services.AddDistributedMemoryCache();//启用内存缓存
builder.Services.AddSession(options =>
{
options.IdleTimeout = TimeSpan.FromMinutes(30);//设置session过期时间
options.Cookie.IsEssential = true;
});
//注册Autofac
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())//Step 1：容器替换
    .ConfigureContainer<ContainerBuilder>(builder =>
{
    builder.RegisterModule();
    builder.RegisterDynamicProxys();//注册autofac代理
});
//注册应用服务
builder.Services.AddAppServices();
//注册控制器
//全局注册控制器Filter
builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(SQLProfilerFilter));
    options.Filters.Add(typeof(CustomerExceptionFilter));
    options.Filters.Add(typeof(CustomerActionFilter));
    options.Filters.Add(typeof(CustomerResultFilter));
    options.Filters.Add(typeof(CreateProcessActionFilter));
    options.Filters.Add(typeof(MyActionFilterAttribute));
});
//添加日志
builder.Services.AddLogging(m => { m.AddNLog("config/NLog.config"); });

//添加跨域
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", cors =>
    {
        cors.AllowAnyOrigin();
        cors.AllowAnyHeader();
        //cors.AllowAnyMethod();
    });
});

//添加本地默认缓存
builder.Services.AddMemoryCache();
var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerSetup();
}
//ServiceLocator.Instance = app.Services;
app.UseStaticHttpContext();

app.UseStaticFiles();//静态资源
//app.UseCors();// 跨域配置
app.UseCors("AllowSpecificOrigin");
app.UseSession();//使用session
app.UseHttpsRedirection();

app.UseAuthentication();//JWT 授权验证

app.UseAuthorization();

app.MapControllers();
//app.UseMiniProfiler();
//app.MapPost("/todo", [AllowAnonymous] (string Name) => $"Hello {Name}");
//app.MapGet("/Get/m", () => "Hello World");
app.Run();
