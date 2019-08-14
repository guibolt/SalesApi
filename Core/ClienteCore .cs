using Models;
using System;

namespace Core
{
    public class ClienteCore
    {
        private Cliente _cliente { get; set; }

        public ClienteCore() {}

        public ClienteCore(Cliente cliente)
        {
            _cliente = cliente;
        }


        public Cliente Cadastrar(Cliente cliente) => null;

        public Cliente AcharId(string id) => null;

        public Cliente AcharTodos() => null;

        public Cliente Atualizar(string id) => null;

        public void DeletarUm(string id) { }
    }
}
