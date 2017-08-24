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
    public class InventariosController : Controller
    {
        private ProyectoFinalTJEntities1 db = new ProyectoFinalTJEntities1();

        // GET: Inventarios
        public ActionResult Index()
        {
            var inventario = db.Inventario.Include(i => i.DataBodega).Include(i => i.estadoProductos).Include(i => i.Productos);
            return View(inventario.ToList());
        }

        // GET: Inventarios/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inventario inventario = db.Inventario.Find(id);
            if (inventario == null)
            {
                return HttpNotFound();
            }
            return View(inventario);
        }


        // GET: Inventarios/Create
        public ActionResult Create()
        {
            ViewBag.ubicacionProducto = new SelectList(db.DataBodega, "id", "nombreBodega");
            ViewBag.estadoProducto = new SelectList(db.estadoProductos, "id", "nombreEstado");
            ViewBag.codigoProducto = new SelectList(db.Productos, "id", "nombreProducto");
            return View();
        }

        // POST: Inventarios/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,codigoProducto,estadoProducto,ubicacionProducto,precioVenta")] Inventario inventario)
        {
            if (ModelState.IsValid)
            {
                db.Inventario.Add(inventario);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ubicacionProducto = new SelectList(db.DataBodega, "id", "nombreBodega", inventario.ubicacionProducto);
            ViewBag.estadoProducto = new SelectList(db.estadoProductos, "id", "nombreEstado", inventario.estadoProducto);
            ViewBag.codigoProducto = new SelectList(db.Productos, "id", "nombreProducto", inventario.codigoProducto);
            return View(inventario);
        }

        // GET: Inventarios/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inventario inventario = db.Inventario.Find(id);
            if (inventario == null)
            {
                return HttpNotFound();
            }
            ViewBag.ubicacionProducto = new SelectList(db.DataBodega, "id", "nombreBodega", inventario.ubicacionProducto);
            ViewBag.estadoProducto = new SelectList(db.estadoProductos, "id", "nombreEstado", inventario.estadoProducto);
            ViewBag.codigoProducto = new SelectList(db.Productos, "id", "nombreProducto", inventario.codigoProducto);
            return View(inventario);
        }

        // POST: Inventarios/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,codigoProducto,estadoProducto,ubicacionProducto,precioVenta")] Inventario inventario)
        {
            if (ModelState.IsValid)
            {
                db.Entry(inventario).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ubicacionProducto = new SelectList(db.DataBodega, "id", "nombreBodega", inventario.ubicacionProducto);
            ViewBag.estadoProducto = new SelectList(db.estadoProductos, "id", "nombreEstado", inventario.estadoProducto);
            ViewBag.codigoProducto = new SelectList(db.Productos, "id", "nombreProducto", inventario.codigoProducto);
            return View(inventario);
        }

        // GET: Inventarios/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inventario inventario = db.Inventario.Find(id);
            if (inventario == null)
            {
                return HttpNotFound();
            }
            return View(inventario);
        }

        // POST: Inventarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Inventario inventario = db.Inventario.Find(id);
            db.Inventario.Remove(inventario);
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
