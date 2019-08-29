namespace Model
{
    public class Produto:Base
    { 
        // Model de produto herdando da base
      public double Preco { get; set; }
      public int Quantidade { get; set; }

       public double SubTotal() => Preco * Quantidade;
    }
}
