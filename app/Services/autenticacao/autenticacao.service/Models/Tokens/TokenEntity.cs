using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace autenticacao.service.Models.Tokens
{
    public class TokenEntity
    {
        public TokenEntity(string token)
        {
            Token = token;
            DataDeCriacao = DateTime.UtcNow;
            DataDeExpiracao = DateTime.UtcNow.AddHours(1);
        }

        public string Token { get;}
        public DateTime DataDeCriacao { get;}
        public DateTime DataDeExpiracao { get;}

        public bool tokenExpirado()
        {
            if(this.DataDeExpiracao < DateTime.UtcNow) return true;
            return false;
        }
    }
}