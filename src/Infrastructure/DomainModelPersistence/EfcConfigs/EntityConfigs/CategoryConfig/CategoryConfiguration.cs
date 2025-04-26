using Domain.Aggregates.ServiceCategory;
using Domain.Aggregates.ServiceCategory.Entities;
using Domain.Aggregates.ServiceCategory.Values;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DomainModelPersistence.EfcConfigs.EntityConfigs.CategoryConfig;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> entityBuilder)
    {
        entityBuilder.HasKey(c => c.CategoryId);
        
        entityBuilder.Property(c => c.CategoryId)
             .IsRequired()
             .HasConversion(
                 id => id.Value,
                 value => CategoryId.FromGuid(value));

        entityBuilder.ComplexProperty(p => p.CategoryName, b =>
        {
            b.Property(v => v.Value).HasColumnName("CategoryName");
        });
        
        entityBuilder.ComplexProperty(p => p.Description, b =>
         {
             b.Property(v => v.Value).HasColumnName("Description");
         });
        
        entityBuilder.OwnsMany(s => s.MediaUrls, mediaBuilder =>
         {
             mediaBuilder.ToTable("MediaUrls"); 
             mediaBuilder.WithOwner().HasForeignKey("CategoryId");

             mediaBuilder.Property<Guid>("Id"); 
             mediaBuilder.HasKey("Id");

             mediaBuilder.Property(m => m.Value)
                 .HasColumnName("Url")
                 .IsRequired();
         });
        
        // configure services
        entityBuilder.HasMany<Service>(cat => cat._services)
            .WithOne()
            .HasForeignKey("CategoryId")
            .OnDelete(DeleteBehavior.Cascade);
    }
}