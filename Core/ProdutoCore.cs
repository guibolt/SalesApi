
using ApiForSales;
using Microsoft.EntityFrameworkCore;
using Models;
using System.Linq;
using System;
using System.Collections.Generic;

namespace Core
{
    public class ProdutoCore
    {

        private ComprasContext _contexto { get; set; }
        private DbSet<Produto> Produtos { get; set; }
        public ProdutoCore(ComprasContext contexto)
        {
            _contexto = contexto;
            Produtos = contexto.Set<Produto>();
        }
        public Produto Cadastrar(Produto produto)
        {
            Produtos.Add(produto);
            _contexto.SaveChanges();
            return produto;
        }
       
        public Produto AcharId(string id) => Produtos.FirstOrDefault(c => c.Id.ToString() == id);
        public List<Produto>AcharTodos() => Produtos.ToList();

        public Produto Atualizar(Produto produto)
        {
            var umProduto = Produtos.FirstOrDefault(c => c.Id == produto.Id);

            if (umProduto != null)
            {
                try
                {
                    _contexto.Entry(umProduto).CurrentValues.SetValues(produto);
                    _contexto.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return umProduto;
        }

        public void DeletarUm(string id)
        {
            var umProduto = Produtos.FirstOrDefault(c => c.Id.ToString() == id);
            Produtos.Remove(umProduto);
            _contexto.SaveChanges();
        }


    }
}
