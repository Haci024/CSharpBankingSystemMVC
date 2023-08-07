using BussinessLayer.Abstract;
using BussinessLayer.Concrete;
using DataAccsessLayer.Abstract;
using DataAccsessLayer.Concrete;
using DataAccsessLayer.EFrameworkCore;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.CodeAnalysis.Host;
using Microsoft.Extensions.Options;
using MoneyTransferProject.Models;
using NETCore.MailKit.Core;
using NuGet.Configuration;
using NuGet.Protocol;
using System.Globalization;
using System.Reflection;
using IEmailService = BussinessLayer.Abstract.IEmailService;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>();
builder.Services.AddMvc();
builder.Services.AddSingleton<LanguageService>();
//services.AddTransient<ISmsService, TwilioSmsService>();
//services.Configure<SendSMS>(Configuration.GetSection("SendSMS"));
builder.Services.AddScoped<ICustomerActionProcessDAL,EFCustomerActionProcessRepository>();//DataAccessLayer
builder.Services.AddScoped<ICustomerActionProcessService, CustomerActionProcessManager>();//Bussinesslayer
builder.Services.AddScoped<ICustomerAccountDAL, EFCustomerAccountRepository>();//DataAccessLayer
builder.Services.AddScoped<ICustomerAccountService, CustomerAccountManager>();//Bussinesslayer
builder.Services.AddScoped<IEmailService, EmailManager>();//BussinessLayer
builder.Services.AddScoped<ISMSService,SMSManager>();//BussinessLayer
builder.Services.AddScoped<ICreditDAL, EFCreditRepository>();//DataAccessLayer
builder.Services.AddScoped<ICreditService, CreditManager>();//Bussinesslayer
builder.Services.AddScoped<ICreditDetailService,CreditDetailManager >();//BussinesLayer
builder.Services.AddScoped<ICreditDetailDAL, EFCreditDetailRepository>();//DataAccessLayer
builder.Services.AddScoped<ICreditDetailDAL, EFCreditDetailRepository>();//Bussinesslayer
builder.Services.AddScoped<IMonthlyPaymentService, MonthlyPaymentManager>();//Bussinesslayer
builder.Services.AddScoped<IMonthlyPaymentDAL, EFMonthlyPaymentRepository>();//DataAccessLayer
builder.Services.AddScoped<IKassaService, KassaManager>();//Bussinesslayer
builder.Services.AddScoped<IKassaDAL, EFKassaRepository>();//DataAccessLayer

builder.Services.AddScoped<IMoneyConvertService, ConvertMoneyManager>();

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.AddMvc().
    AddViewLocalization().
    AddDataAnnotationsLocalization(options =>
    {
        options.DataAnnotationLocalizerProvider = (type, factory) =>
        {
            var assemblyName = new AssemblyName(typeof(ShareResource).GetTypeInfo().Assembly.FullName);
            return factory.Create(nameof(ShareResource), assemblyName.Name);
        };

    });

builder.Services.Configure<RequestLocalizationOptions>(
    options =>
    {
        var supportedCultures = new List<CultureInfo>
        {
                        new CultureInfo("az-AZ"),
                         new CultureInfo("en-US"),
                          new CultureInfo("ru-RU"),
    };
        options.DefaultRequestCulture = new RequestCulture(culture: "az-AZ", uiCulture: "az-AZ");
        options.SupportedCultures = supportedCultures;
        options.SupportedUICultures = supportedCultures;

        options.RequestCultureProviders.Insert(0, new QueryStringRequestCultureProvider());
    });
builder.Services.AddIdentity<AppUser, AppRole>(options =>
{

    options.User.RequireUniqueEmail = true;
    options.Password.RequireNonAlphanumeric = false;
    
	options.Lockout.MaxFailedAccessAttempts = 5;// sifreni daxil eden zaman ne qeder sehv cehd etmek olar!
	options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);//Sifreni sehv daxiletme limitini kecdikden sonra bloklanma muddeti!


}).AddEntityFrameworkStores<AppDbContext>().AddErrorDescriber<CustomIdentityValidator>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

var locOptions = app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>();
app.UseRequestLocalization(locOptions.Value);
app.UseHttpLogging();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();



app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "areas",
        pattern: "{area:exists}/{controller=MyAccount}/{action=AllCards}/{id?}");

    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    endpoints.MapRazorPages();
});

app.Run();
