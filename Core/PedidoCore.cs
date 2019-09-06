using AutoMapper;
using Core.Util;
using FluentValidation;
using Model;
using System;
using System.Linq;

namespace Core
{
    public class PedidoCore : AbstractValidator<Pedido>
    {
        private Pedido _pedido;
        public Sistema db { get; set; }

        public PedidoCore()
        {
            
            db = Arq.ManipulacaoDeArquivos(true, null).sistema;
            db = db ?? new Sistema();
        }

        public PedidoCore(Pedido pedido)
        {

            
            db = Arq.ManipulacaoDeArquivos(true, null).sistema;
            db = db ?? new Sistema();

            //_pedido = _mapper.Map<PedidoView,Pedido>(pedido);
            _pedido = pedido;
            RuleFor(c => c.Produtos).NotEmpty().WithMessage("A lista de produtos nao pode ser vazia");
            RuleFor(c => c.Cliente).NotNull().WithMessage("O Cliente nao pode ser nulo");
            RuleForEach(c => c.Produtos).
                Must(produto => db.Produtos.SingleOrDefault(p => p.Id == produto.Id) == null || produto.Quantidade > db.Produtos.SingleOrDefault(p => p.Id == produto.Id).Quantidade ? false : true).
                WithMessage("O produto está inválido.");

            RuleForEach(c => c.Produtos).Must(p => p.Quantidade > 0);

            _pedido.Produtos.ForEach(c => c.TrocandoDados(db.Produtos.FirstOrDefault(e => e.Id == c.Id)));
            _pedido.Cliente.TrocandoDados(db.Clientes.FirstOrDefault(c => c.Id == _pedido.Cliente.Id));
        }

        /// <summary>
        /// Método para relização do pedido.
        /// </summary>
        /// <returns></returns>
        public Retorno RealizarUmPedido()
        {
            var valida = Validate(_pedido);
            // checa se os dados sao validos
            if (!valida.IsValid)
                return new Retorno { Status = false, Resultado = valida.Errors.Select(c => c.ErrorMessage).ToList() };

            // checa se o cliente realmente existe na base
            if (!db.Clientes.Any(c => c.Id == _pedido.Cliente.Id))
                return new Retorno { Status = false, Resultado = "Esse cliente não existe na base de dados!" };

            // Testa se as promocoes sao validas, se sim as executa.
            ValidaTodasPromocoes(_pedido);
             //   _pedido.Produtos.ForEach(p => db.Promocoes.FirstOrDefault(c => c.Categoria == p.Categoria).MudaValor(p));

            // para movimentar o estoque.
            _pedido.Produtos.ForEach(d => db.Produtos.FirstOrDefault(c => c.Id == d.Id).Quantidade -= d.Quantidade);

            //calcula o total e adciona na lista
            _pedido.CalculaTotal();

            //procura e atribui valor total para o cliente
            db.Clientes.FirstOrDefault(c => c.Id == _pedido.Cliente.Id).TotalComprado += _pedido.ValorTotal;

            //adciona o cliente na lista
            db.Pedidos.Add(_pedido);

            Arq.ManipulacaoDeArquivos(false, db);
            return new Retorno { Status = true, Resultado = _pedido };
        }

        /// <summary>
        /// Método para buscar todos pedidos da base de dados.
        /// </summary>
        /// <returns></returns>
        public Retorno BuscarTodosPedidos()
        {
            var todosPedidos = db.Pedidos;
            return todosPedidos.Count == 0 ? new Retorno { Status = false, Resultado = "Não existem registros na base" } : new Retorno { Status = true, Resultado = db.Pedidos };
        }

        /// <summary>
        /// Método para retornar um pedido especifico baseado no id fornecido.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Retorno BuscarProdutoPorId(string id)
        {
            var umPedido = db.Pedidos.FirstOrDefault(c => c.Id == id);
            return umPedido == null ? new Retorno { Status = false, Resultado = "Registro nao existe na base de dados" } : new Retorno { Status = true, Resultado = umPedido };
        }

        /// <summary>
        /// Método para deletar pedido por id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Retorno DeletarPedidoPorID(string id)
        {
            var umPedido = db.Pedidos.FirstOrDefault(c => c.Id == id);
            if (umPedido == null) new Retorno { Status = false, Resultado = "Registro nao existe na base de dados" };

            db.Pedidos.Remove(umPedido);

            Arq.ManipulacaoDeArquivos(false, db);
            return new Retorno { Status = true, Resultado = "Registro removido!" };
        }
        /// <summary>
        /// Método para retornar pedido por data
        /// </summary>
        /// <param name="dataComeço"></param>
        /// <param name="dataFim"></param>
        /// <returns></returns>
        public Retorno BuscaPedidoPorData(string dataComeço, string dataFim)
        {
            // Tento fazer a conversao e checho se ela nao for feita corretamente, se ambas nao forem corretas retorno FALSE
            if (!DateTime.TryParse(dataComeço, out DateTime primeiraData) && !DateTime.TryParse(dataFim, out DateTime segundaData))
                return new Retorno { Status = false, Resultado = "Dados Invalidos" };

            // Tento fazer a conversao da segunda data for invalida faço somente a pesquisa da primeira data
            if (!DateTime.TryParse(dataFim, out segundaData))
                return new Retorno { Status = true, Resultado = db.Pedidos.Where(c => Convert.ToDateTime(c.DataCadastro) >= primeiraData).ToList() };

            // Tento fazer a conversao da primeiradata for invalida faço somente a pesquisa da segunda data
            if (!DateTime.TryParse(dataComeço, out primeiraData))
                return new Retorno { Status = true, Resultado = db.Pedidos.Where(c => Convert.ToDateTime(c.DataCadastro) <= segundaData).ToList() };

            // returno a lista completa entre as duas datas informadas.
            return new Retorno { Status = true, Resultado = db.Pedidos.Where(c => Convert.ToDateTime(c.DataCadastro) >= primeiraData && Convert.ToDateTime(c.DataCadastro) <= segundaData).ToList() };
        }

        /// <summary>
        ///  Método para exibir os registros por paginação
        /// </summary>
        /// <param name="ordempor"></param>
        /// <param name="numeroPagina"></param>
        /// <param name="qtdRegistros"></param>
        /// <returns></returns>
        public Retorno PedidosPorPaginacao(string ordempor, int numeroPagina, int qtdRegistros)
        {
            // Limitando a quantidade registros para a paginação
            if (qtdRegistros > 50) qtdRegistros = 50;

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

            return db.Pedidos.Count == 0 ? new Retorno { Status = false, Resultado = "Não tem nenhum item na Lista" } :
                new Retorno { Status = true, Resultado = ($"Não foi fazer a paginação, registros totais: {db.Pedidos.Count()}, Exibindo a lista padrão:", db.Pedidos.Take(5).ToList()) };
        }



        /// <summary>
        /// Método para realizar a validacao de todas as possiveis promocoes dos produtos na lista de pedido em questão.
        /// </summary>
        /// <param name="pedido"></param>
        /// <returns></returns>

        public void ValidaTodasPromocoes(Pedido pedido)
        {

            foreach (var produto in pedido.Produtos)
            {
                if (db.Promocoes.FirstOrDefault(c => c.Categoria == produto.Categoria)!=null)
                    db.Promocoes.FirstOrDefault(c => c.Categoria == produto.Categoria).MudaValor(produto);
                    
            }

        }
    }
}