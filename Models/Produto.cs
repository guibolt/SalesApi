using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Models
{
    public class Produto:Base
    {
        // Model para o produto
      public double Preco { get; set; }
      public int Quantidade { get; set; }
    }
}
