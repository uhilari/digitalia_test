using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HS
{
    public static class EntityMapping
    {
        public static EntityTypeBuilder<T> Entidad<T>(this EntityTypeBuilder<T> builder) where T : Entity
        {
            builder.Property(c => c.Id)
                .HasValueGenerator<SequentialGuidValueGenerator>();
            builder.Property(c => c.Eliminado);
            builder.HasKey(c => c.Id);
            return builder;
        }
    }
}
