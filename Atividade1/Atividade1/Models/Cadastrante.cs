using Microsoft.AspNetCore.Mvc;

namespace Atividade1.Models
{
    public class Cadastrante
    {
        public int CadastranteID { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public string Cpf { get; set; }
        public DateTime DataNascimento { get; set; }
        public int EventoID { get; set; }
        public Evento Evento { get; set; }
    }
}
