using Core.Util;
using FluentValidation;
using Model;
using System;
using System.Linq;

namespace Core
{  // Classe contendo as regras de negocio
    public class PromocaoCore : AbstractValidator<Promocao>
    {
        private Promocao _promocao;
        public Sistema db { get; set; }

        // Construtor sem argumento inicando a base dados
        public PromocaoCore()
        {
            db = Arq.ManipulacaoDeArquivos(true, null).sistema;
            db = db ?? new Sistema();
        }
        // Construtor com a validação
        public PromocaoCore(Promocao promocao)
        {
            db = Arq.ManipulacaoDeArquivos(true, null).sistema;

            db = db ?? new Sistema();

            _promocao = promocao;

            RuleFor(p => p.Descricao).MinimumLength(3).WithMessage("Erro, o nome deve ter no minimo 3 caracteres e nao pode ser nulo");
            RuleFor(p => p.DataFinal).GreaterThan(DateTime.Now);
            RuleFor(p => p.TaxaDesconto).GreaterThan(0).LessThan(90).WithMessage("Taxa precisa estar entre 1 e 90");
            RuleFor(p => p.Categoria).IsInEnum().WithMessage("Categoria nao existe.");
        }
        /// <summary>
        /// Método para cadastrar uma promocao
        /// </summary>
        /// <returns></returns>
        public Retorno CadastrarPromocao()
        {
            var valida = Validate(_promocao);

            if (!valida.IsValid)
                return new Retorno { Status = false, Resultado = valida.Errors.Select(a => a.ErrorMessage).ToList() };

            if (db.Promocoes.Any(c => c.Descricao == _promocao.Descricao))
                return new Retorno() { Status = false, Resultado = "Essa promocão ja está cadastrada" };

            db.Promocoes.Add(_promocao);
            Arq.ManipulacaoDeArquivos(false, db);

            return new Retorno() { Status = true, Resultado = _promocao };
        }
        /// <summary>
        /// Método para retornar um produto se baseando no id fornecido.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Retorno BuscarumaPromocao(string id)
        {
            var umaPromocao = db.Promocoes.FirstOrDefault(c => c.Id == id);
            return umaPromocao == null ? new Retorno { Status = false, Resultado = "Registro nao existe na base de dados" } : new Retorno { Status = true, Resultado = umaPromocao };
        }
        /// <summary>
        /// Método para deletar uma promocao se baseando no id fornecido.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Retorno DeletarPromocao(string id)
        {
            var umaPromocao = db.Promocoes.FirstOrDefault(c => c.Id == id);

            if (umaPromocao == null) return new Retorno { Status = false, Resultado = "Registro nao existe na base de dados" };

            db.Promocoes.Remove(umaPromocao);
            Arq.ManipulacaoDeArquivos(false, db);
            return new Retorno { Status = true, Resultado = "Produto deletado!" };
        }
        /// <summary>
        ///  método para retornar todos os produtos registrados na base de dados.
        /// </summary>
        /// <returns></returns>
        public Retorno BuscarTodasPromocoes()
        {
            var todasPromocoes = db.Promocoes;
            return todasPromocoes.Count == 0 ? new Retorno { Status = false, Resultado = "Não existem registros na base." } : new Retorno { Status = true, Resultado = todasPromocoes };
        }

        /// <summary>
        /// Método para fazer a exibição por paginação
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
                return new Retorno { Status = true, Resultado = db.Promocoes.Skip((numeroPagina - 1) * qtdRegistros).Take(qtdRegistros).ToList() };

            // faço a verificação e depois ordeno por nome. 
            if (numeroPagina > 0 && qtdRegistros > 0 && ordempor.ToUpper().Trim() == "DESCRICAO")
                return new Retorno { Status = true, Resultado = db.Promocoes.OrderBy(c => c.Descricao.Skip((numeroPagina - 1) * qtdRegistros).Take(qtdRegistros).ToList()) };

            return db.Promocoes.Count == 0 ? new Retorno { Status = false, Resultado = "Não tem nenhum item na Lista" } :
                new Retorno { Status = true, Resultado = ($"Não foi fazer a paginação, registros totais: {db.Promocoes.Count()}, Exibindo a lista padrão:", db.Promocoes.Take(5).ToList()) };
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
                return new Retorno { Status = true, Resultado = db.Promocoes.Where(c => Convert.ToDateTime(c.DataCadastro) >= primeiraData).ToList() };

            // Tento fazer a conversao da primeiradata for invalida faço somente a pesquisa da segunda data
            if (!DateTime.TryParse(dataComeço, out primeiraData))
                return new Retorno { Status = true, Resultado = db.Promocoes.Where(c => Convert.ToDateTime(c.DataCadastro) <= segundaData).ToList() };

            // returno a lista completa entre as duas datas informadas.
            return new Retorno { Status = true, Resultado = db.Promocoes.Where(c => Convert.ToDateTime(c.DataCadastro) >= primeiraData && Convert.ToDateTime(c.DataCadastro) <= segundaData).ToList() };
        }

        /// <summary>
        /// Método para atualizar uma promocão por id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="promocao"></param>
        /// <returns></returns>
        public Retorno AtualizarumaPromocao(string id, Promocao promocao)
        {
            var umaPromocao = db.Promocoes.FirstOrDefault(c => c.Id == id);

            if (umaPromocao == null) return new Retorno { Status = false, Resultado = "Esse produto nao está registrado na base de dados" };

            if (promocao.Descricao != null)
                umaPromocao.Descricao = promocao.Descricao;

            if (promocao.TaxaDesconto != 0)
                umaPromocao.TaxaDesconto = promocao.TaxaDesconto;

            if (promocao.Categoria != 0)
                umaPromocao.Categoria = promocao.Categoria;

            if (promocao.DataFinal != null)
                umaPromocao.DataFinal = promocao.DataFinal;

            if (promocao.Concluida)
                umaPromocao.Concluida = promocao.Concluida;

            Arq.ManipulacaoDeArquivos(false, db);
            return new Retorno { Status = true, Resultado = umaPromocao };
        }
    }
}
