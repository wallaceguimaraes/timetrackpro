using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace api.Models.EntityModel.Times
{
    public static class TimeMap
    {
        public static void Map(this EntityTypeBuilder<Time> entity)
        {
            entity.ToTable("Tempo", "cadastro");

            entity.HasKey(p => p.Id);
            entity.Property(p => p.Id).HasColumnName("Id").UseIdentityColumn();
            entity.Property(p => p.StartedAt).HasColumnName("DataInicio").IsRequired();
            entity.Property(p => p.EndedAt).HasColumnName("DataFim");
            entity.HasOne(p => p.Project).WithMany(p => p.Times).HasForeignKey(p => p.ProjectId).OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(p => p.User).WithMany(p => p.Times).HasForeignKey(p => p.UserId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}