using System;

namespace Models
{
    public class Base
    {
        public string Nome { get; set; }
        public string Id = Guid.NewGuid().ToString().Substring(0,6);
    }
}
