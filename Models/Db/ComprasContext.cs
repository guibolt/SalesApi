using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiForSales
{
    public class ComprasContext:DbContext
    {
        // Classe para a conexao com o entity
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
     
        public ComprasContext(DbContextOptions<ComprasContext> options)
            : base(options)
        {
        }

    }
}
