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
    public class DataBodegasController : Controller
    {
        private ProyectoFinalTJEntities1 db = new ProyectoFinalTJEntities1();

        // GET: DataBodegas
        public ActionResult Index()
        {
            var dataBodega = db.DataBodega.Include(d => d.Usuarios);
            return View(dataBodega.ToList());
        }

        // GET: DataBodegas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DataBodega dataBodega = db.DataBodega.Find(id);
            if (dataBodega == null)
            {
                return HttpNotFound();
            }
            return View(dataBodega);
        }

        // GET: DataBodegas/Create
        public ActionResult Create()
        {
            //        SELECT a.id,a.nombrePersona from Usuarios as a left join DataBodega as d 
            //            on a.id = d.adminBodega inner join AspNetRoles as o
            //on a.rol = o.Id where d.adminBodega is null and o.Name = 'Manager Bodega'

            //ViewBag.adminBodega = new SelectList((from a in db.Usuarios
            //                                      join d in db.DataBodega on
            //                a.id equals d.adminBodega
            //                                      join o in db.AspNetRoles on
            //            a.rol equals o.Id
            //                                      where d.adminBodega == null && o.Name == "Manager Bodega"
            //                                      select a).ToList(), "id", "nombrePersona");
            ViewBag.adminBodega = new SelectList(db.ManagerBodegaUser(), "id", "nombrePersona");
            return View();
        }

        // POST: DataBodegas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,nombreBodega,direccionBodega,adminBodega")] DataBodega dataBodega)
        {
            if (ModelState.IsValid)
            {
                db.DataBodega.Add(dataBodega);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.adminBodega = new SelectList(db.ManagerBodegaUser(), "id", "nombrePersona");
            return View(dataBodega);
        }

        // GET: DataBodegas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DataBodega dataBodega = db.DataBodega.Find(id);
            if (dataBodega == null)
            {
                return HttpNotFound();
            }
            ViewBag.adminBodega = new SelectList(db.ManagerBodegaUser(), "id", "nombrePersona");
            return View(dataBodega);
        }

        // POST: DataBodegas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,nombreBodega,direccionBodega,adminBodega")] DataBodega dataBodega)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dataBodega).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.adminBodega = new SelectList(db.ManagerBodegaUser(), "id", "nombrePersona");
            return View(dataBodega);
        }

        // GET: DataBodegas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DataBodega dataBodega = db.DataBodega.Find(id);
            if (dataBodega == null)
            {
                return HttpNotFound();
            }
            return View(dataBodega);
        }

        // POST: DataBodegas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DataBodega dataBodega = db.DataBodega.Find(id);
            db.DataBodega.Remove(dataBodega);
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
