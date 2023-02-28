using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace EpilepsieDB.Services
{
    public interface ITokenService
    {
        public AuthenticationResponse CreateToken(IdentityUser user, List<string> userRoles);
    }

    public class AuthenticationResponse
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
