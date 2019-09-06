using Core.Util;
using FluentValidation;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core
{  // Classe contendo as regras de negocio
    public class ProdutoCore : AbstractValidator<Produto>
    {
        private Produto _produto;
        public Sistema db { get; set; }

        // Construtor sem argumento inicando a base dados
        public ProdutoCore()
        {
            db = Arq.ManipulacaoDeArquivos(true, null).sistema;
            db = db ?? new Sistema();
        }
        // Construtor com a validação
        public ProdutoCore(Produto produto)
        {
            db = Arq.ManipulacaoDeArquivos(true, null).sistema;

            db = db ?? new Sistema();

            _produto = produto;

            RuleFor(p => p.Nome).MinimumLength(3).
                WithMessage("Erro, o nome deve ter no minimo 3 caracteres e nao pode ser nulo");
            RuleFor(p => p.Preco).GreaterThan(0).NotEmpty().WithMessage("O preço precisa ser maior que 0 e nao pdoe ser vazio");
            RuleFor(p => p.Quantidade).NotEmpty().GreaterThan(0).WithMessage("A quantidade do produto deve ser de no minimo um produto");
            RuleFor(p => p.Categoria).IsInEnum().WithMessage("Categoria nao existe.");
        }
        /// <summary>
        /// Método para cadastrar um produto
        /// </summary>
        /// <returns></returns>
        public Retorno CadastrarUmProduto()
        {
            var valida = Validate(_produto);

            if (!valida.IsValid)
                return new Retorno { Status = false, Resultado = valida.Errors.Select(a => a.ErrorMessage).ToList() };

            if (db.Produtos.Any(c => c.Nome == _produto.Nome))
                return new Retorno() { Status = false, Resultado = "Este produto ja está cadastrado" };

            db.Produtos.Add(_produto);
            Arq.ManipulacaoDeArquivos(false, db);

            return new Retorno() { Status = true, Resultado = _produto };
        }
        
        /// <summary>
        ///  Método para retornar um produto se baseando no id fornecido.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Retorno BuscarUmProduto(string id)
        {
            var umProduto = db.Produtos.FirstOrDefault(c => c.Id == id);
            return umProduto == null ? new Retorno { Status = false, Resultado = "Registro nao existe na base de dados" } : new Retorno { Status = true, Resultado = umProduto };
        }

        /// <summary>
        /// Método para deletar um produto baseando no id fornecido.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // Método para deletar por id
        public Retorno DeletarProdutoPorId(string id)
        {
            var umProduto = db.Produtos.FirstOrDefault(c => c.Id == id);

            if (umProduto == null) return new Retorno { Status = false, Resultado = "Registro nao existe na base de dados" };

            db.Produtos.Remove(umProduto);

            Arq.ManipulacaoDeArquivos(false, db);

            return new Retorno { Status = true, Resultado = "Produto deletado!" };
        }
        /// <summary>
        /// método para retornar todos os produtos registrados na base de dados
        /// </summary>
        /// <returns></returns>
        public Retorno BuscarTodosProdutos()
        {
            var todosProdutos = db.Produtos;
            return todosProdutos.Count == 0 ? new Retorno { Status = false, Resultado = "Não existem registros na base." } : new Retorno { Status = true, Resultado = todosProdutos };
        }

        /// <summary>
        /// Método para exibir os pedidos por paginação
        /// </summary>
        /// <param name="ordempor"></param>
        /// <param name="numeroPagina"></param>
        /// <param name="qtdRegistros"></param>
        /// <returns></returns>
        public Retorno ProdutosPorPaginacao(string ordempor, int numeroPagina, int qtdRegistros)
        {
            // Limitando a quantidade registros para a paginação
            if (qtdRegistros > 50) qtdRegistros = 50;

            // checo se as paginação é valida pelas variaveis e se sim executo o skip take contendo o calculo
            if (numeroPagina > 0 && qtdRegistros > 0 && ordempor == null)
                return new Retorno { Status = true, Resultado = db.Produtos.Skip((numeroPagina - 1) * qtdRegistros).Take(qtdRegistros).ToList() };

            // faço a verificação e depois ordeno por nome. 
            if (numeroPagina > 0 && qtdRegistros > 0 && ordempor.ToUpper().Trim() == "NOME")
                return new Retorno { Status = true, Resultado = db.Produtos.OrderBy(c => c.Nome).Skip((numeroPagina - 1) * qtdRegistros).Take(qtdRegistros).ToList() };

            // faço a verificação e depois ordeno por menor preço. 
            if (numeroPagina > 0 && qtdRegistros > 0 && ordempor.ToUpper().Trim() == "MENORPRECO")
                return new Retorno { Status = true, Resultado = db.Produtos.OrderBy(c => c.Preco).Skip((numeroPagina - 1) * qtdRegistros).Take(qtdRegistros).ToList() };
            // faço a verificação e depois ordeno por maior. 
            if (numeroPagina > 0 && qtdRegistros > 0 && ordempor.ToUpper().Trim() == "MAIORPRECO")
                return new Retorno { Status = true, Resultado = db.Produtos.OrderByDescending(c => c.Preco).Skip((numeroPagina - 1) * qtdRegistros).Take(qtdRegistros).ToList() };

            // se nao der pra fazer a paginação

            return db.Produtos.Count == 0 ? new Retorno { Status = false, Resultado = "Não tem nenhum item na Lista" } :
                new Retorno { Status = true, Resultado = ($"Não foi fazer a paginação, registros totais: {db.Produtos.Count()}, Exibindo a lista padrão:", db.Produtos.Take(5).ToList()) };
        }
        /// <summary>
        /// Método para retornar produto por data.
        /// </summary>
        /// <param name="dataComeço"></param>
        /// <param name="dataFim"></param>
        /// <returns></returns>
        public Retorno BuscaProdutoPorData(string dataComeço, string dataFim)
        {
            // Tento fazer a conversao e checho se ela nao for feita corretamente, se ambas nao forem corretas retorno FALSE
            if (!DateTime.TryParse(dataComeço, out DateTime primeiraData) && !DateTime.TryParse(dataFim, out DateTime segundaData))
                return new Retorno { Status = false, Resultado = "Dados Invalidos" };

            // Tento fazer a conversao da segunda data for invalida faço somente a pesquisa da primeira data
            if (!DateTime.TryParse(dataFim, out segundaData))
                return new Retorno { Status = true, Resultado = db.Produtos.Where(c => Convert.ToDateTime(c.DataCadastro) >= primeiraData).ToList() };

            // Tento fazer a conversao da primeiradata for invalida faço somente a pesquisa da segunda data
            if (!DateTime.TryParse(dataComeço, out primeiraData))
                return new Retorno { Status = true, Resultado = db.Produtos.Where(c => Convert.ToDateTime(c.DataCadastro) <= segundaData).ToList() };

            // returno a lista completa entre as duas datas informadas.
            return new Retorno { Status = true, Resultado = db.Produtos.Where(c => Convert.ToDateTime(c.DataCadastro) >= primeiraData && Convert.ToDateTime(c.DataCadastro) <= segundaData).ToList() };
        }
        /// <summary>
        /// Método para atualizar um produto por id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="produto"></param>
        /// <returns></returns>
        public Retorno AtualizarUmProduto(string id, Produto produto)
        {
            var umProduto = db.Produtos.FirstOrDefault(c => c.Id == id);

            if (umProduto == null) return new Retorno { Status = false, Resultado = "Esse produto nao está registrado na base de dados" };

            if (produto.Nome != null)
                umProduto.Nome = produto.Nome;

            if (produto.Preco != 0)
                umProduto.Preco = produto.Preco;

            if (produto.Quantidade != 0)
                umProduto.Quantidade = produto.Quantidade;

            Arq.ManipulacaoDeArquivos(false, db);
            return new Retorno { Status = true, Resultado = umProduto };
        }

        /// <summary>
        /// método para a exibição das categorias dos produtos.
        /// </summary>
        /// <returns></returns>
        public Retorno ExibirCategorias() => new Retorno
        {
            Status = true,
            Resultado = new List<string>
            {
             "SMARTPHONES Categoria: 1",
            "INFORMATICA Categoria: 2",
            "GAMES Categoria: 3",
            "VESTUARIO Categoria: 4",
            "SAUDE Categoria: 5",
            "FITNESS Categoria: 6",
            "MOVEIS Categoria: 7",
            "BRINQUEDOS Categoria: 8",
            "COSMETICOS Categoria: 9",
            "ELECTRODOMESTICOS Categoria: 10", }
        };
    }
}
