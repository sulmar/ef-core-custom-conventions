using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
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

    public static class ModelBuilderExtensions
    {
        public static IEnumerable<IMutableProperty> Properties(this ModelBuilder modelBuilder)
        {
            var props = from e in modelBuilder.Model.GetEntityTypes()
                        from p in e.GetProperties()
                        select p;

            return props;
        }

        public static IEnumerable<IMutableProperty> Properties<T>(this ModelBuilder modelBuilder) 
            =>  modelBuilder.Properties().Where(p => p.PropertyInfo.PropertyType == typeof(T));
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

            var props2 = modelBuilder.Properties<string>()
                .Where(p => p.Name.Contains("Name"));

            foreach (var p in props2)
            {
                p.SetMaxLength(50);
                p.IsNullable = false;
                p.SetIsUnicode(true);
                // p.DeclaringEntityType.AddKey(p);
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
    }
}
