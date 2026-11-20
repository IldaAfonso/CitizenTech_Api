using CitizenTech.Api.Data;
using CitizenTech.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CitizenTech.Api.Controllers;

/// <summary>
/// Base com as cinco rotas do CRUD (GET lista, GET por id, POST 201, PUT 204, DELETE 204).
/// Cada controller concreto só define as regras de relacionamento.
/// </summary>
public abstract class CrudController<T> : ControllerBase where T : class, IEntity
{
    protected readonly AppDbContext Db;
    protected CrudController(AppDbContext db) => Db = db;

    // Retorna a mensagem de erro (400) se alguma chave estrangeira não existir; null se estiver tudo certo.
    protected virtual Task<string> ValidarRelacionamentos(T entidade) => Task.FromResult<string>(null);

    protected IActionResult Erro400(string mensagem) => BadRequest(new { mensagem });
    protected IActionResult Erro404(int id) => NotFound(new { mensagem = $"{typeof(T).Name} com id {id} não encontrado." });

    [HttpGet]
    public async Task<ActionResult<IEnumerable<T>>> Listar() =>
        Ok(await Db.Set<T>().AsNoTracking().ToListAsync());

    [HttpGet("{id:int}")]
    public async Task<IActionResult> ObterPorId(int id)
    {
        var item = await Db.Set<T>().FindAsync(id);
        return item == null ? Erro404(id) : Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Criar(T entidade)
    {
        var erro = await ValidarRelacionamentos(entidade);
        if (erro != null) return Erro400(erro);

        entidade.Id = 0;
        Db.Set<T>().Add(entidade);
        try { await Db.SaveChangesAsync(); }
        catch (DbUpdateException) { return Erro400("Não foi possível salvar: verifique os dados (ex.: valor único já existente)."); }

        return CreatedAtAction(nameof(ObterPorId), new { id = entidade.Id }, entidade);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Atualizar(int id, T entidade)
    {
        if (entidade.Id != 0 && entidade.Id != id)
            return Erro400("O id da rota é diferente do id enviado no corpo.");

        var existente = await Db.Set<T>().FindAsync(id);
        if (existente == null) return Erro404(id);

        var erro = await ValidarRelacionamentos(entidade);
        if (erro != null) return Erro400(erro);

        entidade.Id = id;
        Db.Entry(existente).CurrentValues.SetValues(entidade);
        try { await Db.SaveChangesAsync(); }
        catch (DbUpdateException) { return Erro400("Não foi possível atualizar: verifique os dados."); }

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Excluir(int id)
    {
        var existente = await Db.Set<T>().FindAsync(id);
        if (existente == null) return Erro404(id);

        Db.Set<T>().Remove(existente);
        try { await Db.SaveChangesAsync(); }
        catch (DbUpdateException) { return Conflict(new { mensagem = "Não é possível excluir: existem registros relacionados." }); }

        return NoContent();
    }
}
