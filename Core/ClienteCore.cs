using Core.Util;
using FluentValidation;
using Model;
using Models;
using System.Linq;

namespace Core
{
    public class ClienteCore : AbstractValidator<Cliente>
    {
        private Cliente _cliente;

        public ClienteCore(){}

        public ClienteCore(Cliente Cliente) {
            _cliente = Cliente;

            RuleFor(c => c.Idade).GreaterThan(6).WithMessage("A idade do cliente deve ser maior que 6");
            RuleFor(c => c.Nome).MinimumLength(3).NotNull().WithMessage("O nome deve conter mais que 2 caracteres");
            RuleFor(c => c.Documento).Length(11, 11).NotNull().WithMessage("Cpf inválido");
            RuleFor(c => c.Sexo.ToUpper()).NotNull().Must(c => c == "MASCULINO"|| c == "FEMININO").WithMessage($"Campo {_cliente.Sexo.GetType()} não pode ser nulo");
            
        }

        public Retorno CadastrarProduto()
        {
            var valida = Validate(_cliente);

            if (!valida.IsValid)
                return new Retorno { Status = false, Resultado = valida.Errors };

            var db = Arq.ManipulacaoDeArquivos(true, null);

            if (db.sistema == null)
                db.sistema = new Sistema();

            var clientes = db.sistema.Clientes;

            if (clientes.Any(c => c.Nome == _cliente.Nome))
                return new Retorno() { Status = false, Resultado = null };

            db.sistema.Clientes.Add(_cliente);
            Arq.ManipulacaoDeArquivos(false, db.sistema);

            return new Retorno() { Status = true, Resultado = _cliente };
        }

        public Retorno AcharUm(string id)
        {
            var db = Arq.ManipulacaoDeArquivos(true, null);
            if (db.sistema == null)
                db.sistema = new Sistema();

            if (!db.sistema.Clientes.Exists(e => e.Id.ToString() == id))
                return new Retorno() { Status = false, Resultado = null };

            var umCliente = db.sistema.Clientes.Find(c => c.Id == id);
            return new Retorno() { Status = true, Resultado = umCliente };
        }

        public Retorno AcharTodos()
        {
            var db = Arq.ManipulacaoDeArquivos(true, null);
            if (db.sistema == null)
                db.sistema = new Sistema();

            return new Retorno() { Status = true, Resultado = db.sistema.Clientes };

        }
        public Retorno DeletarId(string id)
        {
            var db = Arq.ManipulacaoDeArquivos(true, null);
            if (db.sistema == null)
                db.sistema = new Sistema();

            var UmCliente= db.sistema.Clientes.Find(c => c.Id.ToString() == id);

            db.sistema.Clientes.Remove(UmCliente);

            Arq.ManipulacaoDeArquivos(false, db.sistema);

            return new Retorno { Status = true, Resultado = null };

        }

        public Retorno AtualizarUm(string id, Cliente cliente)
        {

            var db = Arq.ManipulacaoDeArquivos(true, null);
            if (db.sistema == null)
                db.sistema = new Sistema();

            var umCliente = db.sistema.Clientes.Find(c => c.Id == id);
            db.sistema.Clientes.Remove(umCliente);

            if (cliente.Documento != null)
                umCliente.Documento = cliente.Documento;

            if (cliente.Id != null)
                umCliente.Id = cliente.Id;

            if (cliente.Sexo != null)
                umCliente.Sexo = cliente.Sexo;

            if (cliente.Nome != null)
                 umCliente.Nome = cliente.Nome;

            db.sistema.Clientes.Add(umCliente);

            Arq.ManipulacaoDeArquivos(false, db.sistema);

            return new Retorno() { Status = true, Resultado = umCliente };
        }

    }
}
