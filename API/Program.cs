using Common.Auth;
using DAL.Repository;
using DAL.Repository.Models;
using DAL.Services.Interfaces;
using DAL.Services.Concrete.EF;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using BL.Services.Interfaces;
using BL.Services.Concrete;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Register DAL services
builder.Services.AddScoped<IAuthDAL, AuthDAL>();
builder.Services.AddScoped<ICartItemDAL, CartItemDAL>();
builder.Services.AddScoped<IOnlinePaymentDAL, OnlinePaymentDAL>();
builder.Services.AddScoped<IOrderDAL, OrderDAL>();
builder.Services.AddScoped<IPaymentMethodDAL, PaymendMethodDAL>();
builder.Services.AddScoped<IProductDAL, ProductDAL>();
builder.Services.AddScoped<IShoppingCartDAL, ShoppingCartDAL>();
builder.Services.AddScoped<IProductDetailsDAL<Brand>, BrandDAL>();
builder.Services.AddScoped<IProductDetailsDAL<Category>, CategoryDAL>();
builder.Services.AddScoped<IProductDetailsDAL<SpecialTag>, SpecialTagDAL>();

// Register BL services
builder.Services.AddScoped<IAuthBL, AuthBL>();
builder.Services.AddScoped<IBrandBL, BrandBL>();
builder.Services.AddScoped<ICategoryBL, CategoryBL>();
builder.Services.AddScoped<IOnlinePaymentBL, OnlinePaymentStripeBL>();
builder.Services.AddScoped<IOrderBL, OrderBL>();
builder.Services.AddScoped<IPaymentMethodBL, PaymentMethodBL>();
builder.Services.AddScoped<IProductBL, ProductBL>();
builder.Services.AddScoped<IShoppingCartBL, ShoppingCartBL>();
builder.Services.AddScoped<ISpecialTagBL, SpecialTagBL>();


builder.Services.AddDbContext<ApplicationDbContext>();
builder.Services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.Configure<IdentityOptions>(options => {
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
});

var key = AuthOptions.KEY;
builder.Services.AddAuthentication(u => {
    u.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    u.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(u => {
    u.RequireHttpsMetadata = false;
    u.SaveToken = true;
    u.TokenValidationParameters = new TokenValidationParameters {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
        ValidateIssuer = false,
        ValidateAudience = false,
    };
});

builder.Services.AddCors();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(o => {
    o.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme {
        Description = "JWT Authorization Header",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        Type = SecuritySchemeType.Http
    });
    o.AddSecurityRequirement(new OpenApiSecurityRequirement() {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().WithExposedHeaders("*"));

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
