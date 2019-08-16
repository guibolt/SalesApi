
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
        //Classe com as regras de negócio e lógica.
        
          //Referencia ao contexto.
        private ComprasContext _contexto { get; set; }
            
        // Construtor contendo o contexto.
        public ProdutoCore(ComprasContext contexto)
        {
            _contexto = contexto;
            _contexto.Produtos = contexto.Set<Produto>();
        }
        //Método para cadastar um protudo.
        public Produto Cadastrar(Produto produto)
        {
            _contexto.Produtos.Add(produto);
            _contexto.SaveChanges();
            return produto;
        }
       // Método para buscar um produto por id
        public Produto AcharId(string id) => _contexto.Produtos.FirstOrDefault(c => c.Id.ToString() == id);
        //Método para listar os protudos.
        public List<Produto>AcharTodos() => _contexto.Produtos.ToList();
        // Método para atualizar os dados de um produto.
        public Produto Atualizar(Produto produto)
        {
            if (!_contexto.Produtos.Any(p => p.Id == produto.Id))
                return null;

            var umProduto = _contexto.Produtos.FirstOrDefault(c => c.Id == produto.Id);

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

        //Método para deletar um deletar um produto.
        public void DeletarUm(string id)
        {
            var umProduto = _contexto.Produtos.FirstOrDefault(c => c.Id.ToString() == id);
            _contexto.Produtos.Remove(umProduto);
            _contexto.SaveChanges();
        }
    }
}
