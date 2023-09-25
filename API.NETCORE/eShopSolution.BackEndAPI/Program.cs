
using Application.Catalog.Product;
using Application.Catalog.Product.DTO;
using Application.Catalog.Product.Public;
using Application.Common;
using Application.System;
using Data.EF;
using Data.Entities;
using eShopSolution.ViewModel.System.User;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace eShopSolution.BackEndAPI
{

    // tất những gì ở ngoài project web muốn sử dụng thì đều phải => khai báo vào đây thì mới sử dụng được 
    public class Program
    {
     
        public static void Main(string[] args)
        {
     
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            // khai báo để sd Fluent 
            // RegisterValidatorsFromAssemblyContaining : đăng kí tất cả những thằng nào cùng Assembly cùng project với LoginRequestValidator 
            builder.Services.AddControllers().AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<LoginRequestValidator>());
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
 
            // sử dụng DI (interface)
            // + Scoped  : Tạo một thể hiện mới cho tất cả các scope (Mỗi request là một scope).
            // Trong scope thì service được dùng lại
            // + Transient : mỗi lần chạy nó sẽ lại khởi tạo lại cái mới 
            // + Singerton : chỉ tạo 1 lần duy nhất và dùng đi dùng lại
            // 
            // => trước khi chạy hãy kiểm tra tất cả xem có DI ở trong DI cần chạy không => nếu có thì thêm
            // ví dụ IStorageService có trong ManagerProduct => phải thêm vào
            builder.Services.AddTransient<IPublicProduct, PublicProduct>();
            builder.Services.AddScoped<IManagerProduct, ManagerProduct>();
            builder.Services.AddScoped<IStorageService, StorageService>();
            builder.Services.AddScoped<UserManager<AppUser>, UserManager<AppUser>>();
            builder.Services.AddScoped<SignInManager<AppUser>, SignInManager<AppUser>>();
            builder.Services.AddScoped<RoleManager<AppRole>, RoleManager<AppRole>>();
            builder.Services.AddScoped<IUser, User>();
            builder.Services.AddIdentity<AppUser, AppRole>().AddEntityFrameworkStores<ESHOPDBContext>().AddDefaultTokenProviders();
            // không cần nữa vì mình đã RegisterValidatorsFromAssemblyContaining cho cả project đó rồi 

            //builder.Services.AddScoped<IValidator<LoginRequest>, LoginRequestValidator>();
            //builder.Services.AddScoped<IValidator<RegisterRequest>, RegisterValidatorRequest>();


            builder.Services.AddSwaggerGen(c =>
            {

                // định nghĩa AddSecurityDefinition => chỉ để setup JWWT 
                // mỗi khi mà cta gọi nó sẽ truyền vào 1 cái header tên Bearer , name là Authorization 
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header, // truyền cái tên Bearer vào Header
                    Type = SecuritySchemeType.ApiKey, // kiểu là ApiKey 
                    Scheme = "Bearer"
                });

                // khi gọi Swagger thì  yêu cầu truyền vào Swagger header vào đây 
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                  {
                    {
                      new OpenApiSecurityScheme
                      {
                        Reference = new OpenApiReference
                          {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                          },
                          Scheme = "oauth2",
                          Name = "Bearer",
                          In = ParameterLocation.Header,
                        },
                        new List<string>()
                      }
                });
            });

            string issuer =builder.Configuration.GetValue<string>("Tokens:Issuer");
            string signingKey =builder.Configuration.GetValue<string>("Tokens:Key");
            byte[] signingKeyBytes = System.Text.Encoding.UTF8.GetBytes(signingKey);

          builder.Services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
              .AddJwtBearer(options =>
              {
                  options.RequireHttpsMetadata = false;
                  options.SaveToken = true;
                  options.TokenValidationParameters = new TokenValidationParameters()
                  {
                      ValidateIssuer = true,
                      ValidIssuer = issuer,
                      ValidateAudience = true,
                      ValidAudience = issuer,
                      ValidateLifetime = true,
                      ValidateIssuerSigningKey = true,
                      ClockSkew = System.TimeSpan.Zero,
                      IssuerSigningKey = new SymmetricSecurityKey(signingKeyBytes)
                  };
              });


            // kết nối db 
            builder.Services.AddDbContext<ESHOPDBContext>(option =>
                {
                    option.UseSqlServer(builder.Configuration.GetConnectionString("Database"));
                });
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseAuthentication();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}