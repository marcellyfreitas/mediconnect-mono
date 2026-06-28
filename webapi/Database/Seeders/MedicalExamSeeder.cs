using WebApi.Models;
using WebApi.Database;
using Microsoft.EntityFrameworkCore;
using Bogus;

namespace WebApi.Database.Seeders
{
    public class MedicalExamSeeder : ISeeder
    {
        private readonly ApplicationDbContext _context;
        private readonly Faker _faker;

        public MedicalExamSeeder(ApplicationDbContext context)
        {
            _context = context;
            _faker = new Faker("pt_BR");
        }

        public async Task Seed()
        {
            if (!await _context.MedicalExams.AnyAsync())
            {
                var nomesExames = new[] { "Hemograma", "Raio-X", "Ressonância Magnética", "Ultrassonografia" };

                var exames = nomesExames.Select(nome => new MedicalExam
                {
                    NomeExame = nome,
                    DescricaoExame = _faker.Lorem.Sentence(10),
                    SetorResponsavel = _faker.Commerce.Department(),
                    TipoExame = _faker.Random.Word(),
                    PrazoEntrega = _faker.Date.Soon(7).ToShortDateString(),
                    RequisitosPreparo = _faker.Lorem.Sentence(10),
                    MaterialColeta = _faker.Lorem.Word()
                }).ToList();

                await _context.MedicalExams.AddRangeAsync(exames);
                await _context.SaveChangesAsync();

                Console.WriteLine("4 exames médicos cadastrados com sucesso.");
            }
        }
    }
}