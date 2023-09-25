using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Cofiguration
{
    internal class ProductCofiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");

            builder.HasKey(x => x.id);
            builder.Property(x => x.id).UseIdentityColumn();

            builder.Property(x => x.price).IsRequired();

            builder.Property(x => x.originalPrice).IsRequired();

            builder.Property(x => x.stock).IsRequired().HasDefaultValue(0);

            builder.Property(x => x.viewCount).IsRequired().HasDefaultValue(0);

        }
    }
}
