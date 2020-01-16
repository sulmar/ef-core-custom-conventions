using ef_core_custom_conventions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Linq;

namespace ef_core_custom_conventions
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }


    public class MyContextFactory : IDesignTimeDbContextFactory<MyContext>
    {
        public MyContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MyContext>();
            optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=mydb01;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

            return new MyContext(optionsBuilder.Options);
        }
    }

    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //var props = from e in modelBuilder.Model.GetEntityTypes()
            //            from p in e.GetProperties()
            //            where p.Name.Contains("Name")
            //               && p.PropertyInfo.PropertyType == typeof(string)
            //            select p;

            //foreach (var p in props)
            //{
            //    p.SetMaxLength(200);
            //    p.IsNullable = false;
            //   // p.DeclaringEntityType.AddKey(p);
            //}

            var props = modelBuilder.Properties<string>()
                .Where(p => p.Name.Contains("Name"));

            foreach (var p in props)
            {
                p.SetMaxLength(50);
                p.IsNullable = false;
                p.SetIsUnicode(true);
                // p.DeclaringEntityType.AddKey(p);
            }


            var dates = modelBuilder.Properties<DateTime>();

            foreach (var p in dates)
            {
                p.SetColumnType("datetime2(3)");
            }

            base.OnModelCreating(modelBuilder);
        }
    }

    public class StringConvention : IEntityTypeAddedConvention
    {
        public void ProcessEntityTypeAdded(
            IConventionEntityTypeBuilder entityTypeBuilder,
            IConventionContext<IConventionEntityTypeBuilder> context)
        {
            
        }
    }

    public class Customer
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
