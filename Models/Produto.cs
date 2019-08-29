namespace Model
{
    public class Produto:Base
    {
      public double Preco { get; set; }
      public int Quantidade { get; set; }

       public double SubTotal() => Preco * Quantidade;
    }
}
