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

        public System.Data.Entity.DbSet<WebParqueadero.Models.Parqueadero> Parqueaderoes { get; set; }
    }
}