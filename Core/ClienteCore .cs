using ApiForSales;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core
{
    public class ClienteCore
    {
        //Classe com as regras de negócio e lógica.
        //Referencia ao contexto.
        private ComprasContext _contexto { get; set; }

        // Construtor contendo o contexto.
        public ClienteCore(ComprasContext Contexto)
        {
            _contexto = Contexto;
            _contexto.Clientes = _contexto.Set<Cliente>();    
        }
        //Método para cadastar um protudo.
        public Cliente Cadastrar(Cliente cliente)
        {
            _contexto.Clientes.Add(cliente);
            _contexto.SaveChanges();
            return cliente;
        }
        // Método para buscar um produto por id
        public Cliente AcharId(string id)  => _contexto.Clientes.FirstOrDefault(c => c.Id.ToString() == id);
        //Método para listar os protudos.
        public List<Cliente> AcharTodos() => _contexto.Clientes.ToList();
        // Método para atualizar os dados de um produto.
        public Cliente Atualizar( Cliente cliente )
        {
            if (!_contexto.Clientes.Any(c => c.Id == cliente.Id))
                return null;

            var umCliente = _contexto.Clientes.FirstOrDefault(c => c.Id == cliente.Id);

            if (umCliente != null)
            {
                try
                {
                    _contexto.Entry(umCliente).CurrentValues.SetValues(cliente);
                    _contexto.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return umCliente;
        }
        //Método para deletar um deletar um produto.
        public void DeletarUm(string id)
        {
           var umCliente = _contexto.Clientes.FirstOrDefault(c => c.Id.ToString() == id);
            _contexto.Clientes.Remove(umCliente);
            _contexto.SaveChanges();
        }
    }
}
