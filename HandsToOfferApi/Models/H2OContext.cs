using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace HandsToOfferApi.Models
{
    public class H2OContext : DbContext 
    {
        public H2OContext() : base("H2OContext")
        {
        }

        public DbSet<Project> Project { get; set; }
        public DbSet<Event> Event { get; set; }
    }
}