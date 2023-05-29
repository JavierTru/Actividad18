using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Actividad17.Shared.Models
{
    public class Servicios
    {
        public int Id { get; set; }

        public string? Nombre { get; set; }
        
        public DateTime Fecha { get; set; }

        public int? ClientesId { get; set; }
        //Vincular con el cliente
        public virtual Clientes? Clientes { get; set; }
        //garantias 
        public virtual ICollection<Garantias>? Garantias { get; set;}
    }
}
