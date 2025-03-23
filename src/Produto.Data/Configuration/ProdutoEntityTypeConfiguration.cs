namespace Produto.Data.Configuration;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Produto.Data.Entities;

public class ProdutoEntityTypeConfiguration : IEntityTypeConfiguration<ProdutoEntity>
{
    public void Configure(EntityTypeBuilder<ProdutoEntity> builder)
    {
        builder.ToTable("produtos");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Nome)
            .IsRequired()
            .HasMaxLength(100);
       
        builder.HasIndex(p => p.Nome)
            .IsUnique();

        builder.Property(p => p.Descricao)
            .HasMaxLength(500);

        builder.Property(p => p.Valor)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(p => p.Estoque)
           .IsRequired();

        builder.HasData([
            new ProdutoEntity(
                nome: "Produto 1",
                descricao: "Descrição do Produto 1",
                valor: 10.00m,
                estoque: 100){
                Id = new Guid("0195c525-9a61-711c-8f23-fe5648bb690a")
            },
            new ProdutoEntity(
                nome: "Produto 2",
                descricao: "Descrição do Produto 2",
                valor: 20.00m,
                estoque: 200){
                Id = new Guid("0195c525-9a61-711c-8f24-03f9b0e3cd25")
            },
            new ProdutoEntity(
                nome: "Produto 3",
                descricao: "Descrição do Produto 3",
                valor: 30.00m,
                estoque: 300){
                Id = new Guid("0195c525-9a61-711c-8f24-0592096c4b61")
            },
            new ProdutoEntity(
                nome: "Produto 4",
                descricao: "Descrição do Produto 4",
                valor: 40.00m,
                estoque: 400){
                Id = new Guid("0195c525-9a61-711c-8f24-0b3d685cc7d6")
            },
            new ProdutoEntity(
                nome: "Produto 5",
                descricao: "Descrição do Produto 5",
                valor: 50.00m,
                estoque: 500){
                Id = new Guid("0195c525-9a61-711c-8f24-0c11233469cf")
            }
        ]);
    }
}