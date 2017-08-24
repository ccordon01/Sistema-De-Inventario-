using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Proyecto1TJ.Models
{
    public class DetalleEntrada
    {

        public int id { get; set; }
        public string nombreProducto { get; set; }
        public decimal costoProducto { get; set; }
        public int unit { get; set; }
        public decimal partial { get { return (unit * costoProducto); } }
    }
}