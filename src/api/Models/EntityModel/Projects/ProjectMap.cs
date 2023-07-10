using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace api.Models.EntityModel.Projects
{
    public static class ProjectMap
    {
        public static void Map(this EntityTypeBuilder<Project> entity)
        {
            entity.ToTable("Projeto", "cadastro");

            entity.HasKey(p => p.Id);
            entity.Property(p => p.Id).HasColumnName("Id").UseIdentityColumn();
            entity.Property(p => p.Title).HasColumnName("Titulo").HasMaxLength(45);
            entity.Property(p => p.Description).HasColumnName("Descricao").HasMaxLength(150);
            entity.HasMany(p => p.Times).WithOne(p => p.Project).HasForeignKey(p => p.ProjectId).OnDelete(DeleteBehavior.Cascade);
            entity.HasMany(p => p.UserProjects).WithOne(p => p.Project).HasForeignKey(p => p.ProjectId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}