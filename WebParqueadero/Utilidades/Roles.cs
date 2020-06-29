using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebParqueadero.Models;

namespace WebParqueadero.Utilidades
{
    public class RolesParqueadero : IDisposable
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public void CrearRoles(string Role)
        {
            var Rolmanager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            if (!Rolmanager.RoleExists(Role))
            {
                Rolmanager.Create(new IdentityRole(Role));
            }
        }

        public void AddUserRol(string Email, string contra, string Role)
        {
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var User = UserManager.FindByName(Email);
            if (User == null)
            {
                User = new ApplicationUser
                {
                    UserName = Email,
                    Email = Email
                };
            }
            UserManager.Create(User, contra);
            UserManager.AddToRole(User.Id, Role);
        }

        public void AddPermisionToUser(string Correo, string rol)
        {
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));

            var User = UserManager.FindByName(Correo);
            if (!UserManager.IsInRole(User.Id, rol))
            {
                UserManager.AddToRole(User.Id, rol);
            }
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}