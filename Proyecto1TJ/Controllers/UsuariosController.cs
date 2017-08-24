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
    public class UsuariosController : Controller
    {
        private ProyectoFinalTJEntities1 db = new ProyectoFinalTJEntities1();

        // GET: Usuarios
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var usuarios = db.Usuarios.Include(u => u.AspNetRoles).Include(u => u.AspNetUsers).Include(u => u.Usuarios2);
            return View(usuarios.ToList());
        }
        [Authorize(Roles = "Admin,Manager Bodega")]
        public ActionResult IndexEmployee()
        {
            //var usuarios = db.Usuarios.Include(u => u.AspNetRoles).Include(u => u.AspNetUsers).Include(u => u.Usuarios2);
            return View(db.GetEmpList(int.Parse(Session["UserID"].ToString())));
        }
        // GET: Usuarios/Details/5
        [Authorize(Roles = "Admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuarios usuarios = db.Usuarios.Find(id);
            if (usuarios == null)
            {
                return HttpNotFound();
            }
            return View(usuarios);
        }

        // GET: Usuarios/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            ViewBag.rol = new SelectList(db.AspNetRoles, "Id", "Name");
            ViewBag.idAspNetUsers = new SelectList(db.AspNetUsers, "Id", "Email");
            ViewBag.superior = new SelectList(db.Usuarios, "id", "nombrePersona");
            return View();
        }

        // POST: Usuarios/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "id,nombrePersona,apellidoPersona,nombreUsuario,contraUsuario,edad,numeroTelefono,email,rol,superior,sexo,idAspNetUsers")] Usuarios usuarios)
        {
            if (ModelState.IsValid)
            {
                db.Usuarios.Add(usuarios);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.rol = new SelectList(db.AspNetRoles, "Id", "Name", usuarios.rol);
            ViewBag.idAspNetUsers = new SelectList(db.AspNetUsers, "Id", "Email", usuarios.idAspNetUsers);
            ViewBag.superior = new SelectList(db.Usuarios, "id", "nombrePersona", usuarios.superior);
            return View(usuarios);
        }

        // GET: Usuarios/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuarios usuarios = db.Usuarios.Find(id);
            if (usuarios == null)
            {
                return HttpNotFound();
            }
            ViewBag.rol = new SelectList(db.AspNetRoles, "Id", "Name", usuarios.rol);
            ViewBag.idAspNetUsers = new SelectList(db.AspNetUsers, "Id", "Email", usuarios.idAspNetUsers);
            ViewBag.superior = new SelectList(db.Usuarios, "id", "nombrePersona", usuarios.superior);
            return View(usuarios);
        }

        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "id,nombrePersona,apellidoPersona,nombreUsuario,contraUsuario,edad,numeroTelefono,email,rol,superior,sexo,idAspNetUsers")] Usuarios usuarios)
        {
            if (ModelState.IsValid)
            {
                db.Entry(usuarios).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.rol = new SelectList(db.AspNetRoles, "Id", "Name", usuarios.rol);
            ViewBag.idAspNetUsers = new SelectList(db.AspNetUsers, "Id", "Email", usuarios.idAspNetUsers);
            ViewBag.superior = new SelectList(db.Usuarios, "id", "nombrePersona", usuarios.superior);
            return View(usuarios);
        }

        // GET: Usuarios/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuarios usuarios = db.Usuarios.Find(id);
            if (usuarios == null)
            {
                return HttpNotFound();
            }
            return View(usuarios);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Usuarios usuarios = db.Usuarios.Find(id);
            db.Usuarios.Remove(usuarios);
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
