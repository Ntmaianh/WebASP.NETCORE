using Data.Entities;
using eShopSolution.ViewModel.System.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.System
{
    public class User : IUser
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IConfiguration _config;

        // UserManager , SignInManager là thư viện của thằng identiti => để sd được thì phải khai báo ở Program 
        public User(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<AppRole> roleManager, IConfiguration config)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _config = config;
        }

        // Login 
        public async Task<string> Login(LoginRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null)
            {
                return null;
            }
            var result = await _signInManager.PasswordSignInAsync(user, request.Password, request.RememberMe, true);
            if (!result.Succeeded)
            {
                return null;
            }
            var roles = await _userManager.GetRolesAsync(user);
            var claim = new[]
            {
                // muốn đẩy thông tin gì vê tooken thì cho vào Claim 
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.GivenName,user.LastName),
                new Claim(ClaimTypes.Role,string.Join(";",roles))
            };

            // Mã hóa Claim bằng SymmetricSecurityKey 
            // chuyền vào 1 cái Key =. từ key đấy ren ra Tooken 

            // ==> Không có hiểu :((
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Tokens:Issuer"],
                _config["Tokens:Issuer"],
                claim,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // đăng kí 
        public async Task<bool> Register(RegisterRequest request)
        {
            var newUser = new AppUser
            {
                Dob = request.Dob,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.UserName,
                PhoneNumber = request.PhoneNumber
            };
            var createUser = await _userManager.CreateAsync(newUser, request.Password);

            // những quy định về mật khẩu : 
            // + Mật khẩu phải có kí tự không phải là chữ và số
            // + mật khẩu phải có ít nhất 1 kí tự viết hoa chữ viết hoa 
            if (createUser.Succeeded)
            {
                return true;
            }
            return false;
        }
    }
}
