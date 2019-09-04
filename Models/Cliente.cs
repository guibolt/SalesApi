using System;

namespace Model
{
    public class Cliente: Base 
    {
        // Model de cliente herdando da base
        public string Nome { get; set; }
        public int Idade { get; set; }
        public string Cpf { get; set; }
        public string Genero { get; set; }
        public double TotalComprado { get; set; } 

        // método para realizar a troca dos dados de um objeto nulo para um completo
        public void TrocandoDados(Cliente cliente)
        {
            Idade = cliente.Idade;
            Cpf = cliente.Cpf;
            Genero = cliente.Genero;
            Nome = cliente.Nome;
            DataCadastro = cliente.DataCadastro;
        }
    } 
}
