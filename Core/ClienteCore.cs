using Core.Util;
using FluentValidation;
using Model;
using System.Linq;

namespace Core
{
    public class ClienteCore : AbstractValidator<Cliente>
    {
        private Cliente _cliente;
        private Sistema db;
        public ClienteCore()
        {
            db = Arq.ManipulacaoDeArquivos(true, null).sistema;

            if (db == null)  db = new Sistema();
        }

        public ClienteCore(Cliente Cliente) {

            db = Arq.ManipulacaoDeArquivos(true, null).sistema;

            if (db == null) db = new Sistema();
        

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
                return new Retorno { Status = false, Resultado = valida.Errors.Select(c => c.ErrorMessage).ToList() };

            if (db.Clientes.Any(c => c.Nome == _cliente.Nome) || db.Clientes.Any(c => c.Documento == _cliente.Documento))
                return new Retorno() { Status = false, Resultado = "Cliente já registrado" };

            db.Clientes.Add(_cliente);
            Arq.ManipulacaoDeArquivos(false, db);

            return new Retorno() { Status = true, Resultado = _cliente };
        }

        public Retorno AcharUm(string id)
        {
            if (!db.Clientes.Exists(e => e.Id.ToString() == id))
                return new Retorno() { Status = false, Resultado = "Registro nao existe na base de dados" };

            var umCliente = db.Clientes.Find(c => c.Id == id);
            return new Retorno() { Status = true, Resultado = umCliente };
        }

        public Retorno AcharTodos() =>  new Retorno() { Status = true, Resultado = db.Clientes };

        
        public Retorno DeletarId(string id)
        {
            db.Clientes.Remove(db.Clientes.Find(c => c.Id.ToString() == id));

            Arq.ManipulacaoDeArquivos(false, db);

            return new Retorno { Status = true, Resultado = "Registro deletado!" };

        }

        public Retorno AtualizarUm(string id, Cliente cliente)
        {

            var umCliente = db.Clientes.Find(c => c.Id == id);
            db.Clientes.Remove(umCliente);

            if (cliente.Documento != null)
                umCliente.Documento = cliente.Documento;

            if (cliente.Id != null)
                umCliente.Id = cliente.Id;

            if (cliente.Sexo != null)
                umCliente.Sexo = cliente.Sexo;

            if (cliente.Nome != null)
                 umCliente.Nome = cliente.Nome;

            db.Clientes.Add(umCliente);

            Arq.ManipulacaoDeArquivos(false, db);

            return new Retorno() { Status = true, Resultado = umCliente };
        }

    }
}
