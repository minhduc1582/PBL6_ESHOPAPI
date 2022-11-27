using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eshop_api.Entities;
using eshop_pbl6.Entities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
namespace eshop_api.Helpers
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<RoleInPermission>().HasKey(UserInRole => new {
            UserInRole.RoleId, UserInRole.PermissionId
            });

            builder.Entity<Product>(b => {
                b.Property(u => u.Detail)
                .HasConversion(
                    d => JsonConvert.SerializeObject(d, Formatting.None),
                    s => s == null ? new Dictionary<string, object>()
                        : JsonConvert.DeserializeObject<Dictionary<string, object>>(s)
                )
                .HasMaxLength(4000)
                .IsRequired();
            });
        }
        public DbSet<User> AppUsers {get; set;}
        public DbSet<Comunity> Comunities{get;set;} 
        public DbSet<District> Dictricts  {get;set;}  
        public DbSet<Province> Provinces {get;set;}
        public DbSet<Role> Roles {get;set;}
        public DbSet<RoleInPermission> RoleInPermissions{get;set;}
        public DbSet<Permission> Permissions{get;set;}
        public DbSet<Category> Categories{get;set;}
        public DbSet<Product> Products{get;set;}
        public DbSet<OrderDetail> OrderDetails{get;set;}
        public DbSet<Order> Orders{get;set;}
        public DbSet<Comment> Comments{get;set;}
        public DbSet<Image> Images{get;set;}
        public DbSet<History> Histories{get;set;}
    }
}