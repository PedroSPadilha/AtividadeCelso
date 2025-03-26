using Atividade1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class EventoController : Controller
{
    private readonly Context _context;

    public EventoController(Context context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        return View(_context.Eventos.Include(e => e.Cadastrantes));
    }

    public IActionResult Details(int id)
    {
        var evento = _context.Eventos
            .Include(e => e.Cadastrantes)
            .FirstOrDefault(e => e.EventoID == id);

        if (evento == null)
            return NotFound();

        return View(evento);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Evento evento)
    {
        _context.Add(evento);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }

    public IActionResult Edit(int id)
    {
        var evento = _context.Eventos.Find(id);
        if (evento == null)
            return NotFound();

        return View(evento);
    }

    [HttpPost]
    public IActionResult Edit(Evento evento)
    {
        _context.Entry(evento).State = EntityState.Modified;
        _context.SaveChanges();
        return RedirectToAction("Index");
    }

    public IActionResult Delete(int id)
    {
        var evento = _context.Eventos.Find(id);
        if (evento == null)
        {
            return NotFound();
        }
        return View(evento);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        var evento = _context.Eventos.Find(id);
        if (evento != null)
        {
            _context.Eventos.Remove(evento);
            _context.SaveChanges();
        }
        return RedirectToAction(nameof(Index));
    }

}
