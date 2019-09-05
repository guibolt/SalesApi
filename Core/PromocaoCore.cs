using Core.Util;
using FluentValidation;
using Model;
using System;
using System.Collections.Generic;
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

        //Método para cadastrar uma promocao
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
        // Método para retornar um produto
        public Retorno BuscarumaPromocao(string id)
        {
            var umaPromocao = db.Promocoes.FirstOrDefault(c => c.Id == id);
            return umaPromocao == null ? new Retorno { Status = false, Resultado = "Registro nao existe na base de dados" } : new Retorno { Status = true, Resultado = umaPromocao };
        }

        // Método para deletar por id
        public Retorno DeletarPromocao(string id)
        {
            var umaPromocao = db.Promocoes.FirstOrDefault(c => c.Id == id);

            if (umaPromocao == null)  return new Retorno { Status = false, Resultado = "Registro nao existe na base de dados" };

            db.Promocoes.Remove(umaPromocao);
            Arq.ManipulacaoDeArquivos(false, db);
            return new Retorno { Status = true, Resultado = "Produto deletado!"};
        }
        // método para retornar todos os produtos registrados.
        public Retorno BuscarTodasPromoces() => db.Promocoes.Count == 0 ? new Retorno { Status = false, Resultado = "Não existem registros na base." }
        : new Retorno { Status = true, Resultado = db.Promocoes };
        

        // Método para exibir os registros por paginação
        public Retorno ProdutosPorPaginacao(string ordempor, int numeroPagina, int qtdRegistros)
        {
            // Limitando a quantidade registros para a paginação
            if (qtdRegistros > 50) qtdRegistros = 50;

            // checo se as paginação é valida pelas variaveis e se sim executo o skip take contendo o calculo
            if (numeroPagina > 0 && qtdRegistros > 0 && ordempor == null)
                return new Retorno { Status = true, Resultado = db.Promocoes.Skip((numeroPagina - 1) * qtdRegistros).Take(qtdRegistros).ToList() };

            // faço a verificação e depois ordeno por nome. 
            if (numeroPagina > 0 && qtdRegistros > 0 && ordempor.ToUpper().Trim() == "DESCRICAO")
                return new Retorno { Status = true, Resultado = db.Promocoes.OrderBy(c => c.Descricao.Skip((numeroPagina - 1) * qtdRegistros).Take(qtdRegistros).ToList())};

            // se nao der pra fazer a paginação
            return new Retorno { Status = true, Resultado = ($"Não foi fazer a paginação, registros totais: {db.Pedidos.Count()}, Exibindo a lista padrão:", db.Pedidos.Take(5).ToList()) };
        }

        // Método para retornar produto por data de cadastro
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

        // Método para atualizar por id
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

            Arq.ManipulacaoDeArquivos(false, db);
            return new Retorno { Status = true, Resultado = umaPromocao };
        }

        // Exibe as categorias possiveis no momento
        public Retorno ExibirCategorias() => new Retorno { Status = true, Resultado = db.ListaCategorias };
    }
}
