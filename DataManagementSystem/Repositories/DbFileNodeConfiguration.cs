using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataManagementSystem.Repositories
{
    /*
     * Configuration for a DbFileNode
     */
    public class DbFileNodeConfiguration : IEntityTypeConfiguration<DbFileNode>
    {
        public void Configure(EntityTypeBuilder<DbFileNode> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.DateTime).IsRequired();
            builder.Property(x => x.FileBytes).IsRequired();
            builder.Property(x => x.FileName).IsRequired();
            builder.Property(x => x.RelativePath).IsRequired();
        }
    }
}
