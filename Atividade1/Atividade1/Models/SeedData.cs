using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Atividade1.Models
{
    public class SeedData
    {
        public static void EnsurePopulated(IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<Context>();

            context.Database.Migrate();

            if (!context.Eventos.Any())
            {
                var evento1 = new Evento { Nome = "Desenvolvimento Web Back-End: Fundamentos e Práticas" };
                var evento2 = new Evento { Nome = "Workshop de .NET Avançado" };

                context.Eventos.AddRange(evento1, evento2);
                context.SaveChanges();

                // Verifica se os IDs foram realmente gerados
                if (evento1.EventoID > 0 && evento2.EventoID > 0)
                {
                    var cadastrantes = new List<Cadastrante>
                {
                    new Cadastrante
                    {
                        Nome = "João Silva",
                        Email = "joao@example.com",
                        Telefone = "11912345678",
                        Cpf = "12345678900",
                        DataNascimento = new DateTime(1990, 5, 10),
                        EventoID = evento1.EventoID
                    },
                    new Cadastrante
                    {
                        Nome = "Maria Souza",
                        Email = "maria@example.com",
                        Telefone = "11998765432",
                        Cpf = "98765432100",
                        DataNascimento = new DateTime(1988, 12, 20),
                        EventoID = evento2.EventoID
                    }
                };

                    context.Cadastrantes.AddRange(cadastrantes);
                    context.SaveChanges();
                }
            }
        }
    }
}
