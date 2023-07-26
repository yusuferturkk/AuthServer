using AuthServer.Core.Dtos;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Core.Services
{
    public interface IAuthenticationService
    {
        Task<ResponseDto<TokenDto>> CreateToken(LoginDto loginDto);

        Task<ResponseDto<TokenDto>> CreateTokenByRefreshToken(string refreshToken);

        Task<ResponseDto<NoContentDto>> RevokeRefreshToken(string refreshToken);

        Task<ResponseDto<ClientTokenDto>> CreateTokenByClient(ClientTokenDto clientTokenDto);
    }
}
