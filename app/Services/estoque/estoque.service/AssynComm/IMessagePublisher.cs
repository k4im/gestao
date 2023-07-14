using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace estoque.service.AssynComm
{
    public interface IMessagePublisher
    {
        void publicarProduto(Produto produto);
        void atualizarProduto(Produto produto);
        void deletarProduto(Produto produto);
    }

}