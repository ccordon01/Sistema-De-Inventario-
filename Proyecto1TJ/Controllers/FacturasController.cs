using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Proyecto1TJ.Models;

namespace Proyecto1TJ.Controllers
{
    public class FacturasController : Controller
    {
        private ProyectoFinalTJEntities1 db = new ProyectoFinalTJEntities1();

        // GET: Facturas
        public ActionResult Index()
        {
            var factura = db.Factura.Include(f => f.Clientes).Include(f => f.estadoFacturas).Include(f => f.Usuarios);
            return View(factura.ToList());
        }

        // GET: Facturas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Factura factura = db.Factura.Find(id);
            if (factura == null)
            {
                return HttpNotFound();
            }
            return View(factura);
        }

        // GET: Facturas/Create
        public ActionResult Create()
        {
            ViewBag.cliente = new SelectList(db.Clientes, "id", "nombrePersona");
            ViewBag.estadoFactura = new SelectList(db.estadoFacturas, "id", "nombreEstado");
            ViewBag.vendedor = new SelectList(db.Usuarios, "id", "nombrePersona");
            ViewBag.fechaI = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss");
            if (Session["FacturaI"]==null)
            {
                ViewBag.totalI = 0;
                FacturaView fv = new FacturaView();
                fv.factEncab = new Factura();
                fv.Titulos = new FacturaDetalle();
                fv.factList = new List<FacturaDetalle>();
                Session["FacturaI"] = fv;
                return View(fv);
            }
            decimal c = 0;
            var ff = Session["FacturaI"] as FacturaView;
            foreach (var item in ff.factList)
            {
                c += decimal.Parse(item.precio.ToString());
            }
            ViewBag.totalI = c;
            return View(Session["FacturaI"] as FacturaView);
        }

        public ActionResult CancelarFact() {
            ViewBag.cliente = new SelectList(db.Clientes, "id", "nombrePersona");
            ViewBag.estadoFactura = new SelectList(db.estadoFacturas, "id", "nombreEstado");
            ViewBag.vendedor = new SelectList(db.Usuarios, "id", "nombrePersona");
            ViewBag.fechaI = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss");
            ViewBag.totalI = 0;
            FacturaView fv = new FacturaView();
            fv.factEncab = new Factura();
            fv.Titulos = new FacturaDetalle();
            fv.factList = new List<FacturaDetalle>();
            Session["FacturaI"] = fv;
            Session["ProductI"] = new List<Inventario>();
            return RedirectToAction("Create");
        }
        [HttpPost]
        public ActionResult Create(FacturaView fvv)
        {

            decimal c = 0;
            var ff = Session["FacturaI"] as FacturaView;
            fvv.factList = ff.factList;
            foreach (var item in ff.factList)
            {
                c += decimal.Parse(item.precio.ToString());
            }
            Factura insF = fvv.factEncab;
            var clientes = db.Clientes.ToList();
            foreach (var item in clientes)
            {
                if (item.nit.Equals(insF.nitFactura))
                {
                    insF.cliente = item.id;
                    insF.nombreFactura = item.nombrePersona + " " + item.apellidoPersona;
                    break;
                }
            }
            insF.estadoFactura = db.estadoFacturas.ToList().First().id;
            insF.vendedor = int.Parse(Session["UserID"].ToString());
            insF.totalFactura = c;
            insF.fechaFactura= DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
            db.Factura.Add(insF);
            db.SaveChanges();
            insF = db.Factura.ToList().Last();
            foreach (var item in fvv.factList)
            {
                DetalleFactura df = new DetalleFactura();
                df.codigoFactura = insF.id;
                df.codigoInventario = item.codigoInventario;
                df.precio = item.precio;
                db.DetalleFactura.Add(df);
                db.SaveChanges();
            }
            InOut io = new InOut();
            io.descripcion = "Salida por factura generada";
            io.fechaInOut = insF.fechaFactura;
            io.tipoInOut = 2;
            io.cantidadInOut = fvv.factList.Count;
            int eje = int.Parse(Session["UserID"].ToString());
            int ub = int.Parse((from q in db.EmployeeBodega
                                where q.idUsuario == eje
                                select q).ToList().First().idBodega.ToString());
            io.ubicacionInOut = ub;
            db.InOut.Add(io);
            db.SaveChanges();
            io = db.InOut.ToList().Last();
            List<DetalleInOutP> dip = new List<DetalleInOutP>();
            foreach (var item in fvv.factList)
            {
                bool creador = true;
                int proc = int.Parse((from q in db.Inventario
                                      where q.id == item.codigoInventario
                                      select q).ToList().First().codigoProducto.ToString());
                foreach (var item1 in dip)
                {
                    if (item1.codigoProducto==proc)
                    {
                        item1.cantidadP++;
                        creador = false;
                    }
                }
                if (creador)
                {
                    DetalleInOutP dop = new DetalleInOutP();
                    dop.inoutType = false;
                    dop.codigoProducto = proc;
                    dop.cantidadP = 1;
                    dop.coidgoInOut = io.id;
                    dip.Add(dop);
                }
            }
            foreach (var item in dip)
            {
                db.DetalleInOutP.Add(item);
                db.SaveChanges();
                var ite = db.DetalleInOutP.ToList().Last();
                foreach (var item1 in fvv.factList)
                {
                    int proc = int.Parse((from q in db.Inventario
                                          where q.id == item1.codigoInventario
                                          select q).ToList().First().codigoProducto.ToString());
                    if (proc==ite.codigoProducto)
                    {
                        DetalleInOut dit = new DetalleInOut();
                        dit.coidgoInOut = ite.id;
                        dit.codigoProducto = item1.codigoInventario;
                        dit.cantidad = 1;
                        db.DetalleInOut.Add(dit);
                        db.SaveChanges();
                    }
                }
            }
            foreach (var item in fvv.factList)
            {
                db.inveVendido(item.codigoInventario);
            }
            ViewBag.cliente = new SelectList(db.Clientes, "id", "nombrePersona");
            ViewBag.estadoFactura = new SelectList(db.estadoFacturas, "id", "nombreEstado");
            ViewBag.vendedor = new SelectList(db.Usuarios, "id", "nombrePersona");
            ViewBag.fechaI = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss");
            ViewBag.totalI = 0;
            FacturaView fv = new FacturaView();
            fv.factEncab = new Factura();
            fv.Titulos = new FacturaDetalle();
            fv.factList = new List<FacturaDetalle>();
            Session["FacturaI"] = fv;
            Session["ProductI"] = new List<Inventario>();
            return RedirectToAction("Create");
        }

        public ActionResult AddProduct() {
            int eje = int.Parse(Session["UserID"].ToString());
            int ub = int.Parse((from q in db.EmployeeBodega
                      where q.idUsuario == eje
                      select q).ToList().First().idBodega.ToString());
            var inventario = (from q in db.Inventario
                              where q.estadoProducto == 1 && q.ubicacionProducto == ub
                              select q);
             List < Inventario > ListIn = inventario.ToList();
            if (Session["ProductI"] != null)
            {
                foreach (Inventario item in Session["ProductI"] as List<Inventario>)
                {
                    int cont = 0;
                    foreach (var item2 in ListIn)
                    {
                        if (item2.id==item.id)
                        {
                            break;
                        }
                        cont++;
                    }
                    ListIn.RemoveAt(cont);
                }
            }
            else {
                Session["ProductI"] = new List<Inventario>();
            }
            return View(ListIn);
        }
        public ActionResult ComprarProduct(int? id) {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inventario inventario = db.Inventario.Find(id);
            if (inventario == null)
            {
                return HttpNotFound();
            }
            var pi = Session["ProductI"] as List<Inventario>;
            pi.Add(inventario);
            var fv = Session["FacturaI"] as FacturaView;
            FacturaDetalle fd = new FacturaDetalle();
            Productos p = db.Productos.Find(inventario.codigoProducto);
            fd.Descripcion = p.descripcionProducto;
            fd.codigoInventario = inventario.id;
            fd.Nombre = p.nombreProducto;
            fd.precio = inventario.precioVenta;
            fv.factList.Add(fd);
            return RedirectToAction("AddProduct");
        }
        // POST: Facturas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "id,numeroSerie,numeroFactura,nitFactura,nombreFactura,direccionFacutra,fechaFactura,estadoFactura,totalFactura,cliente,vendedor")] Factura factura)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Factura.Add(factura);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.cliente = new SelectList(db.Clientes, "id", "nombrePersona", factura.cliente);
        //    ViewBag.estadoFactura = new SelectList(db.estadoFacturas, "id", "nombreEstado", factura.estadoFactura);
        //    ViewBag.vendedor = new SelectList(db.Usuarios, "id", "nombrePersona", factura.vendedor);
        //    return View(factura);
        //}

        // GET: Facturas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Factura factura = db.Factura.Find(id);
            if (factura == null)
            {
                return HttpNotFound();
            }
            ViewBag.cliente = new SelectList(db.Clientes, "id", "nombrePersona", factura.cliente);
            ViewBag.estadoFactura = new SelectList(db.estadoFacturas, "id", "nombreEstado", factura.estadoFactura);
            ViewBag.vendedor = new SelectList(db.Usuarios, "id", "nombrePersona", factura.vendedor);
            return View(factura);
        }

        // POST: Facturas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,numeroSerie,numeroFactura,nitFactura,nombreFactura,direccionFacutra,fechaFactura,estadoFactura,totalFactura,cliente,vendedor")] Factura factura)
        {
            if (ModelState.IsValid)
            {
                db.Entry(factura).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.cliente = new SelectList(db.Clientes, "id", "nombrePersona", factura.cliente);
            ViewBag.estadoFactura = new SelectList(db.estadoFacturas, "id", "nombreEstado", factura.estadoFactura);
            ViewBag.vendedor = new SelectList(db.Usuarios, "id", "nombrePersona", factura.vendedor);
            return View(factura);
        }

        // GET: Facturas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Factura factura = db.Factura.Find(id);
            if (factura == null)
            {
                return HttpNotFound();
            }
            return View(factura);
        }

        // POST: Facturas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Factura factura = db.Factura.Find(id);
            db.Factura.Remove(factura);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
