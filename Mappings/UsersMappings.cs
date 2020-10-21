using Honeywell_backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Honeywell_backend.Mappings
{
    public class UsersMappings : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder
                .HasKey(u => u.Id);

            builder
                .Property(u => u.Name)
                .IsRequired()
                .HasColumnType("varchar(100)");

            builder
                .Property(u => u.Address)
                .IsRequired()
                .HasColumnType("varchar(100)");            

            builder
                .Property(u => u.IsStaff)
                .IsRequired();

        }


    }
}
