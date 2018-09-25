using System;
using System.Collections.Generic;
using System.Text;

namespace AppOferta.Models
{
    class Persona
    {
        //public string id   { get; set; }
        public string nombre { get; set; }
        public string apellidos { get; set; }
        public string correo { get; set; }
        public string contrasena { get; set; }
        public string telefono { get; set; }
        public string ciudad { get; set; }
        public string genero { get; set; }
        public string rol { get; set; }
        public string estado { get; set; }
        public string token { get; set; }

        public Persona() { }

        public override string ToString()
        {
            string persona =
                this.nombre + this.apellidos + this.correo + this.contrasena + this.telefono + this.genero + this.rol + this.estado + this.token;
            return persona;
        }
    }
}
