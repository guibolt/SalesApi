using System;

namespace Models
{
    public class Base
    {
        // Model de base 
        public string Nome { get; set; }
        public string Id = Guid.NewGuid().ToString().Substring(0,6);
    }
}
