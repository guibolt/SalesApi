
using Models;
using System;

namespace Core
{
    public class ProdutoCore
    {
        private Produto _produto { get; set; }

        public ProdutoCore()
        {

        }

        public ProdutoCore(Produto produto)
        {
            _produto = produto;
        }

        public Produto Cadastrar(Produto produto) => null;

        public Produto AcharId(string id) => null;

        public Produto AcharTodos() => null;

        public Produto Atualizar(string id) => null;

        public void DeletarUm(string id) { }


    }
}
