using Core.Util;
using FluentValidation;
using Model;
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
            if (db == null) db = new Sistema();
        }
        // Construtor com a validação
        public ProdutoCore(Produto produto)
        {
            db = Arq.ManipulacaoDeArquivos(true, null).sistema;
            if (db == null)
                db = new Sistema();

            if (db == null) db = new Sistema();

            _produto = produto;

            RuleFor(p => p.Nome).MinimumLength(3).
                WithMessage("Erro, o nome deve ter no minimo 3 caracteres e nao pode ser nulo");
            RuleFor(p => p.Preco).GreaterThan(0).NotEmpty().WithMessage("O preço precisa ser maior que 0 e nao pdoe ser vazio");
        }

        //Método para cadastrar um produto
        public Retorno CadastrarProduto()
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
        // Método para retornar um produto
        public Retorno AcharUm(string id)
        {
            if (!db.Produtos.Any(e => e.Id == id))
                return new Retorno() { Status = false, Resultado = "Esse produto nao está registrado na base de dados" };

            return new Retorno() { Status = true, Resultado = db.Produtos.Find(c => c.Id.ToString() == id) };
        }

        // Método para deletar por id
        public Retorno DeletarId(string id)
        {
            if (!db.Produtos.Any(e => e.Id == id))
                return new Retorno() { Status = false, Resultado = "Esse produto nao está registrado na base de dados" };

            return new Retorno() { Status = true, Resultado = db.Produtos.Find(c => c.Id.ToString() == id) };
        }
        // método para retornar todos os produtos registrados.
        public Retorno AcharTodos() => new Retorno() { Status = true, Resultado = db.Produtos.OrderBy(n => n.Nome) };

        public Retorno PorPaginacao(string ordempor, int numeroPagina, int qtdRegistros)
        {
            // checo se as paginação é valida pelas variaveis e se sim executo o skip take contendo o calculo
            if (numeroPagina > 0 && qtdRegistros > 0 && ordempor == null)
                return new Retorno() { Status = true, Resultado = db.Produtos.Skip((numeroPagina - 1) * qtdRegistros).Take(qtdRegistros).ToList() };

            // faço a verificação e depois ordeno por nome. 
            if (numeroPagina > 0 && qtdRegistros > 0 && ordempor.ToUpper().Trim() == "NOME")
                return new Retorno() { Status = true, Resultado = db.Produtos.OrderBy(c => c.Nome).Skip((numeroPagina - 1) * qtdRegistros).Take(qtdRegistros).ToList() };

            // faço a verificação e depois ordeno por menor preço. 
            if (numeroPagina > 0 && qtdRegistros > 0 && ordempor.ToUpper().Trim() == "MENORPRECO")
                return new Retorno() { Status = true, Resultado = db.Produtos.OrderBy(c => c.Preco).Skip((numeroPagina - 1) * qtdRegistros).Take(qtdRegistros).ToList() };
            // faço a verificação e depois ordeno por maior. 
            if (numeroPagina > 0 && qtdRegistros > 0 && ordempor.ToUpper().Trim() == "MAIORPRECO")
                return new Retorno() { Status = true, Resultado = db.Produtos.OrderByDescending(c => c.Preco).Skip((numeroPagina - 1) * qtdRegistros).Take(qtdRegistros).ToList() };
            // se nao der pra fazer a paginação
            return new Retorno() { Status = false, Resultado = "Dados inválidos, nao foi possivel realizar a paginação." };
        }

        // Método para atualizar por id
        public Retorno AtualizarUm(string id, Produto produto)
        {
            if (!db.Produtos.Any(e => e.Id == id))
                return new Retorno() { Status = false, Resultado = "Esse produto nao está registrado na base de dados" };

            var umProduto = db.Produtos.Find(c => c.Id.ToString() == id);

            if (produto.Nome != null)
                umProduto.Nome = produto.Nome;

            if (produto.Preco != 0)
                umProduto.Preco = produto.Preco;

            if (produto.Quantidade != 0)
                umProduto.Quantidade = produto.Quantidade;


            Arq.ManipulacaoDeArquivos(false, db);
            return new Retorno() { Status = true, Resultado = umProduto };
        }
    }
}
