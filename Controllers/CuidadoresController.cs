using CitizenTech.Api.Data;
using CitizenTech.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace CitizenTech.Api.Controllers;

[ApiController]
[Route("api/cuidadores")]
public class CuidadoresController : CrudController<Cuidador>
{
    public CuidadoresController(AppDbContext db) : base(db) { }
}
