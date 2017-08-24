using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Proyecto1TJ.Models
{
    public class MasterEntrada
    {
        public InOut varInOut { get; set; }
        public DetalleEntrada varTitulosDetalleInOutP { get; set; }

        public List<DetalleEntrada> varDetalleInOutP { get; set;}
    }
}