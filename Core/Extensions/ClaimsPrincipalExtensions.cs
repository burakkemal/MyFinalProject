using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Core.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static List<string> Claims(this ClaimsPrincipal claimsPrincipal, string claimType) //principal=bir kişinin claimlerine erişmek için kullanılır. 
        {
            var result = claimsPrincipal?.FindAll(claimType)?.Select(x => x.Value).ToList(); //soru işareti null olabilir.
            return result;
        }

        public static List<string> ClaimRoles(this ClaimsPrincipal claimsPrincipal) //direkt rolleri getirecek metot. 
        {
            return claimsPrincipal?.Claims(ClaimTypes.Role);
        }
    }
}
