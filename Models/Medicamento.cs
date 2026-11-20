using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CitizenTech.Api.Models;

public class Medicamento : IEntity
{
    public int Id { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "IdosoId é obrigatório.")]
    public int IdosoId { get; set; }

    [Required(ErrorMessage = "O nome é obrigatório.")]
    [StringLength(100)]
    public string Nome { get; set; }

    [StringLength(50)]
    public string Dosagem { get; set; }

    [Required]
    [RegularExpression(@"^([01]\d|2[0-3]):[0-5]\d$", ErrorMessage = "Horário deve estar no formato HH:MM.")]
    public string HorarioPrevisto { get; set; }

    public bool Ativo { get; set; } = true;

    [JsonIgnore] public Idoso Idoso { get; set; }
    [JsonIgnore] public List<Evento> Eventos { get; set; } = new();
}
