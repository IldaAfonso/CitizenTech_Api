using CitizenTech.Api.Data;
using CitizenTech.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CitizenTech.Api.Controllers;

[ApiController]
[Route("api/medicamentos")]
public class MedicamentosController : CrudController<Medicamento>
{
    public MedicamentosController(AppDbContext db) : base(db) { }

    protected override async Task<string> ValidarRelacionamentos(Medicamento med)
    {
            if (await Db.Idosos.CountAsync(i => i.Id == med.IdosoId) == 0)
            return $"Idoso com id {med.IdosoId} não existe.";
        return null;
    }
}
