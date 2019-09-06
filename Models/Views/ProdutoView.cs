using Model.Enum;

namespace Model
{
    // Model de produto herdando da base
    public class ProdutoView
    {
        public string Nome { get; set; }
        public double Preco { get; set; }
        public Categorias Categoria { get; set; }
        public int Quantidade { get; set; }
    }
}
