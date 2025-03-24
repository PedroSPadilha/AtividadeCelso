using Atividade1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;


namespace Atividade1.Controllers
{
    public class CadastranteController : Controller
    {
        public Context _context;

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
                return BadRequest("Esse evento não tem certificado.");

            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Image", "certificado.jpg");

            if (!System.IO.File.Exists(path))
                return NotFound("Certificado não encontrado.");

            using (var image = Image.FromFile(path))
            using (var graphics = Graphics.FromImage(image))
            {
                var font = new Font("Arial", 25, FontStyle.Bold);
                var brush = new SolidBrush(Color.Black);

                float x = 3700f;
                float y = 3600f;

                graphics.DrawString(cadastrante.Nome, font, brush, new PointF(x, y));

                using (var ms = new MemoryStream())
                {
                    image.Save(ms, ImageFormat.Jpeg);
                    ms.Seek(0, SeekOrigin.Begin);
                    var fileBytes = ms.ToArray();
                    var contentType = "image/jpeg";

                    return File(fileBytes, contentType);
                }
            }
        }

        public IActionResult Create()
        {
            ViewBag.EventoID = new SelectList(_context.Eventos.OrderBy(e => e.Nome), "EventoID", "Nome");
            return View();
        }

        [HttpPost]
        public IActionResult Create(Cadastrante cadastrante)
        {
            _context.Add(cadastrante);
            _context.SaveChanges();
            return RedirectToAction("Index");
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
            _context.Entry(cadastrante).State = EntityState.Modified;
            _context.SaveChanges();
            return RedirectToAction("Index");
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
