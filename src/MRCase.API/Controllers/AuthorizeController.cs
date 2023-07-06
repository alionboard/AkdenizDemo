using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.IdentityModel.Tokens;
using MRCase.Application.Authorization.Dtos;
using MRCase.Core.Authorization;
using MRCase.Core.Localization;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MRCase.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizeController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration configuration;
        private readonly IStringLocalizer<Resource> localizer;


        public AuthorizeController(UserManager<ApplicationUser> userManager, IConfiguration configuration, IStringLocalizer<Resource> localizer)
        {
            this.userManager = userManager;
            this.configuration = configuration;
            this.localizer = localizer;
        }

        //Login User
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromForm] LoginDto loginDto)
        {
            var user = await userManager.FindByNameAsync(loginDto.Username);
            if (user != null && await userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                var userRoles = await userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub,user.Id),
                    new Claim(ClaimTypes.Name,user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]));
                var token = new JwtSecurityToken(
                    issuer: configuration["JWT:ValidIssuer"],
                    audience: configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddDays(7),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );

                return Ok(new 
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
            }
            throw new UnauthorizedAccessException(localizer["LoginError"].Value);
        }

        //Register User
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] RegisterDto registerDto)
        {
            var userExists = await userManager.FindByNameAsync(registerDto.Username);
            if (userExists != null)
            {
                throw new Exception(localizer["UsernameExists"].Value);
            }

            ApplicationUser user = new ApplicationUser()
            {
                UserName = registerDto.Username,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = await userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
            {
                throw new Exception(localizer["UserCreationFailed"].Value);

            }

            return Ok(localizer["Created"].Value);
        }

        //Verifying Tokens
        [HttpGet("verifyToken")]
        public async Task<IActionResult> VerifyToken([FromHeader]string authorization)
        {
            var token = authorization.Split(" ").Last();
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(configuration["JWT:Secret"]);
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;

                var username = jwtToken.Claims.First(x => x.Type.EndsWith("/name")).Value;
                return Ok(new { username = username, message = localizer["SuccessfulLogin"].Value });
            }
            catch
            {
                throw new ArgumentException(localizer["VerifyTokenFailed"].Value);
            }

        }
    }
}
