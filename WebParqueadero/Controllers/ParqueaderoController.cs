﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebParqueadero.Models;

namespace WebParqueadero.Controllers
{
    public class ParqueaderoController : Controller
    {
        private WebParqueaderoContext db = new WebParqueaderoContext();

        // GET: Parqueadero
        public async Task<ActionResult> Index()
        {
            //return Json(db.Parqueaderoes.ToListAsync(), JsonRequestBehavior.AllowGet);
            return View(await db.Parqueaderoes.ToListAsync());
        }

        // GET: Parqueadero/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Parqueadero parqueadero = await db.Parqueaderoes.FindAsync(id);
            if (parqueadero == null)
            {
                return HttpNotFound();
            }

            return Json(parqueadero, JsonRequestBehavior.AllowGet);
            //return View(parqueadero);
        }

        // GET: Parqueadero/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Parqueadero/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Parqueadero parqueadero)
        {
            using (var transacion = db.Database.BeginTransaction())
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        parqueadero.Id_Parq = Guid.NewGuid();
                        db.Parqueaderoes.Add(parqueadero);
                        await db.SaveChangesAsync();
                        transacion.Commit();
                        return RedirectToAction("Index");
                        //return Json(parqueadero, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json("Error", JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception ex)
                {
                    transacion.Rollback();
                    return Json(string.Format("Error: {0}",ex.Message), JsonRequestBehavior.AllowGet);
                }
            }
            return Json(parqueadero, JsonRequestBehavior.AllowGet);
            //return View(parqueadero);
        }

        // GET: Parqueadero/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Parqueadero parqueadero = await db.Parqueaderoes.FindAsync(id);
            if (parqueadero == null)
            {
                return HttpNotFound();
            }
            return Json(parqueadero, JsonRequestBehavior.AllowGet);
            //return View(parqueadero);
        }

        // POST: Parqueadero/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Parqueadero parqueadero)
        {
            if (ModelState.IsValid)
            {
                db.Entry(parqueadero).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return Json(parqueadero, JsonRequestBehavior.AllowGet);
            //return View(parqueadero);
        }

        // GET: Parqueadero/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Parqueadero parqueadero = await db.Parqueaderoes.FindAsync(id);
            if (parqueadero == null)
            {
                return HttpNotFound();
            }
            return Json(parqueadero, JsonRequestBehavior.AllowGet);
            //return View(parqueadero);
        }

        // POST: Parqueadero/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            Parqueadero parqueadero = await db.Parqueaderoes.FindAsync(id);
            db.Parqueaderoes.Remove(parqueadero);
            await db.SaveChangesAsync();
            return Json(parqueadero, JsonRequestBehavior.AllowGet);
            //return RedirectToAction("Index");
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
