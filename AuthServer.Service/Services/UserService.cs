using AuthServer.Core.Dtos;
using AuthServer.Core.Models;
using AuthServer.Core.Services;
using Microsoft.AspNetCore.Identity;
using SharedLibrary.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Service.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<UserApp> _userManager;

        public UserService(UserManager<UserApp> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ResponseDto<UserAppDto>> CreateUserAsync(CreateUserDto userDto)
        {
            var user = new UserApp { Email = userDto.Email, UserName = userDto.Username };
            var result = await _userManager.CreateAsync(user, userDto.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(x => x.Description).ToList();
                return ResponseDto<UserAppDto>.Fail(400, new ErrorDto(errors, true));
            }

            return ResponseDto<UserAppDto>.Success(200, ObjectMapper.Mapper.Map<UserAppDto>(user));
        }

        public async Task<ResponseDto<UserAppDto>> GetUserByName(string username)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user == null)
            {
                return ResponseDto<UserAppDto>.Fail(404, "Username not found!", true);
            }

            return ResponseDto<UserAppDto>.Success(200, ObjectMapper.Mapper.Map<UserAppDto>(user));
        }
    }
}
