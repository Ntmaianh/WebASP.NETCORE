using eShopSolution.ViewModel.System.User;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


namespace Application.System
{
    public interface IUser
    {
        Task<string> Login(LoginRequest request);
        Task<bool> Register(RegisterRequest request);
    }
}
