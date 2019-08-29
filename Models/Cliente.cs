namespace Model
{
    public class Cliente: Base 
    {
        // Model de cliente herdando da base
        public int Idade { get; set; }
        public string Documento { get; set; }
        public string Sexo { get; set; }
    } 
}
