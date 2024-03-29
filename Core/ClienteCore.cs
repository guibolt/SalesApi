﻿using Core.Util;
using FluentValidation;
using Model;
using System;
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
            db = db ?? new Sistema();
        }

        // Construtor com a validação
        public ClienteCore(Cliente Cliente)
        {
            db = Arq.ManipulacaoDeArquivos(true, null).sistema;
            db = db ?? new Sistema();

            _cliente = Cliente;
     
            RuleFor(c => c.Idade).GreaterThan(6).WithMessage("A idade do cliente deve ser maior que 6");
            RuleFor(c => c.Nome).MinimumLength(3).NotNull().WithMessage("O nome deve conter mais que 2 caracteres");
            RuleFor(c => c.Cpf).Length(11, 11).NotNull().WithMessage("Cpf inválido");
            RuleFor(c => c.Genero.ToUpper()).NotNull().Must(c => c == "MASCULINO"|| c == "FEMININO").WithMessage($"Campo sexo não pode ser nulo");
        }
        /// <summary>
        /// Método para cadastar um cliente
        /// </summary>
        public Retorno CadastrarCliente()
        {
            var valida = Validate(_cliente);

            if (!valida.IsValid)
                return new Retorno { Status = false, Resultado = valida.Errors.Select(c => c.ErrorMessage).ToList() };

            if (db.Clientes.Any(c => c.Nome == _cliente.Nome) || db.Clientes.Any(c => c.Cpf == _cliente.Cpf))
                return new Retorno { Status = false, Resultado = "Cliente já registrado" };

            db.Clientes.Add(_cliente);
            Arq.ManipulacaoDeArquivos(false, db);

            return new Retorno { Status = true, Resultado = _cliente };
        }

        /// <summary>
        /// Método para buscar um cliente baseado no Id fornecido.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Retorno BuscarCliente(string id)
        {
            var umCliente = db.Clientes.FirstOrDefault(c => c.Id == id);
            return umCliente == null ? new Retorno { Status = false, Resultado = "Cliente nao existe na base de dados" } : new Retorno { Status = true, Resultado = umCliente };
        }

        /// <summary>
        /// método para retornar todos os clientes registrados na base de dados.
        /// </summary>
        /// <returns></returns>
        public Retorno BuscarTodosClientes()
        {
            var todosClientes = db.Clientes;
            return todosClientes.Count == 0 ? new Retorno { Status = false, Resultado = "Não exitem registros na base" } : new Retorno { Status = true, Resultado = db.Clientes.OrderBy(n => n.Nome) };
        }
        
        /// <summary>
        /// Método para deletar um cliente por id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Retorno DeletarCliente(string id)
        {
            var umCliente = db.Clientes.FirstOrDefault(c => c.Id == id);
            if (umCliente == null)
                return new Retorno() { Status = false, Resultado = "Registro nao existe na base de dados" };

            db.Clientes.Remove(umCliente);
            Arq.ManipulacaoDeArquivos(false, db);

            return new Retorno { Status = true, Resultado = "Registro deletado!" };
        }

        /// <summary>
        /// Método para atualizar os dados de um cliente baseado no id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cliente"></param>
        /// <returns></returns>
        // Método para atualizar por id
        public Retorno AtualizarCliente(string id, Cliente cliente)
        {
            var umCliente = db.Clientes.FirstOrDefault(c => c.Id == id);
            if (umCliente == null) return new Retorno() { Status = false, Resultado = "Registro nao existe na base de dados" };

            if (cliente.Cpf != null)
                umCliente.Cpf = cliente.Cpf;

            if (cliente.Genero != null)
                umCliente.Genero = cliente.Genero;

            if (cliente.Nome != null)
                 umCliente.Nome = cliente.Nome;

            if (cliente.Idade != 0)
                umCliente.Idade = cliente.Idade;

            Arq.ManipulacaoDeArquivos(false, db);
            return new Retorno { Status = true, Resultado = umCliente };
        }
        /// <summary>
        /// Método para realizar a busca por data.
        /// </summary>
        /// <param name="dataComeço"></param>
        /// <param name="dataFim"></param>
        /// <returns></returns>
        public Retorno BuscaClientePorData(string dataComeço, string dataFim)
        {
            // Tento fazer a conversao e checho se ela nao for feita corretamente, se ambas nao forem corretas retorno FALSE
            if (!DateTime.TryParse(dataComeço, out DateTime primeiraData) && !DateTime.TryParse(dataFim, out DateTime segundaData))
                return new Retorno() { Status = false, Resultado = "Dados Invalidos" };

            // Tento fazer a conversao da segunda data for invalida faço somente a pesquisa da primeira data
            if (!DateTime.TryParse(dataFim, out segundaData))
                return new Retorno { Status = true, Resultado = db.Clientes.Where(c => Convert.ToDateTime(c.DataCadastro) >= primeiraData).ToList() };

            // Tento fazer a conversao da primeiradata for invalida faço somente a pesquisa da segunda data
            if (!DateTime.TryParse(dataComeço, out primeiraData))
                return new Retorno { Status = true, Resultado = db.Clientes.Where(c => Convert.ToDateTime(c.DataCadastro) <= segundaData).ToList() };

            // returno a lista completa entre as duas datas informadas.
            return new Retorno { Status = true, Resultado = db.Clientes.Where(c => Convert.ToDateTime(c.DataCadastro) >= primeiraData && Convert.ToDateTime(c.DataCadastro) <= segundaData).ToList() };
        }

        /// <summary>
        ///  Método para exibir os registros por paginação
        /// </summary>
        /// <param name="ordempor"></param>
        /// <param name="numeroPagina"></param>
        /// <param name="qtdRegistros"></param>
        /// <returns></returns>
        public Retorno ClentesPorPaginacao(string ordempor, int numeroPagina, int qtdRegistros)
        {
            // Limitando a quantidade registros para a paginação
            if (qtdRegistros > 50) qtdRegistros = 50;

            // checo se as paginação é valida pelas variaveis e se sim executo o skip take contendo o calculo
            if (numeroPagina > 0 && qtdRegistros > 0 && ordempor == null)
                return new Retorno() { Status = true, Resultado = db.Clientes.Skip((numeroPagina - 1) * qtdRegistros).Take(qtdRegistros).ToList() };

            // faço a verificação e depois ordeno por nome. 
            if (numeroPagina > 0 && qtdRegistros > 0 && ordempor.ToUpper().Trim() == "NOME")
                return new Retorno() { Status = true, Resultado = db.Clientes.OrderBy(c => c.Nome).Skip((numeroPagina - 1) * qtdRegistros).Take(qtdRegistros).ToList() };

            // faço a verificação e depois ordeno por idade. 
            if (numeroPagina > 0 && qtdRegistros > 0 && ordempor.ToUpper().Trim() == "IDADE")
                return new Retorno() { Status = true, Resultado = db.Clientes.OrderBy(c => c.Idade).Skip((numeroPagina - 1) * qtdRegistros).Take(qtdRegistros).ToList() };

            // se nao der pra fazer a paginação
            return db.Clientes.Count == 0 ? new Retorno { Status = false, Resultado = "Não tem nenhum item na Lista" } :
                  new Retorno { Status = true, Resultado = ($"Não foi fazer a paginação, registros totais: {db.Clientes.Count()}, Exibindo a lista padrão:", db.Clientes.Take(5).ToList()) };
        }
    }
}
