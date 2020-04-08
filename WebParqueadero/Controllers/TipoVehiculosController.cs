using System;
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
    public class TipoVehiculosController : Controller
    {
        private WebParqueaderoContext db = new WebParqueaderoContext();

        // GET: TipoVehiculos
        [Authorize]
        public async Task<ActionResult> Index()
        { 
            return View(await db.TipoVehiculos.ToListAsync());
        }

        // GET: TipoVehiculos/Details/5
        [Authorize]
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoVehiculos tipoVehiculos = await db.TipoVehiculos.FindAsync(id);
            if (tipoVehiculos == null)
            {
                return HttpNotFound();
            }
            return View(tipoVehiculos);
        }

        // GET: TipoVehiculos/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: TipoVehiculos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> Create(TipoVehiculos tipoVehiculos)
        {
            using (var transaccion = db.Database.BeginTransaction())
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        tipoVehiculos.Id_TVeh = Guid.NewGuid();
                        tipoVehiculos.Nombre_TVeh.ToUpper();
                        tipoVehiculos.CaracterImagen_TVeh = tipoVehiculos.Nombre_TVeh.Trim().Substring(0,1).ToUpper();
                        tipoVehiculos.Estado_TVeh = true;
                        if (db.TipoVehiculos.Where(t => t.CaracterImagen_TVeh == tipoVehiculos.CaracterImagen_TVeh).ToList().Count > 0)
                        {
                            tipoVehiculos.CaracterImagen_TVeh = tipoVehiculos.Nombre_TVeh.Trim().Substring(0, 2).ToUpper();
                        }
                        db.TipoVehiculos.Add(tipoVehiculos);
                        await db.SaveChangesAsync();
                        transaccion.Commit();
                        return RedirectToAction("Index");
                    }
                }
                catch (Exception ex)
                {
                    transaccion.Rollback();
                    ModelState.AddModelError(string.Empty, string.Format("Error al ingresar vehiculo: {0}", ex.Message));
                    return View("Index");
                }
            }

            return View("Index");
        }

        // GET: TipoVehiculos/Edit/5
        [Authorize]
        public async Task<ActionResult> Edit(Guid? id)
        {
            TipoVehiculos tipoVehiculos = new TipoVehiculos();
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                tipoVehiculos = await db.TipoVehiculos.FindAsync(id);
                if (tipoVehiculos == null)
                {
                    return HttpNotFound();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, string.Format("Error al cargar modal: {0}", ex.Message));
                return View("Index");
            }
            return PartialView("_ModalEditarViewPartial", tipoVehiculos);
        }

        // POST: TipoVehiculos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> Edit(TipoVehiculos tipoVehiculos)
        {
            using (var transaccion = db.Database.BeginTransaction())
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        db.Entry(tipoVehiculos).State = EntityState.Modified;
                        await db.SaveChangesAsync();
                        transaccion.Commit();
                        return RedirectToAction("Index");
                    }
                }
                catch (Exception ex)
                {
                    transaccion.Rollback();
                    ModelState.AddModelError(string.Empty, string.Format("Error al editar tipo de vehiculo: {0}", ex.Message));
                    return View("Index");
                }
            }

            return View("Index");
        }

        // GET: TipoVehiculos/Delete/5
        [Authorize]
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoVehiculos tipoVehiculos = await db.TipoVehiculos.FindAsync(id);
            if (tipoVehiculos == null)
            {
                return HttpNotFound();
            }
            return View(tipoVehiculos);
        }

        // POST: TipoVehiculos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            TipoVehiculos tipoVehiculos = await db.TipoVehiculos.FindAsync(id);
            db.TipoVehiculos.Remove(tipoVehiculos);
            await db.SaveChangesAsync();
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
