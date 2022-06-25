using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectModels
{
    public class Persona
    {
        public int ID { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Provincia { get; set; }
        public string Dni { get; set; }
        public string Telefono { get; set; }
        public int Activo { get; set; }
        public string Email { get; set; }
        public string Profiles { get; set; }
        public string Skills { get; set; }
    }
}
