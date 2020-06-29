using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using WebParqueadero.Models;
using WebParqueadero.Utilidades;
using Microsoft.AspNet.Identity;

namespace WebParqueadero
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private WebParqueaderoContext db = new WebParqueaderoContext();
        protected void Application_Start()
        {

            IngresarVehiculos ingresarVehiculos = new IngresarVehiculos();
            Database.SetInitializer(new MigrateDatabaseToLatestVersion
            <WebParqueaderoContext, Migrations.Configuration>());
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ingresarVehiculos.Ingresar(db);
            CrearRoles();

        }

        private void CrearRoles()
        {
            RolesParqueadero roles = new RolesParqueadero(); 
            roles.CrearRoles("Administrador");
            roles.CrearRoles("Cajero");
        }

    }
}
