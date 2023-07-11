using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace estoque.service.AssynComm
{
    public interface IMessagePublisher
    {
        void publicarProduto(ProdutoDisponivel produto);
    }

}