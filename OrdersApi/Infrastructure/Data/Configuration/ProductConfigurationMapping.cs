using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrdersApi.Entities;

namespace OrdersApi.Infrastructure.Data.Configuration
{
    public class ProductConfigurationMapping : IEntityTypeConfiguration<ProductConfiguration>
    {
        public void Configure(EntityTypeBuilder<ProductConfiguration> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.ProductType).IsRequired();
            builder.Property(p => p.Width).IsRequired();
            builder.Property(p => p.NumberOfItemsInStack).IsRequired();
        }
    }
}
