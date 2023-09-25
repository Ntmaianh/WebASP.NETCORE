using Data.Entities;
using FluentValidation; // Install-Package FluentValidation
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.ViewModel.System.User
{
    // đây là class chứa những cái cta muốn validation 

    // ra program khai báo để sd 
    //  builder.Services.AddScoped<IValidator<LoginRequest>, LoginRequestValidator>();
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            // Rule for : quy tắc áp dụng cho cái gì 
            // ở dòng 17 là áp dụng cho UserName 
            // NotEmpty là không trống => nếu trống văng ra Extention 
            RuleFor(x => x.UserName).NotEmpty().WithMessage("UserName is requỉed");
            RuleFor(x => x.Password).NotEmpty().WithMessage("UserName is requỉed").MinimumLength(6);
        }
    }
}
