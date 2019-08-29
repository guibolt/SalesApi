using Core.Util;
using FluentValidation;
using Model;
using System.Linq;

namespace Core
{ // Classe contendo as regras de negocio
    public class ClienteCore : AbstractValidator<Cliente>
    {
        private Cliente _cliente;
        public Sistema db { get; set; }
        // Construtor sem argumento inicando a base dados
        public ClienteCore()
        {
            db = Arq.ManipulacaoDeArquivos(true, null).sistema;

            if (db == null)  db = new Sistema();
        }

        // Construtor com a validação
        public ClienteCore(Cliente Cliente) {

            db = Arq.ManipulacaoDeArquivos(true, null).sistema;

            if (db == null) db = new Sistema();

            _cliente = Cliente;


            db = Arq.ManipulacaoDeArquivos(true, null).sistema;

            if (db == null) db = new Sistema();
        

          _cliente = Cliente;
            RuleFor(c => c.Idade).GreaterThan(6).WithMessage("A idade do cliente deve ser maior que 6");
            RuleFor(c => c.Nome).MinimumLength(3).NotNull().WithMessage("O nome deve conter mais que 2 caracteres");
            RuleFor(c => c.Documento).Length(11, 11).NotNull().WithMessage("Cpf inválido");
            RuleFor(c => c.Sexo.ToUpper()).NotNull().Must(c => c == "MASCULINO"|| c == "FEMININO").WithMessage($"Campo {_cliente.Sexo.GetType()} não pode ser nulo");
            
        }
        // Método para cadastar um cliente
        public Retorno CadastrarCliente()
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
        // Método para retornar um cliente
        public Retorno AcharUm(string id)
        {
            if (!db.Clientes.Exists(e => e.Id.ToString() == id))

                return new Retorno() { Status = false, Resultado = "Registro nao existe na base de dados" };

            return new Retorno() { Status = true, Resultado = db.Clientes.Find(c => c.Id == id) };
        }
        // método para retornar todos os clientes registrados.
        public Retorno AcharTodos() =>  new Retorno() { Status = true, Resultado = db.Clientes.OrderBy(n => n.Nome) };
        public Retorno DeletarId(string id)
        {
            db.Clientes.Remove(db.Clientes.Find(c => c.Id.ToString() == id));

            Arq.ManipulacaoDeArquivos(false, db);

            return new Retorno { Status = true, Resultado = "Registro deletado!" };

        }
        // Método para atualizar por id
        public Retorno AtualizarUm(string id, Cliente cliente)
        {
            var umCliente = db.Clientes.Find(c => c.Id == id);


            if (cliente.Documento != null)
                umCliente.Documento = cliente.Documento;

            if (cliente.Id != null)
                umCliente.Id = cliente.Id;

            if (cliente.Sexo != null)
                umCliente.Sexo = cliente.Sexo;

            if (cliente.Nome != null)
                 umCliente.Nome = cliente.Nome;


            Arq.ManipulacaoDeArquivos(false, db);

            return new Retorno() { Status = true, Resultado = umCliente };
        }
    }
}
