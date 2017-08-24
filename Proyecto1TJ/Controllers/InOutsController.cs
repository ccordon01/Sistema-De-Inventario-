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
    public class InOutsController : Controller
    {
        private ProyectoFinalTJEntities1 db = new ProyectoFinalTJEntities1();


        [Authorize(Roles = "Admin, Manager Bodega")]
        // GET: InOuts
        public ActionResult Index()
        {
            var inOut = db.InOut.Include(i => i.DataBodega).Include(i => i.tipoInOut1);
            return View(inOut.ToList());
        }

        // GET: InOuts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InOut inOut = db.InOut.Find(id);
            if (inOut == null)
            {
                return HttpNotFound();
            }
            return View(inOut);
        }
        [HttpGet]
        public ActionResult AddProduct()
        {
            ViewBag.codigoProducto = new SelectList(db.Productos, "id", "nombreProducto");
            return View();
        }
        [Authorize(Roles = "Admin, Manager Bodega")]
        [HttpPost]
        public ActionResult AddProduct(DetalleEntrada detp)
        {
            var eM = Session["entradaM"] as MasterEntrada;
            var productID = int.Parse(Request["codigoProducto"]);
            var product = db.Productos.Find(productID);
            detp = new DetalleEntrada()
            {
                unit = int.Parse(Request["unit"]),
                id = product.id,
                //costoProducto = product.costoProducto,
                nombreProducto = product.nombreProducto
                //unitprice = decimal.Parse(product.costoProducto.ToString())
            };
            eM.varDetalleInOutP.Add(detp);
            ViewBag.codigoProducto = new SelectList(db.Productos, "id", "nombreProducto");
            ViewBag.ubicacionInOut = new SelectList(db.DataBodega, "id", "nombreBodega");
            ViewBag.tipoInOut = new SelectList(db.tipoInOut, "id", "nombretipoInOut");
            ViewBag.fechaI = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss");
            return View("Create",eM);
        }
        [Authorize(Roles = "Admin, Manager Bodega")]
        [HttpPost]
        public ActionResult AddProductToCreate()
        {
            var eM = Session["entradaM"] as MasterEntrada;
            var productID = int.Parse(Request["codigoProducto"]);
            var product = db.Productos.Find(productID);
            DetalleEntrada detp = new DetalleEntrada()
            {
                unit = int.Parse(Request["unit"]),
                id = product.id,
                costoProducto = decimal.Parse(product.costoProducto.ToString()),
                nombreProducto = product.nombreProducto
                //unitprice = decimal.Parse(product.costoProducto.ToString())
            };
            eM.varDetalleInOutP.Add(detp);
            var eL = Session["emlist"] as List<DetalleEntrada>;
            eL.Add(detp);
            Session["emlist"] = eL;
            ViewBag.codigoProducto = new SelectList(db.Productos, "id", "nombreProducto");
            ViewBag.ubicacionInOut = new SelectList(db.DataBodega, "id", "nombreBodega");
            ViewBag.tipoInOut = new SelectList(db.tipoInOut, "id", "nombretipoInOut");
            ViewBag.fechaI = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss");
            ViewBag.de = Session["emlist"];
            int a = 0;
            foreach (var item in eM.varDetalleInOutP)
            {
                a += item.unit;
            }
            ViewBag.cant = a;
            return RedirectToAction("Create");
        }
        // GET: InOuts/Create
        [Authorize(Roles = "Admin, Manager Bodega")]
        public ActionResult Create()
        {
            ViewBag.fechaI = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss");
            ViewBag.ubicacionInOut = new SelectList(db.DataBodega, "id", "nombreBodega");
            ViewBag.tipoInOut = new SelectList(db.tipoInOut, "id", "nombretipoInOut");
            if (Session["entradaM"]==null)
            {
                ViewBag.cant = 0;
                Session["emlist"] = new List<DetalleEntrada>();
                MasterEntrada entradaM = new MasterEntrada();
                entradaM.varDetalleInOutP = new List<DetalleEntrada>();
                entradaM.varTitulosDetalleInOutP = new DetalleEntrada();
                entradaM.varInOut = new InOut();
                Session["entradaM"] = entradaM;
                ViewBag.de = Session["emlist"];
                return View(entradaM);
            }
            var eM = Session["entradaM"] as MasterEntrada;
            int a = 0;
            foreach (var item in eM.varDetalleInOutP)
            {
                a += item.unit;
            }
            ViewBag.cant = a;
            return View(eM);
        }
        [Authorize(Roles = "Admin, Manager Bodega")]
        [HttpPost]
        public ActionResult Create(MasterEntrada add)
        {
            MasterEntrada me = Session["entradaM"] as MasterEntrada;
            int a = 0;
            foreach (var item in me.varDetalleInOutP)
            {
                a += item.unit;
            }
            int ubi = int.Parse(Request["ubicacionInOut"]);
            string desc = add.varInOut.descripcion;
            DateTime fe = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
            InOut io = new InOut();
            io.cantidadInOut = a;
            io.ubicacionInOut = ubi;
            io.tipoInOut = 1;
            io.fechaInOut = fe;
            io.descripcion = desc;
            db.InOut.Add(io);
            db.SaveChanges();
            int ioid = db.InOut.ToList().Last().id;
            foreach (var item in me.varDetalleInOutP)
            {
                DetalleInOutP dp = new DetalleInOutP();
                dp.coidgoInOut = ioid;
                dp.codigoProducto = item.id;
                dp.cantidadP = item.unit;
                dp.inoutType = true;
                db.DetalleInOutP.Add(dp);
                db.SaveChanges();
                int dpid = db.DetalleInOutP.ToList().Last().id;
                for (int i = 0; i < item.unit; i++)
                {
                    Inventario inv = new Inventario();
                    inv.precioVenta = item.costoProducto;
                    inv.codigoProducto = item.id;
                    inv.ubicacionProducto = ubi;
                    inv.estadoProducto = 1;
                    db.Inventario.Add(inv);
                    db.SaveChanges();
                    int invid = db.Inventario.ToList().Last().id;
                    DetalleInOut dino = new DetalleInOut();
                    dino.codigoProducto = invid;
                    dino.cantidad = 1;
                    dino.coidgoInOut = dpid;
                    db.DetalleInOut.Add(dino);
                    db.SaveChanges();
                }
            }
            ViewBag.fechaI = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss");
            ViewBag.ubicacionInOut = new SelectList(db.DataBodega, "id", "nombreBodega");
            ViewBag.tipoInOut = new SelectList(db.tipoInOut, "id", "nombretipoInOut");
            ViewBag.cant = 0;
            Session["emlist"] = new List<DetalleEntrada>();
            MasterEntrada entradaM = new MasterEntrada();
            entradaM.varDetalleInOutP = new List<DetalleEntrada>();
            entradaM.varTitulosDetalleInOutP = new DetalleEntrada();
            entradaM.varInOut = new InOut();
            Session["entradaM"] = entradaM;
            ViewBag.de = Session["emlist"];
            return RedirectToAction("Create", entradaM);
        }
        // POST: InOuts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "id,descripcion,fechaInOut,tipoInOut,cantidadInOut,ubicacionInOut")] InOut inOut)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.InOut.Add(inOut);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.ubicacionInOut = new SelectList(db.DataBodega, "id", "nombreBodega", inOut.ubicacionInOut);
        //    ViewBag.tipoInOut = new SelectList(db.tipoInOut, "id", "nombretipoInOut", inOut.tipoInOut);
        //    return View(inOut);
        //}

        // GET: InOuts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InOut inOut = db.InOut.Find(id);
            if (inOut == null)
            {
                return HttpNotFound();
            }
            ViewBag.ubicacionInOut = new SelectList(db.DataBodega, "id", "nombreBodega", inOut.ubicacionInOut);
            ViewBag.tipoInOut = new SelectList(db.tipoInOut, "id", "nombretipoInOut", inOut.tipoInOut);
            return View(inOut);
        }

        // POST: InOuts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,descripcion,fechaInOut,tipoInOut,cantidadInOut,ubicacionInOut")] InOut inOut)
        {
            if (ModelState.IsValid)
            {
                db.Entry(inOut).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ubicacionInOut = new SelectList(db.DataBodega, "id", "nombreBodega", inOut.ubicacionInOut);
            ViewBag.tipoInOut = new SelectList(db.tipoInOut, "id", "nombretipoInOut", inOut.tipoInOut);
            return View(inOut);
        }

        // GET: InOuts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InOut inOut = db.InOut.Find(id);
            if (inOut == null)
            {
                return HttpNotFound();
            }
            return View(inOut);
        }

        // POST: InOuts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            InOut inOut = db.InOut.Find(id);
            db.InOut.Remove(inOut);
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
