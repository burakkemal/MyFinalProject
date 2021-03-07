using Core.Entities.Concrete;
using Core.Utilities.Security.Encryption;
using Microsoft.Extensions.Configuration; 
using System.IdentityModel.Tokens.Jwt;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Core.Extensions;
using System.Linq;

namespace Core.Utilities.Security.JWT
{
    public class JwtHelper : ITokenHelper
    {
        public IConfiguration Configuration { get; } //apideki değerleri okumaya yarar.
        private TokenOptions _tokenOptions; // okunan değerleri bir nesneye atıyoruz. 
        private DateTime _accessTokenExpiration; //ne zaman geçersiz hale gelecek.
        public JwtHelper(IConfiguration configuration) 
        {
            Configuration = configuration;
            _tokenOptions = Configuration.GetSection("TokenOptions").Get<TokenOptions>(); //appsettings"deki "Toıkenoptions" olanı al. Sınıfın değerlerini kullanarak maple. ordaki ile atama yapar.

        }
        public AccessToken CreateToken(User user, List<OperationClaim> operationClaims) //bu kullanıcı için token üretilir. user bilgisi ver claim ver. ona göre token oluşturma.
        {
            _accessTokenExpiration = DateTime.Now.AddMinutes(_tokenOptions.AccessTokenExpiration); 
            var securityKey = SecurityKeyHelper.CreateSecurityKey(_tokenOptions.SecurityKey); //tokenoptions da ki anahtarı kullanarak yap.
            var signingCredentials = SigningCredentialsHelper.CreateSigningCredentials(securityKey); // kullanılacak algoritma ve anahtarı tutar.
            var jwt = CreateJwtSecurityToken(_tokenOptions, user, signingCredentials, operationClaims); //tokenoptionsları kullanarak. ilgili kullanıcı için ilgili credentialsları kullanarak verilecek claimleri yapan metot. 
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var token = jwtSecurityTokenHandler.WriteToken(jwt);

            return new AccessToken
            {
                Token = token,
                Expiration = _accessTokenExpiration
            };
        }

        public JwtSecurityToken CreateJwtSecurityToken(TokenOptions tokenOptions, User user,
            SigningCredentials signingCredentials, List<OperationClaim> operationClaims)
        {
            var jwt = new JwtSecurityToken( //token oluşturma. tüm bilgileri oluşturur. 
                issuer: tokenOptions.Issuer,
                audience: tokenOptions.Audience,
                expires: _accessTokenExpiration,
                notBefore: DateTime.Now, //şuandan önceyse geçerli olmaz
                claims: SetClaims(user, operationClaims), //claimler için ayrı bir metot. önemli.
                signingCredentials: signingCredentials
            );
            return jwt;
        }

        private IEnumerable<Claim> SetClaims(User user, List<OperationClaim> operationClaims)
        {
            var claims = new List<Claim>();
            claims.AddNameIdentifier(user.Id.ToString());
            claims.AddEmail(user.Email);
            claims.AddName($"{user.FirstName} {user.LastName}"); //2 tane stringi yanyana kullanmak için kullanılır.
            claims.AddRoles(operationClaims.Select(c => c.Name).ToArray());  

            return claims;
        }
    }
}
