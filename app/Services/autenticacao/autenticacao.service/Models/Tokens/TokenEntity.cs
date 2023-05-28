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
        [DataType("NVARCHAR")]
        public string Token { get;}
        
        [DataType("DATETIME")]
        public DateTime DataDeCriacao { get;}
        
        [DataType("DATETIME")]
        public DateTime DataDeExpiracao { get;}

        public bool tokenExpirado()
        {
            if(this.DataDeExpiracao < DateTime.UtcNow) return true;
            return false;
        }
    }
}