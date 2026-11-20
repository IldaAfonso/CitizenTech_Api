using CitizenTech.Api.Data;
using CitizenTech.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CitizenTech.Api.Controllers;

[ApiController]
[Route("api/eventos")]
public class EventosController : CrudController<Evento>
{
    public EventosController(AppDbContext db) : base(db) { }

    protected override async Task<string> ValidarRelacionamentos(Evento ev)
    {
           if (await Db.Idosos.CountAsync(i => i.Id == ev.IdosoId) == 0)
            return $"Idoso com id {ev.IdosoId} não existe.";

        if (ev.MedicamentoId.HasValue &&
             await Db.Medicamentos.CountAsync(m => m.Id == ev.MedicamentoId && m.IdosoId == ev.IdosoId) == 0)
            return $"Medicamento {ev.MedicamentoId} não existe ou não pertence ao idoso {ev.IdosoId}.";

        return null;
    }
}
