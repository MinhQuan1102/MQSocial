using System.Net;
using API.DTO;
using API.DTO.Auth;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Any;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(UserManager<User> userManager, ITokenService tokenService, IMapper mapper)
        : BaseApiController
    {
        [HttpPost("register")]
        public async Task<ActionResult<ApiResponse<RegisterResponseDto>>> Register([FromBody] RegisterRequestDto registerDto) {
            if (await EmailExists(registerDto.Email)) {
                return BadRequest(
                    new ApiResponse<string>((int)HttpStatusCode.BadRequest, "Email has already been taken!", null)
                );
            }
            var user = mapper.Map<User>(registerDto);
            user.FullName = registerDto.FirstName + " " + registerDto.LastName;
            user.UserName = (registerDto.FirstName + registerDto.LastName).Replace(" ", "");
            var result = await userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded) return BadRequest(
                new ApiResponse<string>((int)HttpStatusCode.BadRequest, "Fail to register user", null)
            );

            var roleResult = await userManager.AddToRoleAsync(user, "User");
            if (!roleResult.Succeeded) return BadRequest(roleResult.Errors);

            var userDto = new RegisterResponseDto {
                FullName = user.FullName,
                Email = user.Email ?? "",
                Gender = user.Gender == true ? "Male" : "Female",
                DateOfBirth = user.DateOfBirth,
                PhoneNumber = user.PhoneNumber ?? ""
            };
            if (userDto == null) throw new ArgumentNullException(nameof(userDto));

            return Ok(
                new ApiResponse<RegisterResponseDto>(
                    (int) HttpStatusCode.Created,
                    "User registered successfully",
                    userDto
                )
            );
        }

        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<LoginResponseDto>>> Login(LoginRequestDto loginDto) {
            var user = await userManager.Users.SingleOrDefaultAsync(x => x.Email == loginDto.Email);
            if (user == null) {
                return BadRequest(new ApiResponse<string>(
                    (int) HttpStatusCode.BadRequest,
                    "Invalid email",
                    null
                ));
            }

            var result = await userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!result) {
                return BadRequest(new ApiResponse<string>(
                    (int) HttpStatusCode.BadRequest,
                    "Incorrect password",
                    null
                ));
            }

            var userDto = new LoginResponseDto 
            {
                UserName = user.FullName,
                Email = user.Email ?? "",
                DateOfBirth = user.DateOfBirth,
                Token = await tokenService.CreateToken(user),
            };

            return Ok(
                new ApiResponse<LoginResponseDto>(
                    (int) HttpStatusCode.OK,
                    "Login successfully",
                    userDto
                )
            );
        }

        private async Task<bool> EmailExists(string email)
        {
            return await userManager.Users.AnyAsync(x => x.Email == email.ToLower());
        }
    }
}
