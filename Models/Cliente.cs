using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class Cliente: Base 
    {
        public int Idade { get; set; }
        public string Documento { get; set; }
        public string Sexo { get; set; }
    } 
}
