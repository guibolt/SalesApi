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
        private ComprasContext _contexto { get; set; }
        public DbSet<Cliente> Clientes { get; set; }

        public ClienteCore(ComprasContext Contexto)
        {
            _contexto = Contexto;
            Clientes = _contexto.Set<Cliente>();    
        }
        public Cliente Cadastrar(Cliente cliente)
        {
            Clientes.Add(cliente);
            _contexto.SaveChanges();
            return cliente;
        }

        public Cliente AcharId(string id)  => Clientes.FirstOrDefault(c => c.Id.ToString() == id);

        public List<Cliente> AcharTodos() => Clientes.ToList();

        public Cliente Atualizar( Cliente cliente )
        {
            var umCliente = Clientes.FirstOrDefault(c => c.Id == cliente.Id);

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

        public void DeletarUm(string id)
        {
           var umCliente = Clientes.FirstOrDefault(c => c.Id.ToString() == id);
            Clientes.Remove(umCliente);
            _contexto.SaveChanges();
        }
    }
}
