using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Entities.Users
{
    public class Permission:BaseEntity<int>
    {
        public string ActionFullName { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }
    }

    public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.Property(p => p.ActionFullName).HasMaxLength(200);
            builder.HasOne(p => p.Role)
                .WithMany(p => p.Permissions)
                .HasForeignKey(p => p.RoleId);
        }
    }
}
