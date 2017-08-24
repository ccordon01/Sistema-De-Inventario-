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
    public class DetalleInOutPsController : Controller
    {
        private ProyectoFinalTJEntities1 db = new ProyectoFinalTJEntities1();

        // GET: DetalleInOutPs
        public ActionResult Index()
        {
            var detalleInOutP = db.DetalleInOutP.Include(d => d.Productos).Include(d => d.InOut);
            return View(detalleInOutP.ToList());
        }

        // GET: DetalleInOutPs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DetalleInOutP detalleInOutP = db.DetalleInOutP.Find(id);
            if (detalleInOutP == null)
            {
                return HttpNotFound();
            }
            return View(detalleInOutP);
        }

        // GET: DetalleInOutPs/Create
        public ActionResult Create()
        {
            ViewBag.codigoProducto = new SelectList(db.Productos, "id", "nombreProducto");
            ViewBag.coidgoInOut = new SelectList(db.InOut, "id", "descripcion");
            return View();
        }

        // POST: DetalleInOutPs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,coidgoInOut,codigoProducto,cantidadP")] DetalleInOutP detalleInOutP)
        {
            if (ModelState.IsValid)
            {
                db.DetalleInOutP.Add(detalleInOutP);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.codigoProducto = new SelectList(db.Productos, "id", "nombreProducto", detalleInOutP.codigoProducto);
            ViewBag.coidgoInOut = new SelectList(db.InOut, "id", "descripcion", detalleInOutP.coidgoInOut);
            return View(detalleInOutP);
        }

        // GET: DetalleInOutPs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DetalleInOutP detalleInOutP = db.DetalleInOutP.Find(id);
            if (detalleInOutP == null)
            {
                return HttpNotFound();
            }
            ViewBag.codigoProducto = new SelectList(db.Productos, "id", "nombreProducto", detalleInOutP.codigoProducto);
            ViewBag.coidgoInOut = new SelectList(db.InOut, "id", "descripcion", detalleInOutP.coidgoInOut);
            return View(detalleInOutP);
        }

        // POST: DetalleInOutPs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,coidgoInOut,codigoProducto,cantidadP")] DetalleInOutP detalleInOutP)
        {
            if (ModelState.IsValid)
            {
                db.Entry(detalleInOutP).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.codigoProducto = new SelectList(db.Productos, "id", "nombreProducto", detalleInOutP.codigoProducto);
            ViewBag.coidgoInOut = new SelectList(db.InOut, "id", "descripcion", detalleInOutP.coidgoInOut);
            return View(detalleInOutP);
        }

        // GET: DetalleInOutPs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DetalleInOutP detalleInOutP = db.DetalleInOutP.Find(id);
            if (detalleInOutP == null)
            {
                return HttpNotFound();
            }
            return View(detalleInOutP);
        }

        // POST: DetalleInOutPs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DetalleInOutP detalleInOutP = db.DetalleInOutP.Find(id);
            db.DetalleInOutP.Remove(detalleInOutP);
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
