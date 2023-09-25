using Data.Cofiguration;
using Data.Entities;
using Data.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EF
{
    public class ESHOPDBContext : IdentityDbContext<AppUser,AppRole,Guid> 
    {
        public ESHOPDBContext(DbContextOptions options) : base(options)
        {
        }

        // phường thức này để sử dụng Fulent 
        // ghi đè phương thức OnModelCreating ở net core (lớp cha) đã được tạo sẵn giờ ghi đè vào 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            // Fluent 
            modelBuilder.ApplyConfiguration(new AppConfigConfiguration()); 
            modelBuilder.ApplyConfiguration(new ProductCofiguration());
            modelBuilder.ApplyConfiguration(new CategoryCofiguration()); 
            modelBuilder.ApplyConfiguration(new ProductInCategoryConfiguration());
            modelBuilder.ApplyConfiguration(new OrderCofiguration()); 
            modelBuilder.ApplyConfiguration(new OrderDetailConfiguration());
            modelBuilder.ApplyConfiguration(new CartConfiguration()); 
            modelBuilder.ApplyConfiguration(new CategoryTranslationConfiguration()); 
            modelBuilder.ApplyConfiguration(new ContactConfiguration()); 
            modelBuilder.ApplyConfiguration(new SlideConfiguration());
            modelBuilder.ApplyConfiguration(new ProductTranslationConfiguration());
            modelBuilder.ApplyConfiguration(new ProductImageConfiguration());
            modelBuilder.ApplyConfiguration(new PromotionConfiguration());
            modelBuilder.ApplyConfiguration(new LanguageConfiguration()); 
            modelBuilder.ApplyConfiguration(new TransactionConfiguration());

            modelBuilder.ApplyConfiguration(new AppUserConfiguration());
            modelBuilder.ApplyConfiguration(new AppRoleConfiguration());

            
            modelBuilder.Entity<IdentityUserClaim<Guid>>().ToTable("AppUserClaims");
            modelBuilder.Entity<IdentityUserRole<Guid>>().ToTable("AppUserRoles").HasKey(x => new { x.UserId, x.RoleId });
            modelBuilder.Entity<IdentityUserLogin<Guid>>().ToTable("AppUserLogins").HasKey(x => x.UserId);

            modelBuilder.Entity<IdentityRoleClaim<Guid>>().ToTable("AppRoleClaims");
            modelBuilder.Entity<IdentityUserToken<Guid>>().ToTable("AppUserTokens").HasKey(x => x.UserId);
            // Data Seeding 
            modelBuilder.Seed();
        }

        public override bool Equals(object? obj)
        {
            return obj is ESHOPDBContext context &&
                   base.Equals(obj) &&
                   EqualityComparer<DatabaseFacade>.Default.Equals(Database, context.Database) &&
                   EqualityComparer<ChangeTracker>.Default.Equals(ChangeTracker, context.ChangeTracker) &&
                   EqualityComparer<IModel>.Default.Equals(Model, context.Model) &&
                   EqualityComparer<DbContextId>.Default.Equals(ContextId, context.ContextId) &&
                   EqualityComparer<DbSet<AppUser>>.Default.Equals(Users, context.Users) &&
                   EqualityComparer<DbSet<IdentityUserClaim<Guid>>>.Default.Equals(UserClaims, context.UserClaims) &&
                   EqualityComparer<DbSet<IdentityUserLogin<Guid>>>.Default.Equals(UserLogins, context.UserLogins) &&
                   EqualityComparer<DbSet<IdentityUserToken<Guid>>>.Default.Equals(UserTokens, context.UserTokens) &&
                   EqualityComparer<DbSet<IdentityUserRole<Guid>>>.Default.Equals(UserRoles, context.UserRoles) &&
                   EqualityComparer<DbSet<AppRole>>.Default.Equals(Roles, context.Roles) &&
                   EqualityComparer<DbSet<IdentityRoleClaim<Guid>>>.Default.Equals(RoleClaims, context.RoleClaims) &&
                   EqualityComparer<DbSet<Product>>.Default.Equals(Products, context.Products) &&
                   EqualityComparer<DbSet<Category>>.Default.Equals(Categories, context.Categories) &&
                   EqualityComparer<DbSet<AppConfig>>.Default.Equals(AppConfigs, context.AppConfigs) &&
                   EqualityComparer<DbSet<Cart>>.Default.Equals(Carts, context.Carts) &&
                   EqualityComparer<DbSet<CategoryTranslation>>.Default.Equals(CategoryTranslations, context.CategoryTranslations) &&
                   EqualityComparer<DbSet<ProductInCategory>>.Default.Equals(ProductInCategories, context.ProductInCategories) &&
                   EqualityComparer<DbSet<Contact>>.Default.Equals(Contacts, context.Contacts) &&
                   EqualityComparer<DbSet<Language>>.Default.Equals(Languages, context.Languages) &&
                   EqualityComparer<DbSet<Oder>>.Default.Equals(Orders, context.Orders) &&
                   EqualityComparer<DbSet<OderDetail>>.Default.Equals(OrderDetails, context.OrderDetails) &&
                   EqualityComparer<DbSet<ProductTranslation>>.Default.Equals(ProductTranslations, context.ProductTranslations) &&
                   EqualityComparer<DbSet<Promotion>>.Default.Equals(Promotions, context.Promotions) &&
                   EqualityComparer<DbSet<Transaction>>.Default.Equals(Transactions, context.Transactions) &&
                   EqualityComparer<DbSet<ProductIMage>>.Default.Equals(ProductImages, context.ProductImages) &&
                   EqualityComparer<DbSet<Slide>>.Default.Equals(Slides, context.Slides);
        }

        public DbSet<Product> Products { get; set; }
       public DbSet<Category> Categories  { get; set; }
        public DbSet<AppConfig> AppConfigs { get; set; }

        public DbSet<Cart> Carts { get; set; }

        public DbSet<CategoryTranslation> CategoryTranslations { get; set; }
        public DbSet<ProductInCategory> ProductInCategories { get; set; }

        public DbSet<Contact> Contacts { get; set; }

        public DbSet<Language> Languages { get; set; }

        public DbSet<Oder> Orders { get; set; }

        public DbSet<OderDetail> OrderDetails { get; set; }
        public DbSet<ProductTranslation> ProductTranslations { get; set; }

        public DbSet<Promotion> Promotions { get; set; }

        public DbSet<Transaction> Transactions { get; set; }

        public DbSet<ProductIMage> ProductImages { get; set; }

        public DbSet<Slide> Slides { get; set; }
    }
}
