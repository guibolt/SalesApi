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
            RuleFor(p => p.Preco).GreaterThan(0).WithMessage("O preço precisa ser maior que 0 e nao pdoe ser nulo");


        }

       
        public Retorno CadastrarProduto()
        {
            var valida = Validate(_produto);

            if (!valida.IsValid)

                return new Retorno { Status = false, Resultado = valida.Errors.Select(a =>a.ErrorMessage).ToList() };

            if (db.Produtos.Any(c => c.Nome == _produto.Nome) )
                return new Retorno() { Status = false, Resultado = "Este produto ja está cadastrado" };

            var produtos = db.Produtos;

            if (produtos.Any(c => c.Nome == _produto.Nome))
                return new Retorno() { Status = false, Resultado = null };


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
     

        public Retorno DeletarId(string id)
        {
            if (!db.Produtos.Any(e => e.Id == id))
                return new Retorno() { Status = false, Resultado = "Esse produto nao está registrado na base de dados" };
  
            if (!db.Produtos.Exists(e => e.Id == id))
                return new Retorno() { Status = false, Resultado = null };

            var Umproduto = db.Produtos.Find(c => c.Id.ToString() == id);
            return new Retorno() { Status = true, Resultado = Umproduto };
        }
        // método para retornar todos os produtos registrados.
        public Retorno AcharTodos() => new Retorno() { Status = true, Resultado = db.Produtos.OrderBy(n => n.Nome) };

        // Método para deletar por id
      
             

        // Método para atualizar por id
        public Retorno AtualizarUm(string id, Produto produto)
        {
            if (!db.Produtos.Exists(e => e.Id == id))
                return new Retorno() { Status = false, Resultado = null };

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
