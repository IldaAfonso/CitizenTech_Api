using CitizenTech.Api.Data;
using CitizenTech.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CitizenTech.Api.Controllers;

[ApiController]
[Route("api/idosos")]
public class IdososController : CrudController<Idoso>
{
    public IdososController(AppDbContext db) : base(db) { }

    protected override async Task<string> ValidarRelacionamentos(Idoso idoso)
    {
         if (await Db.Cuidadores.CountAsync(c => c.Id == idoso.CuidadorId) == 0)
            return $"Cuidador com id {idoso.CuidadorId} não existe.";
        return null;
    }
}
