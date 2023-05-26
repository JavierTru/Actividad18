using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Actividad17.Shared.Models
{
    public class Garantias
    {
        public int Id { get; set; }
        public DateTime FechaG { get; set; }
        public DateTime Vigencia { get; set; }

        public int? ServiciosId { get; set; }
        public virtual Servicios? Servicios { get; set; }
    }
}
