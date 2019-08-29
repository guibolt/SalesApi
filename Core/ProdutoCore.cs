using Core.Util;
using FluentValidation;
using Model;
using System.Linq;

namespace Core
{
    public class ProdutoCore : AbstractValidator<Produto>
    {
        private Produto _produto;
        private Sistema db;
        public ProdutoCore()
        {
            db = Arq.ManipulacaoDeArquivos(true, null).sistema;
            if (db == null)
                db = new Sistema();
        }

        public ProdutoCore(Produto produto)
        {
             db = Arq.ManipulacaoDeArquivos(true, null).sistema;
            if (db == null)
                db = new Sistema();
        

            _produto = produto;

            RuleFor(p => p.Nome).MinimumLength(3).
                WithMessage("Erro, o nome deve ter no minimo 3 caracteres e nao pode ser nulo");
            RuleFor(p => p.Preco).GreaterThan(0).WithMessage("O preço precisa ser maior que 0 e nao pdoe ser nulo");

        }
        public Retorno CadastrarProduto()
        {
            var valida = Validate(_produto);

            if (!valida.IsValid)
                return new Retorno { Status = false, Resultado = valida.Errors.Select(a =>a.ErrorMessage).ToList() };

            if (db.Produtos.Any(c => c.Nome == _produto.Nome) )
                return new Retorno() { Status = false, Resultado = "Este produto ja está cadastrado" };

            db.Produtos.Add(_produto);
            Arq.ManipulacaoDeArquivos(false, db);

            return new Retorno() { Status = true, Resultado = _produto };
        }

        public Retorno AcharUm(string id)
        {
            if (!db.Produtos.Any(e => e.Id == id))
                return new Retorno() { Status = false, Resultado = "Esse produto nao está registrado na base de dados" };

            return new Retorno() { Status = true, Resultado = db.Produtos.Find(c => c.Id.ToString() == id) };
        }
        public Retorno AcharTodos() => new Retorno() { Status = true, Resultado = db.Produtos };

        public Retorno DeletarId(string id)
        {
            if (!db.Produtos.Any(e => e.Id == id))
                return new Retorno() { Status = false, Resultado = "Esse produto nao está registrado na base de dados" };

            db.Produtos.Remove(db.Produtos.Find(c => c.Id.ToString() == id));

            Arq.ManipulacaoDeArquivos(false, db);
            return new Retorno { Status = true, Resultado = "Registro removido!" };
        }
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
