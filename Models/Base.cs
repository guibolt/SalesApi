using System;

namespace Model
{
    public class Base
    {
        public string Nome { get; set; }
        public string Id { get; set; } = Guid.NewGuid().ToString().Substring(0,6);
    }
}
