using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Proyecto1TJ.Models
{
    public class FacturaDetalle:DetalleFactura
    {
        public string Nombre { get; set; }

        public string Descripcion { get; set; }
    }
}