using Core.Util;
using FluentValidation;
using Model;
using Models;
using System.Linq;

namespace Core
{
    public class ProdutoCore : AbstractValidator<Produto>
    {
        private Produto _produto;

        public ProdutoCore(){}

        public ProdutoCore(Produto produto)
        {
            _produto = produto;

            RuleFor(p => p.Nome).MinimumLength(3).
                WithMessage("Erro, o nome deve ter no minimo 3 caracteres e nao pode ser nulo");

            RuleFor(p => p.Preco).GreaterThan(0).WithMessage("O preço precisa ser maior que 0 e nao pdoe ser nulo");

   
        }

        public Retorno CadastrarProduto()
        {
            var valida = Validate(_produto);

            if (!valida.IsValid)
                return new Retorno { Status = false, Resultado = valida.Errors };

            var db = Arq.ManipulacaoDeArquivos(true, null);

            if (db.sistema == null)
                db.sistema = new Sistema();

            var produtos = db.sistema.Produtos;

            if (produtos.Any(c => c.Nome == _produto.Nome))
                return new Retorno() { Status = false, Resultado = null };

            db.sistema.Produtos.Add(_produto);
            Arq.ManipulacaoDeArquivos(false, db.sistema);

            return new Retorno() { Status = true, Resultado = _produto };
        }

        public Retorno AcharUm(string id)
        {
            var db = Arq.ManipulacaoDeArquivos(true, null);
            if (db.sistema == null)
                db.sistema = new Sistema();

            if (!db.sistema.Produtos.Exists(e => e.Id == id))
                return new Retorno() { Status = false, Resultado = null };

            var Umproduto = db.sistema.Produtos.Find(c => c.Id.ToString() == id);
            return new Retorno() { Status = true, Resultado = Umproduto };
        }


        public Retorno AcharTodos()
        {
            var db = Arq.ManipulacaoDeArquivos(true, null);
            if (db.sistema == null)
                db.sistema = new Sistema();

            return new Retorno() { Status = true, Resultado = db.sistema.Produtos };

        }


        public Retorno DeletarId(string id)
        {
            var db = Arq.ManipulacaoDeArquivos(true, null);
            if (db.sistema == null)
                db.sistema = new Sistema();

            var UmProduto = db.sistema.Produtos.Find(c => c.Id.ToString() == id);

            db.sistema.Produtos.Remove(UmProduto);

            Arq.ManipulacaoDeArquivos(false, db.sistema);

            return new Retorno { Status = true, Resultado = null };

        }

        public Retorno AtualizarUm(string id, Produto produto)
        {

            var db = Arq.ManipulacaoDeArquivos(true, null);
            if (db.sistema == null)
                db.sistema = new Sistema();

            var umProduto = db.sistema.Produtos.Find(c => c.Id.ToString() == id);
            db.sistema.Produtos.Remove(umProduto);

            if (produto.Nome != null)
                umProduto.Nome = produto.Nome;

            if (produto.Preco != 0)
                umProduto.Preco = produto.Preco;

            if (produto.Quantidade != 0)
                umProduto.Quantidade = produto.Quantidade;

            if (produto.Id != null)
                umProduto.Id = produto.Id;

            db.sistema.Produtos.Add(umProduto);

            Arq.ManipulacaoDeArquivos(false, db.sistema);

            return new Retorno() { Status = true, Resultado = umProduto };
        }

    }
}
