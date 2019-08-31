using System;

namespace Model
{
    public class Cliente: Base 
    {
        // Model de cliente herdando da base
        public int Idade { get; set; }
        public string Documento { get; set; }
        public string Sexo { get; set; }
        public DateTime DataCadastro { get; set; } = DateTime.Now;
        public double TotalComprado { get; set; } 

        // método para realizar a troca dos dados de um objeto nulo para um completo
        public void TrocandoDados(Cliente cliente)
        {
            Idade = cliente.Idade;
            Documento = cliente.Documento;
            Sexo = cliente.Sexo;
            Nome = cliente.Sexo;
            DataCadastro = cliente.DataCadastro;
        }
    } 
}
