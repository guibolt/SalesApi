﻿using System;

namespace Model
{
   abstract public class Base
    {
        // Model de base com os atributos que serao herdados
        public string Id { get; set; } = Guid.NewGuid().ToString().Substring(0,6);
        public string DataCadastro { get; set; } = DateTime.Now.ToString("dddd/MMMM/yyyy");
    }
}
