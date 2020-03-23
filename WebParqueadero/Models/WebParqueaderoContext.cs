using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace WebParqueadero.Models
{
    public class WebParqueaderoContext : DbContext
    {
        public WebParqueaderoContext() : base("name=WebParqueaderoContext")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }

        public DbSet<Parqueadero> Parqueaderoes { get; set; }
        public DbSet<TipoVehiculos> TipoVehiculos { get; set; }
        public DbSet<Vehiculo> Vehiculo { get; set; }
        public DbSet<Documento> Documento { get; set; }
        public DbSet<DetalleDocumento> DetalleDocumento { get; set; }

    }
}