using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Collections.Generic;
using System.Linq;

namespace ef_core_custom_conventions
{
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
