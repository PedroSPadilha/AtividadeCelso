using Atividade1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Atividade1.Controllers
{
    public class CadastranteController : Controller
    {
        private readonly Context _context;

        public CadastranteController(Context context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var lista = _context.Cadastrantes.Include(c => c.Evento).ToList();
            return View(lista);
        }

        public IActionResult Details(int id)
        {
            var cadastrante = _context.Cadastrantes
                .Include(c => c.Evento)
                .FirstOrDefault(c => c.CadastranteID == id);

            if (cadastrante == null)
                return NotFound();

            return View(cadastrante);
        }
        public IActionResult MostrarCertificado(int eventoId, int cadastranteId)
        {
            var cadastrante = _context.Cadastrantes
            .Include(c => c.Evento)
            .FirstOrDefault(c => c.CadastranteID == cadastranteId);

            if (cadastrante == null)
                return NotFound("Cadastrante não encontrado.");

            if (cadastrante.EventoID != 1)
                return BadRequest("Este participante não está inscrito neste evento.");


            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Image", "certificado.jpg");

            if (!System.IO.File.Exists(path))
                return NotFound("Certificado não encontrado.");

            var contentType = "image/jpeg";
            var fileBytes = System.IO.File.ReadAllBytes(path);

            return File(fileBytes, contentType);
        }

        public IActionResult Create()
        {
            ViewBag.EventoID = new SelectList(_context.Eventos.OrderBy(e => e.Nome), "EventoID", "Nome");
            return View();
        }

        [HttpPost]
        public IActionResult Create(Cadastrante cadastrante)
        {
            if (ModelState.IsValid)
            {
                _context.Cadastrantes.Add(cadastrante);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.EventoID = new SelectList(_context.Eventos.OrderBy(e => e.Nome), "EventoID", "Nome", cadastrante.EventoID);
            return View(cadastrante);
        }

        public IActionResult Edit(int id)
        {
            var cadastrante = _context.Cadastrantes.Find(id);
            if (cadastrante == null)
                return NotFound();

            ViewBag.EventoID = new SelectList(_context.Eventos.OrderBy(e => e.Nome), "EventoID", "Nome", cadastrante.EventoID);
            return View(cadastrante);
        }

        [HttpPost]
        public IActionResult Edit(Cadastrante cadastrante)
        {
            if (!ModelState.IsValid)
            {
                var erros = ModelState.Values.SelectMany(v => v.Errors).ToList();
                ViewBag.Erros = erros;
                ViewBag.EventoID = new SelectList(_context.Eventos.OrderBy(e => e.Nome), "EventoID", "Nome", cadastrante.EventoID);
                return View(cadastrante);
            }

            var original = _context.Cadastrantes.FirstOrDefault(c => c.CadastranteID == cadastrante.CadastranteID);
            if (original == null)
                return NotFound();

            original.Nome = cadastrante.Nome;
            original.EventoID = cadastrante.EventoID;

            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }


        public IActionResult Delete(int id)
        {
            var cadastrante = _context.Cadastrantes
                .Include(c => c.Evento)
                .FirstOrDefault(c => c.CadastranteID == id);

            if (cadastrante == null)
                return NotFound();

            return View(cadastrante);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var cadastrante = _context.Cadastrantes.Find(id);
            _context.Cadastrantes.Remove(cadastrante);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }

}
