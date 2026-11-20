using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CitizenTech.Api.Models;

public class Idoso : IEntity
{
    public int Id { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "CuidadorId é obrigatório.")]
    public int CuidadorId { get; set; }

    [Required(ErrorMessage = "O nome é obrigatório.")]
    [StringLength(100)]
    public string Nome { get; set; }

    [Required(ErrorMessage = "A data de nascimento é obrigatória.")]
    public DateTime? DataNascimento { get; set; }

    [StringLength(20)]
    public string Telefone { get; set; }

    [StringLength(200)]
    public string Endereco { get; set; }

    // Identifica o Arduino/ESP32 instalado na casa do idoso
    [Required(ErrorMessage = "O código do dispositivo é obrigatório.")]
    [StringLength(30)]
    public string CodigoDispositivo { get; set; }

    [JsonIgnore] public Cuidador Cuidador { get; set; }
    [JsonIgnore] public List<Medicamento> Medicamentos { get; set; } = new();
    [JsonIgnore] public List<Evento> Eventos { get; set; } = new();
}
