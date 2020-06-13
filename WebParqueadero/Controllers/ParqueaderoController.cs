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
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNet.Identity.Owin;
using WebParqueadero.Utilidades;

namespace WebParqueadero.Controllers
{
    public class ParqueaderoController : Controller
    {
        private WebParqueaderoContext db = new WebParqueaderoContext();
        private ApplicationUserManager _userManager;
        private ApplicationSignInManager _signInManager;

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

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
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
                        var ResultadoParqueadero = db.Parqueaderoes.Where(t => t.NitEmpresa_Parq == parqueadero.NitEmpresa_Parq).ToList();
                        if (ResultadoParqueadero.Count > 0)
                        {
                            throw new Exception(string.Format("El parquero {0} con número nit {1} ya existe, por favor intente recordar la contraseña.", parqueadero.NombreEmpresa_Parq, parqueadero.NitEmpresa_Parq));
                        }

                        var user = new ApplicationUser { UserName = parqueadero.Correo_Parq, Email = parqueadero.Correo_Parq };
                        var result = await UserManager.CreateAsync(user, parqueadero.CorreoContra_Parq);
                        if (result.Succeeded)
                        {
                            await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                            parqueadero.Id_Parq = Guid.NewGuid();
                            db.Parqueaderoes.Add(parqueadero);
                            await db.SaveChangesAsync();

                            foreach (TipoVehiculos item in db.TipoVehiculos.ToList())
                            {
                                item.Valor_TVeh = parqueadero.Valor_Parq;
                                db.Entry(item).State = EntityState.Modified;
                                await db.SaveChangesAsync();
                            }

                            ParqueaderoUsuarioDetalle parqueaderoUsuarioDetalle = new ParqueaderoUsuarioDetalle();

                            parqueaderoUsuarioDetalle.Id_Parq = parqueadero.Id_Parq;
                            parqueaderoUsuarioDetalle.Id_PUD = Guid.NewGuid();
                            parqueaderoUsuarioDetalle.IdUser_PUD = user.Id;

                            db.ParqueaderoUsuarioDetalle.Add(parqueaderoUsuarioDetalle);
                            await db.SaveChangesAsync();


                            transacion.Commit();
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            foreach (var item in result.Errors)
                                ModelState.AddModelError(string.Empty, string.Format("Error al crear: {0}", item));   
                            
                            transacion.Rollback();
                            return View(parqueadero);
                        }
                    }
                    else
                    {
                        return View(parqueadero);
                        //return Json("Error", JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception ex)
                {
                    transacion.Rollback();
                    ModelState.AddModelError(string.Empty, string.Format("Error al crear: {0}", ex.Message));
                    return View(parqueadero);
                    //return Json(string.Format("Error: {0}",ex.Message), JsonRequestBehavior.AllowGet);
                }
            }
            //return Json(parqueadero, JsonRequestBehavior.AllowGet);
            return View(parqueadero);
        }

        [Authorize]
        // GET: Parqueadero/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            Parqueadero parqueadero = new Parqueadero();
            try
            {
                Impresoras Impresoras = new Impresoras();
                if (id == null || id == Guid.Empty)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                parqueadero = await db.Parqueaderoes.FindAsync(id);
                if (parqueadero == null)
                {
                    return HttpNotFound();
                }

                //if (string.IsNullOrEmpty(parqueadero.Impresora_Parq))
                //{
                //    ViewBag.ltsImpresoras = new SelectList(Impresoras.ObtenerImpresoras().OrderBy(m => m.Id_Imp), "Nombre_Imp", "Nombre_Imp");
                //}
                //else
                //{
                //    ViewBag.ltsImpresoras = new SelectList(Impresoras.ObtenerImpresoras().OrderBy(m => m.Id_Imp), "Nombre_Imp", "Nombre_Imp", parqueadero.Impresora_Parq);
                //}
                
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, string.Format("Error al cargar la vista: {0}", ex.Message));
                return View(parqueadero);
            }
            //return Json(parqueadero, JsonRequestBehavior.AllowGet);
            return View(parqueadero);
        }

        // POST: Parqueadero/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> Edit(Parqueadero parqueadero)
        {
            using (var transaccion = db.Database.BeginTransaction())
            {
                Impresoras Impresoras = new Impresoras();
                try
                {
                    if (ModelState.IsValid)
                    {
                        //string Impresora = Request["ltsImpresoras"];
                        //if (string.IsNullOrEmpty(Impresora) || Impresora == "Seleccionar impresora")
                        //{
                        //    if (string.IsNullOrEmpty(parqueadero.Impresora_Parq))
                        //        ViewBag.ltsImpresoras = new SelectList(Impresoras.ObtenerImpresoras().OrderBy(m => m.Id_Imp), "Nombre_Imp", "Nombre_Imp");
                        //    else
                        //        ViewBag.ltsImpresoras = new SelectList(Impresoras.ObtenerImpresoras().OrderBy(m => m.Id_Imp), "Nombre_Imp", "Nombre_Imp", parqueadero.Impresora_Parq);

                        //    throw new Exception("Por favor selecciona la impresora.");
                        //}
                        //parqueadero.Impresora_Parq = Impresora;
                        db.Entry(parqueadero).State = EntityState.Modified;
                        await db.SaveChangesAsync();
                        transaccion.Commit();
                        return RedirectToAction("Index", "Home");
                    }
                }
                catch (Exception ex)
                {
                    transaccion.Rollback();
                    //if (string.IsNullOrEmpty(parqueadero.Impresora_Parq))
                    //    ViewBag.ltsImpresoras = new SelectList(Impresoras.ObtenerImpresoras().OrderBy(m => m.Id_Imp), "Nombre_Imp", "Nombre_Imp");
                    //else
                    //    ViewBag.ltsImpresoras = new SelectList(Impresoras.ObtenerImpresoras().OrderBy(m => m.Id_Imp), "Nombre_Imp", "Nombre_Imp", parqueadero.Impresora_Parq);
                    ModelState.AddModelError(string.Empty, string.Format("Error al editar: {0}", ex.Message));
                    return View(parqueadero);
                }
            }
            //return Json(parqueadero, JsonRequestBehavior.AllowGet);
            return View(parqueadero);
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
