using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CitizenTech.Api.Models;

public class Evento : IEntity
{
    public int Id { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "IdosoId é obrigatório.")]
    public int IdosoId { get; set; }

    // Só preenchido em eventos de medicamento
    public int? MedicamentoId { get; set; }

    [Required]
    [RegularExpression("^(BOTAO_EMERGENCIA|MEDICAMENTO_TOMADO|MEDICAMENTO_NAO_TOMADO|SEM_MOVIMENTO)$",
        ErrorMessage = "Tipo inválido. Use BOTAO_EMERGENCIA, MEDICAMENTO_TOMADO, MEDICAMENTO_NAO_TOMADO ou SEM_MOVIMENTO.")]
    public string Tipo { get; set; }

    [Required]
    [RegularExpression("^(PENDENTE|NOTIFICADO|RESOLVIDO)$",
        ErrorMessage = "Status deve ser PENDENTE, NOTIFICADO ou RESOLVIDO.")]
    public string Status { get; set; } = "PENDENTE";

    public DateTime DataHora { get; set; } = DateTime.Now;

    [StringLength(200)]
    public string Observacao { get; set; }

    [JsonIgnore] public Idoso Idoso { get; set; }
    [JsonIgnore] public Medicamento Medicamento { get; set; }
}
