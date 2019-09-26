using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Infrastructure.Jwt
{
    public class TokenManagement
    {
        public string SecurityKey { get; set; }

        public int AccessExpiration { get; set; }

        public int RefreshExpiration { get; set; }

        public string Issuer { get; set; }

        public string Audience { get; set; }
    }
}
