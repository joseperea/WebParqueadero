using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebParqueadero.Models;

namespace WebParqueadero.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class TipoVehiculosController : Controller
    {
        private readonly WebParqueaderoContext db = new WebParqueaderoContext();

        
        // GET: TipoVehiculos
        public async Task<ActionResult> Index(Guid? id)
        {
            try
            {
                if (id == null || id == Guid.Empty)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                Parqueadero parqueadero = db.Parqueaderoes.Find(id);

                ViewBag.Lavar = parqueadero.Lavar;
                ViewBag.Casillero = parqueadero.Casillero;
                ViewBag.Id_Paq = parqueadero.Id_Parq;

                return View(await db.TipoVehiculos.OrderBy(t => t.Estado_TVeh).OrderBy(x => x.Nombre_TVeh).ToListAsync());
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, string.Format("Error al ingresar vehiculo: {0}", ex.Message));
                return View("Index", await db.TipoVehiculos.OrderBy(t => t.Estado_TVeh).OrderBy(x => x.Nombre_TVeh).ToListAsync());
            }
        }

        // GET: TipoVehiculos/Details/5
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
        public ActionResult Create()
        {
            return View();
        }

        // POST: TipoVehiculos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(TipoVehiculos tipoVehiculos, Guid? Id_Paq)
        {
            using (DbContextTransaction transaccion = db.Database.BeginTransaction())
            {
                if (Id_Paq == null || Id_Paq == Guid.Empty)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                Parqueadero parqueadero = db.Parqueaderoes.Find(Id_Paq);

                ViewBag.Lavar = parqueadero.Lavar;
                ViewBag.Casillero = parqueadero.Casillero;
                ViewBag.Id_Paq = parqueadero.Id_Parq;

                try
                {
                    if (ModelState.IsValid)
                    {
                        tipoVehiculos.Id_TVeh = Guid.NewGuid();
                        tipoVehiculos.Nombre_TVeh.ToUpper();
                        tipoVehiculos.CaracterImagen_TVeh = tipoVehiculos.Nombre_TVeh.Trim().Substring(0, 1).ToUpper();
                        tipoVehiculos.Estado_TVeh = true;
                        if (db.TipoVehiculos.Where(t => t.CaracterImagen_TVeh == tipoVehiculos.CaracterImagen_TVeh).ToList().Count > 0)
                        {
                            tipoVehiculos.CaracterImagen_TVeh = tipoVehiculos.Nombre_TVeh.Trim().Substring(0, 2).ToUpper();
                        }
                        db.TipoVehiculos.Add(tipoVehiculos);
                        await db.SaveChangesAsync();
                        transaccion.Commit();
                        return RedirectToAction("Index", new { id = parqueadero.Id_Parq });
                    }
                }
                catch (Exception ex)
                {
                    transaccion.Rollback();
                    ModelState.AddModelError(string.Empty, string.Format("Error al ingresar vehiculo: {0}", ex.Message));
                    return View("Index", await db.TipoVehiculos.OrderBy(t => t.Estado_TVeh).OrderBy(x => x.Nombre_TVeh).ToListAsync());
                }
            }
            return RedirectToAction("Index");
        }

        // GET: TipoVehiculos/Edit/5
        public async Task<ActionResult> Edit(Guid? id, Guid? Id_Paq)
        {
            TipoVehiculos tipoVehiculos = new TipoVehiculos();

            if (Id_Paq == null || Id_Paq == Guid.Empty)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Parqueadero parqueadero = db.Parqueaderoes.Find(Id_Paq);

            ViewBag.Lavar = parqueadero.Lavar;
            ViewBag.Casillero = parqueadero.Casillero;
            ViewBag.Id_Paq = parqueadero.Id_Parq;

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
                return View("Index", await db.TipoVehiculos.OrderBy(t => t.Estado_TVeh).OrderBy(x => x.Nombre_TVeh).ToListAsync());
            }
            return PartialView("_ModalEditarViewPartial", tipoVehiculos);
        }

        // POST: TipoVehiculos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(TipoVehiculos tipoVehiculos, Guid? Id_Paq)
        {
            using (DbContextTransaction transaccion = db.Database.BeginTransaction())
            {
                if (Id_Paq == null || Id_Paq == Guid.Empty)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                Parqueadero parqueadero = db.Parqueaderoes.Find(Id_Paq);

                ViewBag.Lavar = parqueadero.Lavar;
                ViewBag.Casillero = parqueadero.Casillero;
                ViewBag.Id_Paq = parqueadero.Id_Parq;

                try
                {
                    if (ModelState.IsValid)
                    {
                        if (tipoVehiculos.Lavar_TVeh)
                        {
                            if (tipoVehiculos.ValorLavado_TVeh <= 0)
                            {
                                throw new Exception("Por favor ingrese el valor del lavado del vehiculo.");
                            }
                        }

                        if (tipoVehiculos.Casillero_TVeh)
                        {
                            if (tipoVehiculos.ValorCasillero_TVeh <= 0)
                            {
                                throw new Exception("Por favor ingrese el valor de casillero por articulo.");
                            }
                        }

                        tipoVehiculos.Lavar_TVeh = (tipoVehiculos.ValorLavado_TVeh > 0 && !tipoVehiculos.Lavar_TVeh) ? true : tipoVehiculos.Lavar_TVeh;
                        tipoVehiculos.Casillero_TVeh = (tipoVehiculos.ValorCasillero_TVeh > 0 && !tipoVehiculos.Casillero_TVeh) ? true : tipoVehiculos.Casillero_TVeh;

                        db.Entry(tipoVehiculos).State = EntityState.Modified;
                        await db.SaveChangesAsync();

                        transaccion.Commit();
                        return RedirectToAction("Index", new { id = parqueadero.Id_Parq });
                    }
                }
                catch (Exception ex)
                {
                    transaccion.Rollback();
                    ModelState.AddModelError(string.Empty, string.Format("Error al editar tipo de vehiculo: {0}", ex.Message));

                    return View("Index", await db.TipoVehiculos.OrderBy(t => t.Estado_TVeh).OrderBy(x => x.Nombre_TVeh).ToListAsync());
                }
            }
            return RedirectToAction("Index");
        }

        // GET: TipoVehiculos/Delete/5
        public async Task<ActionResult> Delete(Guid? id, Guid? Id_Paq)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (Id_Paq == null || Id_Paq == Guid.Empty)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoVehiculos tipoVehiculos = await db.TipoVehiculos.FindAsync(id);
            if (tipoVehiculos == null)
            {
                return HttpNotFound();
            }
            
            tipoVehiculos.Estado_TVeh = tipoVehiculos.Estado_TVeh ? false : true;
            db.Entry(tipoVehiculos).State = EntityState.Modified;
            await db.SaveChangesAsync();

            Parqueadero parqueadero = db.Parqueaderoes.Find(Id_Paq);

            ViewBag.Lavar = parqueadero.Lavar;
            ViewBag.Casillero = parqueadero.Casillero;
            ViewBag.Id_Paq = parqueadero.Id_Parq;

            return RedirectToAction("Index", new { id = parqueadero.Id_Parq });
        }

        // POST: TipoVehiculos/Delete/5
        
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //[Authorize]
        //public async Task<ActionResult> DeleteConfirmed(Guid id)
        //{
        //    TipoVehiculos tipoVehiculos = await db.TipoVehiculos.FindAsync(id);
        //    db.TipoVehiculos.Remove(tipoVehiculos);
        //    await db.SaveChangesAsync();
        //    return RedirectToAction("Index");
        //}

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
