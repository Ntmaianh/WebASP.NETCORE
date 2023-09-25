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
    // đây là lớp trung gian của quan hệ nhiều nhiều 
    internal class ProductInCategoryConfiguration : IEntityTypeConfiguration<ProductInCategory>
    {
        public void Configure(EntityTypeBuilder<ProductInCategory> builder)
        {
            builder.HasKey(x => new { x.ProductId, x.CategoryId });
            builder.ToTable("ProductInCategories");

            // liên kết khóa ngoại + mối quan hệ 1-n với bản trung gian ở giữa
            // dịch : 1 product <HasOne> có nhiều <WithMany> trong bảng ProductInCategories 
            builder.HasOne(x => x.Product).WithMany(x => x.ProductInCategories).HasForeignKey(x=> x.ProductId);
            builder.HasOne(x => x.Category).WithMany(x => x.ProductInCategories).HasForeignKey(x=> x.CategoryId);
        }
    }
}
