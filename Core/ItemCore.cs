using Models;
using System;

namespace Core
{
    public class ItemCore
    {
        private Item _item { get; set; }
        public ItemCore()
        {

        }

        public ItemCore(Item item)
        {
            _item = item;
        }

        public Item Cadastrar(Item item) => null;

        public Item AcharId(string id) => null;

        public Item AcharTodos() => null;

        public Item Atualizar(string id) => null;

        public void DeletarUm(string id) { }

    }
}
