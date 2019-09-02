namespace Model
{
    public class Produto : Base
    {
        // Model de produto herdando da base
        public string Nome { get; set; }
        public double Preco { get; set; }
        public int Quantidade { get; set; }

        //Método para relizar o calculo parcial
        public double SubTotal() => Preco * Quantidade;

        // método para realizar a troca dos dados de um objeto nulo para um completo
        public void TrocandoDados(Produto produto)
        {
            Nome = produto.Nome;
            Preco = produto.Preco;
        }
    }
}
