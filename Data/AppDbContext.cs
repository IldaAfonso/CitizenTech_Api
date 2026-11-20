using CitizenTech.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace CitizenTech.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Cuidador> Cuidadores => Set<Cuidador>();
    public DbSet<Idoso> Idosos => Set<Idoso>();
    public DbSet<Medicamento> Medicamentos => Set<Medicamento>();
    public DbSet<Evento> Eventos => Set<Evento>();

    protected override void OnModelCreating(ModelBuilder mb)
    {
        // Prefixo NET_ evita conflito com as tabelas CIT_ criadas à mão na disciplina de Banco de Dados
        mb.Entity<Cuidador>().ToTable("NET_CUIDADOR");
        mb.Entity<Idoso>().ToTable("NET_IDOSO");
        mb.Entity<Medicamento>().ToTable("NET_MEDICAMENTO");
        mb.Entity<Evento>().ToTable("NET_EVENTO");

        mb.Entity<Idoso>().HasIndex(i => i.CodigoDispositivo).IsUnique();

        // bool -> NUMBER(1) (1 = sim, 0 = não). O HasConversion<int>() impede que o provider gere o tipo BOOLEAN,
        // que o Oracle 19c não aceita. Na API o campo continua sendo true/false.
        mb.Entity<Medicamento>().Property(m => m.Ativo).HasConversion<int>().HasColumnType("NUMBER(1)");

        // 1:N Cuidador -> Idoso
        mb.Entity<Idoso>()
            .HasOne(i => i.Cuidador).WithMany(c => c.Idosos)
            .HasForeignKey(i => i.CuidadorId).OnDelete(DeleteBehavior.Restrict);

        // 1:N Idoso -> Medicamento
        mb.Entity<Medicamento>()
            .HasOne(m => m.Idoso).WithMany(i => i.Medicamentos)
            .HasForeignKey(m => m.IdosoId).OnDelete(DeleteBehavior.Restrict);

        // 1:N Idoso -> Evento
        mb.Entity<Evento>()
            .HasOne(e => e.Idoso).WithMany(i => i.Eventos)
            .HasForeignKey(e => e.IdosoId).OnDelete(DeleteBehavior.Restrict);

        // 1:N Medicamento -> Evento (opcional)
        mb.Entity<Evento>()
            .HasOne(e => e.Medicamento).WithMany(m => m.Eventos)
            .HasForeignKey(e => e.MedicamentoId).IsRequired(false).OnDelete(DeleteBehavior.Restrict);
    }
}