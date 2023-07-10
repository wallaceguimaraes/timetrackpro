using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace api.Models.EntityModel.UserProjects
{
    public static class UserProjectMap
    {
        public static void Map(this EntityTypeBuilder<UserProject> entity)
        {
            entity.ToTable("UsuarioProjeto", "cadastro");

            entity.HasKey(p => p.Id);
            entity.Property(p => p.Id).HasColumnName("Id").UseIdentityColumn();
            entity.HasOne(p => p.Project).WithMany(p => p.UserProjects).HasForeignKey(p => p.ProjectId).OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(p => p.User).WithMany().HasForeignKey(p => p.UserId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}