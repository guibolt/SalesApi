using Core.Util;
using FluentValidation;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core
{
    public class PedidoCore: AbstractValidator<Pedido>
    {
        private Pedido _pedido;
        public Sistema db { get; set; }

        public PedidoCore()
        {
            db = Arq.ManipulacaoDeArquivos(true, null).sistema;
            if (db == null) db = new Sistema();
        }

        public PedidoCore(Pedido pedido)
        {
            db = Arq.ManipulacaoDeArquivos(true, null).sistema;
            if (db == null) db = new Sistema();

            _pedido = pedido;
            RuleFor(c => c.Produtos).NotEmpty().WithMessage("A lista de produtos nao pode ser vazia");
            RuleFor(c => c.Cliente).NotNull().WithMessage("O Cliente nao pode ser nulo");
            RuleFor(c => c.Produtos).Must(produto => ValidaProduto()).WithMessage("O produto está inválido.");

            RuleForEach(c => c.Produtos).Must(p => p.Quantidade > 0);

            _pedido.Produtos.ForEach(c => c.TrocandoDados(db.Produtos.FirstOrDefault(e => e.Id == c.Id)));
            _pedido.Cliente.TrocandoDados(db.Clientes.FirstOrDefault(c => c.Id == _pedido.Cliente.Id));
        }

        public Retorno RealizarUmPedido()
        {
            var valida = Validate(_pedido);
            // checa se os dados sao validos
            if (!valida.IsValid)
                return new Retorno { Status = false, Resultado =  valida.Errors.Select(c => c.ErrorMessage).ToList() };

            // checa se o cliente realmente existe na base
            if (!db.Clientes.Any(c => c.Id == _pedido.Cliente.Id))
                return new Retorno { Status = false, Resultado = "Esse cliente não existe na base de dados!" };

            // para movimentar o estoque.
            _pedido.Produtos.ForEach(d => db.Produtos.FirstOrDefault(c => c.Id == d.Id).Quantidade -= d.Quantidade);

            //calcula o total e adciona na lista
            _pedido.CalculaTotal();
            db.Pedidos.Add(_pedido);

            //procura e atribui valor total para o cliente
             db.Clientes.FirstOrDefault(c => c.Id == _pedido.Cliente.Id).TotalComprado += _pedido.ValorTotal;

            Arq.ManipulacaoDeArquivos(false, db);
            return new Retorno { Status = true, Resultado = _pedido };
        }
        public Retorno AcharTodos() => new Retorno { Status = true, Resultado = db.Pedidos };

        // Método para returnar um registro
        public Retorno BuscarProdutoPorId(string id)
        {
            var umPedido = db.Pedidos.FirstOrDefault(c => c.Id == id);
            if (umPedido == null)
                return new Retorno() { Status = false, Resultado = "Registro nao existe na base de dados" };

            return new Retorno { Status = true, Resultado = umPedido };
        }

        //Método para deletar por id
        public Retorno DeletarPedidoPorID(string id)
        {
            var umPedido = db.Pedidos.FirstOrDefault(c => c.Id == id);
            if (umPedido == null)
                return new Retorno() { Status = false, Resultado = "Registro nao existe na base de dados" };

            db.Pedidos.Remove(umPedido);

            Arq.ManipulacaoDeArquivos(false, db);
            return new Retorno { Status = true, Resultado = "Registro removido!" };
        }
        public Retorno BuscaPedidoPorData(string dataComeço, string dataFim)
        {
            // Tento fazer a conversao e checho se ela nao for feita corretamente, se ambas nao forem corretas retorno FALSE
            if (!DateTime.TryParse(dataComeço, out DateTime primeiraData) && !DateTime.TryParse(dataFim, out DateTime segundaData))
                return new Retorno { Status = false, Resultado = "Dados Invalidos" };

            // Tento fazer a conversao da segunda data for invalida faço somente a pesquisa da primeira data
            if (!DateTime.TryParse(dataFim, out segundaData))
                return new Retorno { Status = true, Resultado = db.Pedidos.Where(c => c.DataDoPedido >= primeiraData).ToList() };

            // Tento fazer a conversao da primeiradata for invalida faço somente a pesquisa da segunda data
            if (!DateTime.TryParse(dataComeço, out primeiraData))
                return new Retorno { Status = true, Resultado = db.Pedidos.Where(c => c.DataDoPedido <= segundaData).ToList() };

            // returno a lista completa entre as duas datas informadas.
            return new Retorno { Status = true, Resultado = db.Pedidos.Where(c => c.DataDoPedido >= primeiraData && c.DataDoPedido <= segundaData).ToList() };
        }
        public Retorno PedidosPorPaginacao(string ordempor, int numeroPagina, int qtdRegistros)
        {
            // checo se as paginação é valida pelas variaveis e se sim executo o skip take contendo o calculo
            if (numeroPagina > 0 && qtdRegistros > 0 && ordempor == null)
                return new Retorno { Status = true, Resultado = db.Pedidos.Skip((numeroPagina - 1) * qtdRegistros).Take(qtdRegistros).ToList() };

            // faço a verificação e depois ordeno por nome. 
            if (numeroPagina > 0 && qtdRegistros > 0 && ordempor.ToUpper().Trim() == "MENORVALOR")
                return new Retorno { Status = true, Resultado = db.Pedidos.OrderBy(c => c.ValorTotal).Skip((numeroPagina - 1) * qtdRegistros).Take(qtdRegistros).ToList() };

            // faço a verificação e depois ordeno por idade. 
            if (numeroPagina > 0 && qtdRegistros > 0 && ordempor.ToUpper().Trim() == "MAIORVALOR")
                return new Retorno { Status = true, Resultado = db.Pedidos.OrderByDescending(c => c.ValorTotal).Skip((numeroPagina - 1) * qtdRegistros).Take(qtdRegistros).ToList() };

            // se nao der pra fazer a paginação
            return new Retorno { Status = false, Resultado = new List<string>() { "Dados inválidos, nao foi possivel realizar a paginação." } };
        }

        // método para validar os produtos inseridos
        public bool ValidaProduto()
        {
            foreach (var produto in _pedido.Produtos)
            {
                if (db.Produtos.SingleOrDefault(p => p.Id == produto.Id) == null || produto.Quantidade > db.Produtos.SingleOrDefault(p => p.Id == produto.Id).Quantidade )
                    return false;
            }
            return true;
        }
    }
}
