using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Actividad17.Shared.Models
{
    public class Clientes
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Domicilio { get; set; }
        public string Telefono { get; set; } 
        //vincular todos los servicios a los clientes
        public virtual ICollection<Servicios>? Servicios { get; set; }
    }
}
