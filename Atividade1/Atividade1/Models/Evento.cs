using Microsoft.AspNetCore.Mvc;

namespace Atividade1.Models
{
    public class Evento
    {
        public int EventoID { get; set; }
        public string Nome { get; set; }
        public ICollection<Cadastrante> Cadastrantes { get; set; }
    }
}
