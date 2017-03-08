using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Sample.Azure.Repository.SqlServer
{
    [DbConfigurationType(typeof(SampleBaseContextConfiguration))]
    public class SampleBaseContent : DbContext
    {
        private static bool hasBeenInitialized = false;

        public SampleBaseContent(): base("name=SampleBaseContent")
        {
            if (hasBeenInitialized == false)
            {
                Database.Initialize(true);
                hasBeenInitialized = true;
            }
        }

        public DbSet<Model.Customer.CustomerModel> CustomerModels { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Entity<Model.Customer.CustomerModel>().ToTable("Customer");
            modelBuilder.Entity<Model.Customer.CustomerModel>().HasKey(t => t.CustomerId);

            // Determines if we use stored procedures or not
            //modelBuilder.Entity<Model.Customer.CustomerModel>().MapToStoredProcedures();
        }

    }
}