using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Proyecto1TJ.Models
{
    public class FacturaView
    {
        public Factura factEncab { get; set; }
        public FacturaDetalle Titulos { get; set; }

        public List<FacturaDetalle> factList { get; set; }
    }
}