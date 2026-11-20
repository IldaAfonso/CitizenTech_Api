using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CitizenTech.Api.Models;

public class Cuidador : IEntity
{
    public int Id { get; set; }

    [Required(ErrorMessage = "O nome é obrigatório.")]
    [StringLength(100)]
    public string Nome { get; set; }

    [Required(ErrorMessage = "O telefone é obrigatório.")]
    [StringLength(20)]
    public string Telefone { get; set; }

    [EmailAddress(ErrorMessage = "E-mail inválido.")]
    [StringLength(120)]
    public string Email { get; set; }

    [Required]
    [RegularExpression("^(FAMILIAR|PROFISSIONAL|VIZINHO)$",
        ErrorMessage = "Tipo deve ser FAMILIAR, PROFISSIONAL ou VIZINHO.")]
    public string Tipo { get; set; }

    [JsonIgnore]
    public List<Idoso> Idosos { get; set; } = new();
}
