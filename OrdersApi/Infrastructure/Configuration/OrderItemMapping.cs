using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrdersApi.Entities;

namespace OrdersApi.Infrastructure.Configuration
{
    public class OrderItemMapping : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.ProductType).IsRequired();
            builder.HasOne(p => p.Order)
                .WithMany(c => c.Items)
                .HasForeignKey(p => p.OrderId)
                .OnDelete(DeleteBehavior.ClientCascade);
        }
    }
}
