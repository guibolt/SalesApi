﻿using System;
using System.Collections.Generic;

namespace Model
{
    // Model de pedido
    public class Pedido : Base
    {
        public List<Produto> Produtos { get; set; } = new List<Produto>();
        public Cliente Cliente { get; set; }
        public double ValorTotal { get; set; }
   
        // Método para realizar o calcuto atribuir o valor ao pedido.
        public void CalculaTotal()
        {
            Produtos.ForEach(c => ValorTotal += c.SubTotal());
            if (ValorTotal > 300)
                ValorTotal -= ValorTotal * 0.10;
            else if (ValorTotal > 100)
                ValorTotal -= ValorTotal * 0.05;
        }
    }
}
