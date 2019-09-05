namespace Model
{
    // Model de produto herdando da base
    public class Produto : Base
    {
        public string Nome { get; set; }
        public double Preco { get; set; }
        public Categorias Categoria { get; set; }
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
