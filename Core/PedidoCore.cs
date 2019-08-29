using Core.Util;
using FluentValidation;
using Model;
using System.Linq;

namespace Core
{
    public class PedidoCore: AbstractValidator<Pedido>
    {
        private Pedido _pedido;
        private Sistema db;

        public PedidoCore()
        {
            db = Arq.ManipulacaoDeArquivos(true, null).sistema;
            if (db == null) db = new Sistema();
        }

        public PedidoCore(Pedido pedido)
        {
            _pedido = pedido;

            RuleFor(c => c.Produtos).NotEmpty().WithMessage("A lista de produtos nao pode ser vazia");
            RuleFor(c => c.Cliente).NotNull().WithMessage("O Cliente nao pode ser nulo");
            RuleFor(c => c.Produtos).Must(temp => ValidaProduto()).WithMessage("O produto está inválido.");

            db = Arq.ManipulacaoDeArquivos(true, null).sistema;
            if (db == null) db = new Sistema();
        }

        public Retorno RealizarPedido()
        {
            var valida = Validate(_pedido);
            // checa se os dados sao validos
            if (!valida.IsValid)
                return new Retorno { Status = false, Resultado =  valida.Errors.Select(c => c.ErrorMessage).ToList() };

            // checa se o cliente realmente existe na base
           
            if (!db.Clientes.Any(c => c.Id == _pedido.Cliente.Id))
                return new Retorno { Status = false, Resultado = "Esse cliente não existe na base de dados!" };

            //calcula o total e adciona na lista
            _pedido.CalculaTotal();
            db.Pedidos.Add(_pedido);

            Arq.ManipulacaoDeArquivos(false, db);
            return new Retorno { Status = true, Resultado = _pedido };
        }
        public Retorno AcharTodos() => new Retorno() { Status = true, Resultado = db.Pedidos };

        public Retorno AcharUm(string id)
        {
            if (!db.Pedidos.Any(e => e.Id == id))
                return new Retorno() { Status = false, Resultado = "Esse produto nao está registrado na base de dados" };

            return new Retorno() { Status = true, Resultado = db.Pedidos.Find(c => c.Id.ToString() == id) };
        }

        public Retorno DeletarId(string id)
        {
            if (!db.Pedidos.Any(e => e.Id == id))
                return new Retorno() { Status = false, Resultado = "Esse pedido nao está registrado na base de dados" };

            db.Pedidos.Remove(db.Pedidos.Find(c => c.Id == id));

            Arq.ManipulacaoDeArquivos(false, db);
            return new Retorno { Status = true, Resultado = "Registro removido!" };
        }
    
        // metodo para validar os produtos inseridos
        public bool ValidaProduto()
        {
            foreach (var produto in _pedido.Produtos)
            {
                if (db.Produtos.SingleOrDefault(p => p.Id == produto.Id) == null || produto.Quantidade > db.Produtos.SingleOrDefault(p => p.Id == produto.Id).Quantidade)
                    return false;
            }

            return true;
        }
    }
}
