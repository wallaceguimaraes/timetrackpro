using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace api.Models.EntityModel.Users
{
    public static class UserMap
    {
        public static void Map(this EntityTypeBuilder<User> entity)
        {
            entity.ToTable("Usuario", "cadastro");

            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("Id").UseIdentityColumn();
            entity.HasIndex(e => e.Email).IsUnique();
            entity.Property(e => e.Name).HasColumnName("Nome").HasMaxLength(45).IsRequired();
            entity.Property(e => e.Salt).HasColumnName("Salt").HasMaxLength(150).IsRequired();
            entity.Property(e => e.Login).HasColumnName("Login").HasMaxLength(30).IsRequired();
            entity.Property(e => e.Password).HasColumnName("Senha").HasMaxLength(150).IsRequired();
            entity.Property(e => e.CreatedAt).HasColumnName("DataCadastro").IsRequired();
            entity.Property(e => e.LastUpdateAt).HasColumnName("UltimaAtualizacao");

            entity.HasIndex(e => e.Email);
            entity.HasIndex(e => e.Login);
        }
    }
}